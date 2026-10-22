using Microsoft.AspNetCore.Mvc;
using backend_II.Models;
using backend_II.Services;

namespace backend_II.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsServices _services;

        public ProductsController(ProductsServices services)
        {
            _services = services;
        }

        // GET: api/products
        // Consulta todos los productos activos
        [HttpGet]
        public async Task<GeneralResponse<List<Product>>> GetAll()
        {
            return await _services.GetAll();
        }

        // GET: api/products/{sku}
        // Consulta un producto por SKU pasado en la ruta
        [HttpGet("{sku}")]
        public async Task<GeneralResponse<Product>> GetByRoute([FromRoute] string sku)
        {
            return await _services.GetByRoute(sku);
        }

        // GET: api/products/search?sku=PROD-001
        // Consulta un producto por SKU pasado como Query Parameter
        [HttpGet("search")]
        public async Task<GeneralResponse<Product>> GetByQuery([FromQuery] string sku)
        {
            return await _services.GetByQuery(sku);
        }

        // GET: api/products/search/name?name=Laptop
        // Consulta productos por nombre pasado como Query Parameter
        [HttpGet("search/name")]
        public async Task<GeneralResponse<List<Product>>> GetByName([FromQuery] string name)
        {
            return await _services.GetByName(name);
        }

        // POST: api/products/search
        // Consulta un producto enviando criterios en el cuerpo (Body) en JSON
        [HttpPost("search")]
        public async Task<GeneralResponse<Product>> GetByBody([FromBody] ProductSearchRequest request)
        {
            return await _services.GetByBody(request);
        }
    }
}
