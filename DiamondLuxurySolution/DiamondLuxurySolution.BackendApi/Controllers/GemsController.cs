using DiamondLuxurySolution.Application.Repository.Gem;
using DiamondLuxurySolution.Application.Repository.Promotion;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.Promotion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GemsController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IGemRepo _gem;

        public GemsController(LuxuryDiamondShopContext context, IGemRepo gem)
        {
            _context = context;
            _gem = gem;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAllGem()
        {
            try
            {
                var status = await _gem.GetAll();
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

        [HttpPost("Create")]
        public async Task<ActionResult> CreateGem([FromForm] CreateGemRequest request)
        {
            try
            {
                var status = await _gem.CreateGem(request);
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
        public async Task<ActionResult> UpdateGem([FromForm] UpdateGemResquest request)
        {
            try
            {
                var status = await _gem.UpdateGem(request);
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
        public async Task<IActionResult> DeleteGem([FromQuery] DeleteGemRequest request)
        {
            try
            {
                var status = await _gem.DeleteGem(request);
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
        public async Task<IActionResult> FindById([FromQuery] Guid GemId)
        {
            try
            {
                var status = await _gem.GetGemById(GemId);
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


        [HttpGet("ViewCustomer")]
        public async Task<IActionResult> ViewAllGemPaginationInCustomer([FromQuery] ViewGemRequest request)
        {
            try
            {
                var status = await _gem.ViewGemInCustomer(request);
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

        [HttpGet("ViewManager")]
        public async Task<IActionResult> ViewAllGemPaginationManager([FromQuery] ViewGemRequest request)
        {
            try
            {
                var status = await _gem.ViewGemInManager(request);
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
