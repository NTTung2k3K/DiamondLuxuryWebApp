using DiamondLuxurySolution.Application.Repository.Discount;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.Discount;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IDiscountRepo _discount;

        public DiscountsController(LuxuryDiamondShopContext context, IDiscountRepo discount)
        {
            _context = context;
            _discount = discount;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateDiscount([FromForm] CreateDiscountRequest request)
        {
            try
            {
                var status = await _discount.CreateDiscount(request);
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

        [HttpPut("Update")]
        public async Task<ActionResult> UpdateDiscount([FromForm] UpdateDiscountRequest request)
        {
            try
            {
                var status = await _discount.UpdateDiscount(request);
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


        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteDiscount([FromBody] DeleteDiscountRequest request)
        {
            try
            {
                var status = await _discount.DeleteDiscount(request);
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

        [HttpGet("GetById")]
        public async Task<IActionResult> FindById([FromQuery] Guid DiscountId)
        {
            try
            {
                var status = await _discount.GetDiscountById(DiscountId);
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


        [HttpGet("ViewInCustomer")]
        public async Task<IActionResult> ViewAllDiscountPaginationInCustomer([FromQuery] ViewDiscountRequest request)
        {
            try
            {
                var status = await _discount.ViewDiscountInCustomer(request);
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

        [HttpGet("ViewInManager")]
        public async Task<IActionResult> ViewAllDiscountPagination([FromQuery] ViewDiscountRequest request)
        {
            try
            {
                var status = await _discount.ViewDiscountInManager(request);
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
