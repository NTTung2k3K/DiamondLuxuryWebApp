using Azure.Core;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.Promotion;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Gem
{
    public class GemRepo : IGemRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public GemRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateGem(CreateGemRequest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.GemName))
            {
                errorList.Add("Vui lòng nhập tên kim cương");
                //return new ApiErrorResult<bool>("Vui lòng nhập tên kim cương");
            }
            if (string.IsNullOrEmpty(request.Polish))
            {
                errorList.Add("Vui lòng nhập độ bóng");
                //return new ApiErrorResult<bool>("Vui lòng nhập độ bóng");
            }
            if (string.IsNullOrEmpty(request.Symetry))
            {
                errorList.Add("Vui lòng nhập độ đối xứng");
                //return new ApiErrorResult<bool>("Vui lòng nhập độ đối xứng");
            }
            if (request.Price < 0)
            {
                errorList.Add("Giá của kim cương phải > 0");
                //return new ApiErrorResult<bool>("Giá của kim cương phải > 0");
            }
            if (request.ProportionImage == null)
            {
                errorList.Add("Vui lòng gắn ImageProprortion");
                //return new ApiErrorResult<bool>("Vui lòng gắn Image");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }

            string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.ProportionImage);
            var gem = new DiamondLuxurySolution.Data.Entities.Gem
            {
                GemId = Guid.NewGuid(),
                GemName = request.GemName,
                Polish = request.Polish,
                Symetry = request.Symetry,
                IsOrigin = request.IsOrigin,
                IsMain = request.IsMain,
                Price = request.Price,
                Fluoresence = request.Fluoresence,
                ProportionImage = firebaseUrl,
                AcquisitionDate = request.AcquisitionDate,
                Active = request.Active,
            };
            _context.Gems.Add(gem);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> DeleteGem(DeleteGemRequest request)
        {
            var gem = await _context.Gems.FindAsync(request.GemId);
            if (gem == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy kim cương");
            }

            _context.Gems.Remove(gem);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<GemVm>> GetGemById(Guid GemId)
        {
            var gem = await _context.Gems.FindAsync(GemId);
            if (gem == null)
            {
                return new ApiErrorResult<GemVm>("Không tìm thấy kim cương");
            }
            var gemVm = new GemVm()
            {
                GemId = gem.GemId,
                GemName = gem.GemName,
                Polish = gem.Polish,
                Symetry = gem.Symetry,
                IsOrigin = gem.IsOrigin,
                IsMain = gem.IsMain,
                Price = gem.Price,
                Fluoresence = gem.Fluoresence,
                ProportionImage = gem.ProportionImage,

                AcquisitionDate = gem.AcquisitionDate,
                Active = gem.Active,
            };
            return new ApiSuccessResult<GemVm>(gemVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateGem(UpdateGemResquest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.GemName))
            {
                errorList.Add("Vui lòng nhập tên kim cương");
                //return new ApiErrorResult<bool>("Vui lòng nhập tên kim cương");
            }
            if (string.IsNullOrEmpty(request.Polish))
            {
                errorList.Add("Vui lòng nhập độ bóng");
                //return new ApiErrorResult<bool>("Vui lòng nhập độ bóng");
            }
            if (string.IsNullOrEmpty(request.Symetry))
            {
                errorList.Add("Vui lòng nhập độ đối xứng");
                //return new ApiErrorResult<bool>("Vui lòng nhập độ đối xứng");
            }
            if (request.Price < 0)
            {
                errorList.Add("Giá của kim cương phải > 0");
                //return new ApiErrorResult<bool>("Giá của kim cương phải > 0");
            }
            if (request.ProportionImage == null)
            {
                errorList.Add("Vui lòng gắn ImageProprortion");
                //return new ApiErrorResult<bool>("Vui lòng gắn Image");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            var gem = await _context.Gems.FindAsync(request.GemId);
            if (gem == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy kim cương");
            }
            string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.ProportionImage);
            gem.GemName = request.GemName;
            gem.Polish = request.Polish;
            gem.Symetry = request.Symetry;
            gem.IsOrigin = request.IsOrigin;
            gem.IsMain = request.IsMain;
            gem.Price = request.Price;
            gem.Fluoresence = request.Fluoresence;
            gem.ProportionImage = firebaseUrl;

            gem.AcquisitionDate = request.AcquisitionDate;
            gem.Active = request.Active;

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }
        public async Task<ApiResult<PageResult<GemVm>>> ViewGemInCustomer(ViewGemRequest request)
        {
            var listGem = await _context.Gems.ToListAsync();
            if (request.Keyword != null)
            {
                listGem = listGem.Where(x => x.GemName.Contains(request.Keyword)).ToList();

            }
            listGem = listGem.Where(x => x.Active is true).OrderByDescending(x => x.GemName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listGem.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listGemVm = listPaging.Select(gem => new GemVm()
            {
                GemId = gem.GemId,
                GemName = gem.GemName,
                Polish = gem.Polish,
                Symetry = gem.Symetry,
                IsOrigin = gem.IsOrigin,
                IsMain = gem.IsMain,
                Price = gem.Price,
                Fluoresence = gem.Fluoresence,
                ProportionImage = gem.ProportionImage,

                AcquisitionDate = gem.AcquisitionDate,
                Active = gem.Active,
            }).ToList();
            var listResult = new PageResult<GemVm>()
            {
                Items = listGemVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listGem.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<GemVm>>(listResult, "Success");
        }

        public async Task<ApiResult<PageResult<GemVm>>> ViewGemInManager(ViewGemRequest request)
        {
            var listGem = await _context.Gems.ToListAsync();
            if (request.Keyword != null)
            {
                listGem = listGem.Where(x => x.GemName.Contains(request.Keyword)).ToList();

            }
            listGem = listGem.OrderByDescending(x => x.GemName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listGem.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listGemVm = listPaging.Select(gem => new GemVm()
            {
                GemId = gem.GemId,
                GemName = gem.GemName,
                Polish = gem.Polish,
                Symetry = gem.Symetry,
                IsOrigin = gem.IsOrigin,
                IsMain = gem.IsMain,
                Price = gem.Price,
                Fluoresence = gem.Fluoresence,
                ProportionImage = gem.ProportionImage,
                AcquisitionDate = gem.AcquisitionDate,
                Active = gem.Active,
            }).ToList();
            var listResult = new PageResult<GemVm>()
            {
                Items = listGemVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listGem.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<GemVm>>(listResult, "Success");
        }
    }
}
