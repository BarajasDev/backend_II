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
        [HttpGet]
        public async Task<GeneralResponse<List<Product>>> GetAll()
        {
            return await _services.GetAll();
        }

        // GET: api/products/{sku}
        [HttpGet("{sku}")]
        //validaciones de tokens
        public async Task<GeneralResponse<Product>> GetByRoute([FromRoute] string sku)
        {
            //validaciones de claims
            return await _services.GetByRoute(sku);
        }

        // GET: api/products/search?sku=XYZ
        [HttpGet("search")]
        public async Task<GeneralResponse<Product>> GetByQuery([FromQuery] string sku)
        {
            return await _services.GetByQuery(sku);
        }

        // POST: api/products/search?name= alexis
        [HttpPost("search")]
        public async Task<GeneralResponse<Product>> GetByBody([FromBody] ProductSearchRequest request)
        {
            return await _services.GetByBody(request);
        }
    }
}
