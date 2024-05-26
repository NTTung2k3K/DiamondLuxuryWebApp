using DiamondLuxurySolution.Application.Repository.Gem;
using DiamondLuxurySolution.Application.Repository.Product;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IProductRepo _product;

        public ProductsController(LuxuryDiamondShopContext context, IProductRepo product)
        {
            _context = context;
            _product = product;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateProduct([FromForm] CreateProductRequest request)
        {
            try
            {
                var status = await _product.CreateProduct(request);
                if (status.IsSuccessed)
                {
                    return Ok(status);
                }
                return BadRequest(status);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
