using DiamondLuxurySolution.Application.Repository.Gem;
using DiamondLuxurySolution.Application.Repository.SubGem;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.SubGem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubGemsController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly ISubGemRepo _subGem;

        public SubGemsController(LuxuryDiamondShopContext context, ISubGemRepo Subgem)
        {
            _context = context;
            _subGem = Subgem;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateSubGem([FromForm] CreateSubGemRequest request)
        {
            try
            {
                var status = await _subGem.CreateSubGem(request);
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
        public async Task<ActionResult> UpdateSubGem([FromForm] UpdateSubGemRequest request)
        {
            try
            {
                var status = await _subGem.UpdateSubGem(request);
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
        public async Task<IActionResult> DeleteSubGem([FromBody] DeleteSubGemRequest request)
        {
            try
            {
                var status = await _subGem.DeleteSubGem(request);
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
        public async Task<IActionResult> FindById([FromQuery] Guid SubGemId)
        {
            try
            {
                var status = await _subGem.GetSubGemById(SubGemId);
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


        [HttpGet("ViewSubGemsInCustomer")]
        public async Task<IActionResult> ViewAllSubGemsInCustomer([FromQuery] ViewSubGemRequest request)
        {
            try
            {
                var status = await _subGem.ViewSubGemInCustomer(request);
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

        [HttpGet("ViewSubGemsInManager")]
        public async Task<IActionResult> ViewAllSubGemsInManager([FromQuery] ViewSubGemRequest request)
        {
            try
            {
                var status = await _subGem.ViewSubGemInManager(request);
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
