using DiamondLuxurySolution.Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Platform
{
    public class PlatformInitialize : IPlatformInitialize
    {
        private readonly LuxuryDiamondShopContext _context;
        public PlatformInitialize(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task CreateDefaultAbout()
        {
            var platform = new DiamondLuxurySolution.Data.Entities.Platform {
                PlatformName = "Facebook",
                PlatformUrl = "https://www.facebook.com/diamondluxurystore",
                PlatformLogo = "https://firebasestorage.googleapis.com/v0/b/diamondluxuryshop-980cd.appspot.com/o/images%2FScreenshot%202024-06-23%20235215.png?alt=media&token=7bc28a55-f84f-4832-9602-86307ed4e9ff",
                Status = true
            };
            var checkExist = await _context.Platforms.Where(x => x.PlatformName == platform.PlatformName).ToListAsync();
            if (!checkExist.Any())
            {
                _context.Platforms.Add(platform);
                await _context.SaveChangesAsync();
            }
        }
    }
}
