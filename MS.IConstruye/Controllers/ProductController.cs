using Microsoft.AspNetCore.Mvc;
using MS.IConstruye.Application;
using MS.IConstruye.Service;
using System;
using System.Net;
using System.Threading.Tasks;

namespace MS.IConstruye.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IProductQuery _productQuery;

        public ProductController(
            IMemoryCacheService memoryCacheService,
            IProductQuery productQuery
            )
        {
            _memoryCacheService = memoryCacheService ?? throw new ArgumentNullException(nameof(memoryCacheService));
            _productQuery = productQuery ?? throw new ArgumentNullException(nameof(productQuery));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            if (!_memoryCacheService.TryGetValue($"{ProductConstant.ProductMemory}_{id}", out ProductViewModel product))
            {
                product = await _productQuery.Get(id);
                _memoryCacheService.SetValue($"{ProductConstant.ProductMemory}_{id}", product);
            }
            return Ok(product);
        }
    }
}
