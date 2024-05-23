using DiamondLuxurySolution.Application.Repository.Promotion;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.Promotion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IPromotionRepo _promotion;

        public PromotionsController(LuxuryDiamondShopContext context, IPromotionRepo promotion)
        {
            _context = context;
            _promotion = promotion;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreatePromotion([FromForm] CreatePromotionRequest request)
        {
            try
            {
                var status = await _promotion.CreatePromotion(request);
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
        public async Task<ActionResult> UpdatePromotion([FromForm] UpdatePromotionRequest request)
        {
            try
            {
                var status = await _promotion.UpdatePromotion(request);
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
        public async Task<IActionResult> DeletePromotion([FromBody] DeletePromotionRequest request)
        {
            try
            {
                var status = await _promotion.DeletePromotion(request);
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
        public async Task<IActionResult> FindById([FromQuery] Guid PromotionId)
        {
            try
            {
                var status = await _promotion.GetPromotionById(PromotionId);
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


        [HttpGet("View")]
        public async Task<IActionResult> ViewAllPromotionPagination([FromQuery] ViewPromotionRequest request)
        {
            try
            {
                var status = await _promotion.ViewPromotion(request);
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
