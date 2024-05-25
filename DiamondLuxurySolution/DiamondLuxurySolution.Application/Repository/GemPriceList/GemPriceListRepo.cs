using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.GemPriceList;
using DiamondLuxurySolution.ViewModel.Models.News;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.GemPriceList
{
    public class GemPriceListRepo : IGemPriceListRepo
    {
        private readonly LuxuryDiamondShopContext _context;

        public GemPriceListRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateGemPriceList(CreateGemPriceListRequest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.CaratWeight.Trim()))
            {
                errorList.Add("Vui lòng nhập trọng lượng");
            }
            if (string.IsNullOrEmpty(request.Clarity.Trim()))
            {
                errorList.Add("Vui lòng nhập độ tinh khiết");
            }
            if (string.IsNullOrEmpty(request.Cut.Trim()))
            {
                errorList.Add("Vui lòng nhập giác cắt");
            }
            if (string.IsNullOrEmpty(request.Color.Trim()))
            {
                errorList.Add("Vui lòng nhập màu sắc");
            }
            if (string.IsNullOrEmpty(request.Price.ToString().Trim()) || request.Price <= 0)
            {
                errorList.Add("Vui lòng nhập giá lớn hơn 0");
            }
            var gem = await _context.Gems.FindAsync(request.GemId);
            if (gem == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy kim cương");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            var gemPriceList = new Data.Entities.GemPriceList
            {
                CaratWeight = request.CaratWeight,
                Clarity = request.Clarity,
                Color = request.Color,
                Price = request.Price,
                Cut = request.Cut,
                GemId = request.GemId,
                Gem = gem,
                Active = request.Active
            };

            _context.GemPriceLists.Add(gemPriceList);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");

        }

        public async Task<ApiResult<bool>> DeleteGemPriceList(DeleteGemPriceListRequest request)
        {
            var gemPriceList = await _context.GemPriceLists.FindAsync(request.GemPriceListId);
            if (gemPriceList == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy bảng giá kim cương");
            }
            _context.GemPriceLists.Remove(gemPriceList);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<GemPriceListVm>> GetGemPriceListById(int GemPriceListId)
        {
            var GemPriceList = await _context.GemPriceLists.FindAsync(GemPriceListId);
            if(GemPriceList == null)
            {
                return new ApiErrorResult<GemPriceListVm>("Không tìm thấy bảng giá kim cương");
            }
            var gem = await _context.Gems.FindAsync(GemPriceList.GemId);
            var gemPriceListVm = new GemPriceListVm
            {
                GemPriceListId = GemPriceListId,
                CaratWeight = GemPriceList.CaratWeight,
                Clarity = GemPriceList.Clarity,
                Color = GemPriceList.Color,
                Cut = GemPriceList.Cut,
                Price = GemPriceList.Price,
                Active = GemPriceList.Active,
                GemVm = gem
            };
            return new ApiSuccessResult<GemPriceListVm>(gemPriceListVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateGemPriceList(UpdateGemPriceListRequest request)
        {
            var GemPriceList = await _context.GemPriceLists.FindAsync(request.GemPriceListId);
            if (GemPriceList == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy bảng giá kim cương");
            }
            if (!string.IsNullOrEmpty(request.CaratWeight?.Trim()))
            {
                GemPriceList.CaratWeight = request.CaratWeight;
            }
            if (!string.IsNullOrEmpty(request.Cut?.Trim()))
            {
                GemPriceList.Cut = request.Cut;
            }
            if (!string.IsNullOrEmpty(request.Clarity?.Trim()))
            {
                GemPriceList.Clarity = request.Clarity;
            }
            if (!string.IsNullOrEmpty(request.Color?.Trim()))
            {
                GemPriceList.Color = request.Color;
            }
            if (!string.IsNullOrEmpty(request.Price?.ToString().Trim()))
            {
                GemPriceList.Price = (decimal)request.Price;
            }

            if (GemPriceList.GemId != request.GemId)
            {
                var gem = await _context.Gems.FindAsync(request.GemId);
                if (gem == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy kim cương");
                }
                GemPriceList.GemId = gem.GemId;
                GemPriceList.Gem = gem;
            }
            GemPriceList.Active = request.Active;
            await _context.SaveChangesAsync();

            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<GemPriceListVm>>> ViewGemPriceListInCustomer(ViewGemPriceListRequest request)
        {
            var listGemPriceList = await _context.GemPriceLists.ToListAsync();
            if (request.Keyword != null)
            {
                listGemPriceList = listGemPriceList.Where(x => x.CaratWeight.Contains(request.Keyword)).ToList();

            }
            listGemPriceList = listGemPriceList.Where(x => x.Active == true).OrderByDescending(x => x.CaratWeight).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listGemPriceList.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();
            var listGemPriceListVm = new List<GemPriceListVm>();
            foreach (var item in listPaging)
            {
                var gem = await _context.Gems.FindAsync(item.GemId);
                var gemPriceListVm = new GemPriceListVm()
                {
                    GemPriceListId = item.GemPriceListId,
                    CaratWeight = item.CaratWeight,
                    Clarity = item.Clarity,
                    Cut = item.Cut,
                    Color = item.Color,
                    Price = item.Price,
                    Active = item.Active,
                    GemVm = gem
                };
                listGemPriceListVm.Add(gemPriceListVm);
            }
            var listResult = new PageResult<GemPriceListVm>()
            {
                Items = listGemPriceListVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listGemPriceList.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<GemPriceListVm>>(listResult, "Success");
        }

        public async Task<ApiResult<PageResult<GemPriceListVm>>> ViewGemPriceListInManager(ViewGemPriceListRequest request)
        {
            var listGemPriceList = await _context.GemPriceLists.ToListAsync();
            if (request.Keyword != null)
            {
                listGemPriceList = listGemPriceList.Where(x => x.CaratWeight.Contains(request.Keyword)).ToList();

            }
            listGemPriceList = listGemPriceList.OrderByDescending(x => x.CaratWeight).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listGemPriceList.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listGemPriceListVm = new List<GemPriceListVm>();
            foreach (var item in listPaging)
            {
                var gem = await _context.Gems.FindAsync(item.GemId);
                var gemPriceListVm = new GemPriceListVm()
                {
                    GemPriceListId = item.GemPriceListId,
                    CaratWeight = item.CaratWeight,
                    Clarity = item.Clarity,
                    Cut = item.Cut,
                    Color = item.Color,
                    Price = item.Price,
                    Active = item.Active,
                    GemVm = gem
                };
                listGemPriceListVm.Add(gemPriceListVm);
            }
            var listResult = new PageResult<GemPriceListVm>()
            {
                Items = listGemPriceListVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listGemPriceList.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<GemPriceListVm>>(listResult, "Success");
        }
    }
}
