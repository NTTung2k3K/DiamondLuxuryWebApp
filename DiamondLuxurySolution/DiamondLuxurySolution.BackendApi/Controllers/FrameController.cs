using DiamondLuxurySolution.Application.Repository.About;
using DiamondLuxurySolution.Application.Repository.Frame;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Frame;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrameController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IFrameRepo _frame;

        public FrameController(LuxuryDiamondShopContext context, IFrameRepo frame)
        {
            _context = context;
            _frame = frame;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateFrame([FromBody] CreateFrameRequest request)
        {
            try
            {
                var status = await _frame.CreateFrame(request);
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
        public async Task<ActionResult> UpdateFrame([FromBody] UpdateFrameRequest request)
        {
            try
            {
                var status = await _frame.UpdateFrame(request);
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
        public async Task<IActionResult> DeleteFrame([FromQuery] DeleteFrameRequest request)
        {
            try
            {
                var status = await _frame.DeleteFrame(request);
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
        public async Task<IActionResult> FindById([FromQuery] string FrameId)
        {
            try
            {
                var status = await _frame.GetFrameById(FrameId);
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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var status = await _frame.GetAll();
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


        [HttpGet("ViewInFrame")]
        public async Task<IActionResult> ViewAllFramePagination([FromQuery] ViewFrameRequest request)
        {
            try
            {
                var status = await _frame.ViewFrameInPaging(request);
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
