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

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IPlatform _platform;

        public PlatformsController(LuxuryDiamondShopContext context,IPlatform platform)
        {
            _context = context;
            _platform = platform;
        }

       

        // POST: api/Platforms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
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


        // DELETE: api/Platforms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlatform(int id)
        {
            var platform = await _context.Platforms.FindAsync(id);
            if (platform == null)
            {
                return NotFound();
            }

            _context.Platforms.Remove(platform);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlatformExists(int id)
        {
            return _context.Platforms.Any(e => e.PlatformId == id);
        }
    }
}
