using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.Application.Repository.Platform;
using Azure.Core;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IPlatformRepo _platform;

        public PlatformsController(LuxuryDiamondShopContext context,IPlatformRepo platform)
        {
            _context = context;
            _platform = platform;
        }

       

        [HttpPost("Create")]
        public async Task<ActionResult> CreatePlatform([FromForm]CreatePlatformRequest request)
        {
            try
            {
                var status = await _platform.CreatePlatform(request);
                if(status.IsSuccessed)
                {
                    return Ok(status);
                }
                return BadRequest(status);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<ActionResult> UpdatePlatform([FromForm] UpdatePlatformRequest request)
        {
            try
            {
                var status = await _platform.UpdatePlatform(request);
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
        public async Task<IActionResult> DeletePlatform([FromBody]DeletePlatformRequest request)
        {
            try
            {
                var status = await _platform.DeletePlatform(request);
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
        public async Task<IActionResult> FindById([FromQuery]int PlatformId)
        {
            try
            {
                var status = await _platform.GetPlatfromById(PlatformId);
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
        public async Task<IActionResult> ViewAllPlatformPagination([FromQuery]ViewPlatformRequest request)
        {
            try
            {
                var status = await _platform.ViewPlatfrom(request);
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
