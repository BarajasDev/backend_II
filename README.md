# Backend II — Práctica de Servicios (Capa de Servicios)

**Institución:** Centro de Estudios Superiores del Noroeste (CESUN Universidad)  
**Carrera:** Ingeniería en Desarrollo de Software  
**Grupo:** 08IDESVA  
**Materia:** Backend II  
**Maestro:** Anselmo Alexis Gonzalez Diaz  
**Alumno:** Jahaziel Barajas Avila  

---

## 📌 Descripción del Proyecto

Este proyecto consiste en una **API REST desarrollada en ASP.NET Core (.NET 9)** para un sistema de comercio electrónico B2B. En esta práctica se implementa formalmente la **Capa de Servicios (`ProductsServices`)**, desacoplando la lógica de negocio y las validaciones de los controladores HTTP y centralizando la interacción con el repositorio de datos (`ProductRepository`).

---

## 🏗️ Arquitectura en Capas

El flujo de cada petición HTTP sigue el patrón arquitectónico en capas:

```
[ Cliente / Postman / Swagger ]
             │
             ▼  (HTTP Request / JSON)
 ┌──────────────────────────────────────┐
 │    Controlador (ProductsController)   │  <- Solo maneja HTTP, rutas y parámetros
 └──────────────────┬───────────────────┘
                    │
                    ▼  (Inyección de Dependencias)
 ┌──────────────────────────────────────┐
 │      Servicio (ProductsServices)     │  <- Lógica de negocio, validaciones y formato GeneralResponse
 └──────────────────┬───────────────────┘
                    │
                    ▼  (Inyección de Dependencias)
 ┌──────────────────────────────────────┐
 │    Repositorio (ProductRepository)   │  <- Acceso a datos con ADO.NET (SqlConnection / SqlCommand)
 └──────────────────┬───────────────────┘
                    │
                    ▼  (ExecuteReaderAsync)
 ┌──────────────────────────────────────┐
 │      Base de Datos (SQL Server)      │  <- Stored Procedure: sp_GetAllActiveProducts
 └──────────────────────────────────────┘
```

### ¿Por qué implementar la Capa de Servicios?
1. **Principio de Responsabilidad Única (SRP):** El controlador solo se encarga de recibir peticiones HTTP y devolver respuestas; ya no contiene lógica de negocio ni manipula directamente la base de datos.
2. **Reutilización y Mantenibilidad:** La lógica para validar parámetros y formatear respuestas puede reutilizarse desde otros controladores, tareas en segundo plano o pruebas unitarias.
3. **Desacoplamiento:** Si la lógica de negocio cambia, no afectamos las rutas ni la capa de persistencia.

---

## ⚙️ Registro de Dependencias (`Program.cs`)

Los servicios y repositorios se registran en el contenedor de inversión de control con ciclo de vida **Scoped** (una instancia por cada petición HTTP):

```csharp
// Registro del repositorio y del servicio para inyección de dependencias
builder.Services.AddScoped<backend_II.Database.ProductRepository>();
builder.Services.AddScoped<backend_II.Services.ProductsServices>();
```

---

## 📡 Endpoints Disponibles

Todos los endpoints retornan una estructura unificada `GeneralResponse<T>` con los campos `Success`, `Message` y `Data`.

| Método | Endpoint | Parámetro | Descripción |
| :---: | :--- | :--- | :--- |
| **GET** | `/api/products` | Ninguno | Obtiene la lista completa de productos activos. |
| **GET** | `/api/products/{sku}` | En ruta (`sku`) | Consulta el detalle de un producto por su SKU. |
| **GET** | `/api/products/search?sku={sku}` | Query string (`sku`) | Consulta un producto mediante parámetro de consulta SKU. |
| **GET** | `/api/products/search/name?name={name}` | Query string (`name`) | Consulta y filtra productos activos por coincidencia de nombre. |
| **POST** | `/api/products/search` | Body JSON (`ProductSearchRequest`) | Consulta enviando criterios en el cuerpo (ej. `{ "sku": "PROD-001" }` o `{ "name": "Laptop" }`). |

---

## 📦 Modelo de Respuesta Estándar

```json
{
  "success": true,
  "message": "Productos obtenidos exitosamente.",
  "data": [
    {
      "productId": 1,
      "sku": "PROD-001",
      "name": "Laptop Dell Inspiron 15",
      "description": "Portátil de alta gama para oficina y desarrollo",
      "price": 14500.00,
      "stockQuantity": 25
    }
  ]
}
```

---

## 🚀 Cómo Ejecutar el Proyecto

### Requisitos Previos
- [.NET 9 SDK](https://dotnet.microsoft.com/) instalado.
- Servidor SQL Server con la base de datos `e_commerce` y el stored procedure `sp_GetAllActiveProducts` ejecutado.
- Fichero `.env` configurado con la cadena de conexión:
  ```env
  DB_CONNECTION_STRING=Server=localhost;Database=e_commerce;Trusted_Connection=True;TrustServerCertificate=True;
  ```

### Pasos de Ejecución
1. Clonar el repositorio:
   ```bash
   git clone https://github.com/BarajasDev/backend_II.git
   cd backend_II
   ```
2. Restaurar dependencias y compilar:
   ```bash
   dotnet restore
   dotnet build
   ```
3. Iniciar la API:
   ```bash
   dotnet run
   ```
4. Probar los endpoints en `https://localhost:7000/api/products` o mediante Postman.
