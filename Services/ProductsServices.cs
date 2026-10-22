using backend_II.Models;
using backend_II.Database;

namespace backend_II.Services
{
    public class ProductsServices
    {
        private readonly ProductRepository _repository;

        public ProductsServices(ProductRepository repository)
        {
            _repository = repository;
        }

        // Obtener todos los productos activos
        public async Task<GeneralResponse<List<Product>>> GetAll()
        {
            try
            {
                var products = await _repository.GetAllActiveProductsAsync();

                return new GeneralResponse<List<Product>>
                {
                    Success = true,
                    Message = "Productos obtenidos exitosamente.",
                    Data = products
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<Product>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        // Obtener un producto por SKU recibido en la ruta (URL)
        public async Task<GeneralResponse<Product>> GetByRoute(string sku)
        {
            try
            {
                var product = await _repository.GetProductBySkuAsync(sku);

                if (product == null)
                {
                    return new GeneralResponse<Product>
                    {
                        Success = false,
                        Message = "Producto no encontrado o inactivo.",
                        Data = null
                    };
                }

                return new GeneralResponse<Product>
                {
                    Success = true,
                    Message = "Producto encontrado.",
                    Data = product
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<Product>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        // Obtener un producto por SKU recibido como parámetro de consulta (Query param)
        public async Task<GeneralResponse<Product>> GetByQuery(string sku)
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                return new GeneralResponse<Product>
                {
                    Success = false,
                    Message = "El parámetro 'sku' en la URL es obligatorio.",
                    Data = null
                };
            }

            try
            {
                var product = await _repository.GetProductBySkuAsync(sku);

                if (product == null)
                {
                    return new GeneralResponse<Product>
                    {
                        Success = false,
                        Message = "Producto no encontrado o inactivo.",
                        Data = null
                    };
                }

                return new GeneralResponse<Product>
                {
                    Success = true,
                    Message = "Producto encontrado.",
                    Data = product
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<Product>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        // Obtener productos filtrados por nombre (búsqueda por nombre)
        public async Task<GeneralResponse<List<Product>>> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new GeneralResponse<List<Product>>
                {
                    Success = false,
                    Message = "El parámetro 'name' en la URL es obligatorio.",
                    Data = null
                };
            }

            try
            {
                var allProducts = await _repository.GetAllActiveProductsAsync();
                var filtered = allProducts
                    .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (filtered.Count == 0)
                {
                    return new GeneralResponse<List<Product>>
                    {
                        Success = false,
                        Message = $"No se encontraron productos activos que coincidan con '{name}'.",
                        Data = null
                    };
                }

                return new GeneralResponse<List<Product>>
                {
                    Success = true,
                    Message = "Productos encontrados exitosamente.",
                    Data = filtered
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<List<Product>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        // Obtener producto mediante objeto enviado en el cuerpo (Body) de la petición
        public async Task<GeneralResponse<Product>> GetByBody(ProductSearchRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Sku) && string.IsNullOrWhiteSpace(request.Name))
            {
                return new GeneralResponse<Product>
                {
                    Success = false,
                    Message = "Debes ingresar al menos el atributo 'sku' o 'name' en el cuerpo (Body) de la petición.",
                    Data = null
                };
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(request.Sku))
                {
                    var product = await _repository.GetProductBySkuAsync(request.Sku);

                    if (product == null)
                    {
                        return new GeneralResponse<Product>
                        {
                            Success = false,
                            Message = "Producto no encontrado o inactivo.",
                            Data = null
                        };
                    }

                    return new GeneralResponse<Product>
                    {
                        Success = true,
                        Message = "Producto encontrado.",
                        Data = product
                    };
                }
                else
                {
                    var allProducts = await _repository.GetAllActiveProductsAsync();
                    var product = allProducts
                        .FirstOrDefault(p => p.Name.Contains(request.Name!, StringComparison.OrdinalIgnoreCase));

                    if (product == null)
                    {
                        return new GeneralResponse<Product>
                        {
                            Success = false,
                            Message = $"Producto con nombre '{request.Name}' no encontrado o inactivo.",
                            Data = null
                        };
                    }

                    return new GeneralResponse<Product>
                    {
                        Success = true,
                        Message = "Producto encontrado.",
                        Data = product
                    };
                }
            }
            catch (Exception ex)
            {
                return new GeneralResponse<Product>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
    }
}
