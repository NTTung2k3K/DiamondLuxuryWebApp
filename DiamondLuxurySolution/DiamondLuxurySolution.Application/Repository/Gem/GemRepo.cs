using Azure.Core;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
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
            if (string.IsNullOrEmpty(request.GemName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên kim cương");
            }
            
            var gem = new DiamondLuxurySolution.Data.Entities.Gem
            {
                GemId = Guid.NewGuid(),
                GemName = request.GemName,
                Polish = request.Polish != null ? request.Polish : "",
                Symetry = request.Symetry != null ? request.Symetry : "",
                IsOrigin = request.IsOrigin,
<<<<<<< HEAD
                Price = request.Price,
=======
>>>>>>> 59348e02b106350021dbba36fe0bb84fc3d839e4
                Fluoresence = request.Fluoresence,
                AcquisitionDate = request.AcquisitionDate,
                Active = request.Active,
            };
            if (request.ProportionImage != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.ProportionImage);
                gem.ProportionImage = firebaseUrl;
            } else
            {
                gem.ProportionImage = "";
            }
            if (request.GemImage != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.GemImage);
                gem.GemImage = firebaseUrl;
            }
            else
            {
                gem.GemImage = "";
            }

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
<<<<<<< HEAD
                Price = gem.Price,
=======
                GemImage = gem.GemImage,
>>>>>>> 59348e02b106350021dbba36fe0bb84fc3d839e4
                Fluoresence = gem.Fluoresence,
                ProportionImage = gem.ProportionImage,
                AcquisitionDate = gem.AcquisitionDate,
                Active = gem.Active,
            };
            return new ApiSuccessResult<GemVm>(gemVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateGem(UpdateGemResquest request)
        {
            if (string.IsNullOrEmpty(request.GemName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên kim cương");
            }
            var gem = await _context.Gems.FindAsync(request.GemId);
            if (gem == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy kim cương");
            }
            
            gem.GemName = request.GemName;
            gem.Polish = request.Polish != null ? request.Polish : "";
            gem.Symetry = request.Symetry != null ? request.Symetry : "";
            gem.IsOrigin = request.IsOrigin;
<<<<<<< HEAD
            gem.Price = request.Price;
=======
>>>>>>> 59348e02b106350021dbba36fe0bb84fc3d839e4
            gem.Fluoresence = request.Fluoresence;
            gem.AcquisitionDate = request.AcquisitionDate;
            gem.Active = request.Active;
            if (request.ProportionImage != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.ProportionImage);
                gem.ProportionImage = firebaseUrl;
            } else
            {
                gem.ProportionImage = "";
            }
            if (request.GemImage != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.GemImage);
                gem.GemImage = firebaseUrl;
            }
            else
            {
                gem.GemImage = "";
            }

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
<<<<<<< HEAD
                Price = gem.Price,
=======
                GemImage = gem.GemImage,
>>>>>>> 59348e02b106350021dbba36fe0bb84fc3d839e4
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
<<<<<<< HEAD
                Price = gem.Price,
=======
                GemImage = gem.GemImage,
>>>>>>> 59348e02b106350021dbba36fe0bb84fc3d839e4
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
