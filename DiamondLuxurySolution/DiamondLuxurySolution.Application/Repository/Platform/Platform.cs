using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Platform
{
    public class Platform : IPlatform
    {
        private readonly LuxuryDiamondShopContext _context;
        public Platform(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreatePlatform(CreatePlatformRequest request)
        {
            string firebaseUrl = "";
            if (request.PlatformLogo != null)
            {
               firebaseUrl  = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.PlatformLogo);
            }
            var platform = new DiamondLuxurySolution.Data.Entities.Platform
            {
                PlatformName = request.PlatformName,
                PlatformLogo = firebaseUrl,
                PlatformUrl = request.PlatformUrl,
            };
            _context.Platforms.Add(platform);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true);
        }
    }
}
