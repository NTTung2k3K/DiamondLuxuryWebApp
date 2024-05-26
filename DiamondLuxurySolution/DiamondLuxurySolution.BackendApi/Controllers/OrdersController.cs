using DiamondLuxurySolution.Application.Repository.News;
using DiamondLuxurySolution.Application.Repository.Order;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.News;
using DiamondLuxurySolution.ViewModel.Models.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IOrderRepo _order;

        public OrdersController(LuxuryDiamondShopContext context, IOrderRepo order)
        {
            _context = context;
            _order = order;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var status = await _order.CreateOrder(request);
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
