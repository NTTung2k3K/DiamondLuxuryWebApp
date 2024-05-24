using DiamondLuxurySolution.Application.Repository.Platform;
using DiamondLuxurySolution.Application.Repository.Slide;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Slide;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidesController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly ISlideRepo _slide;

        public SlidesController(LuxuryDiamondShopContext context, ISlideRepo slide)
        {
            _context = context;
            _slide = slide;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateSlide([FromForm] CreateSlideRequest request)
        {
            try
            {
                var status = await _slide.CreateSlide(request);
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
        public async Task<ActionResult> UpdateSlide([FromForm] UpdateSlideRequest request)
        {
            try
            {
                var status = await _slide.UpdateSlide(request);
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
        public async Task<IActionResult> DeleteSlide([FromBody] DeleteSlideRequest request)
        {
            try
            {
                var status = await _slide.DeleteSlide(request);
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
        public async Task<IActionResult> FindById([FromQuery] int SlideId)
        {
            try
            {
                var status = await _slide.GetSlideById(SlideId);
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
        public async Task<IActionResult> ViewAllSlidesInCustomer([FromQuery] ViewSlideRequest request)
        {
            try
            {
                var status = await _slide.ViewSlidesInCustomer(request);
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
        public async Task<IActionResult> ViewAllSlidesInManager([FromQuery] ViewSlideRequest request)
        {
            try
            {
                var status = await _slide.ViewSlidesInManager(request);
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
