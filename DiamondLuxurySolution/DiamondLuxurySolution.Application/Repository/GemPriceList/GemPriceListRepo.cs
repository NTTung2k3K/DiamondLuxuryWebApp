using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.GemPriceList;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.News;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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

            List<string> errorList = new List<string>();

            decimal price = 0;
            try
            {
                // Loại bỏ dấu phân cách hàng nghìn và thay dấu thập phân (nếu cần)
                string processedPrice = request.Price.Replace(".", "").Replace(",", ".");

                // Chuyển đổi chuỗi đã xử lý sang kiểu decimal
                if (decimal.TryParse(processedPrice, out price))
                {
                    if (price <= 0)
                    {
                        errorList.Add("Giá kim cương phải lớn hơn 0");
                    }
                }
                else
                {
                    errorList.Add("Giá kim cương không hợp lệ");
                }
            }
            catch (FormatException)
            {
                errorList.Add("Giá kim cương không hợp lệ");
            }

            if (request.effectDate < DateTime.Today.AddDays(-7) || request.effectDate > DateTime.Today)
            {
                errorList.Add("Bảng giá kim cương phải được cập nhật trong khoảng thời gian gần đây.");
            }


            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }

            var gemPriceList = new DiamondLuxurySolution.Data.Entities.GemPriceList
            {
                CaratWeight = !string.IsNullOrWhiteSpace(request.CaratWeight) ? request.CaratWeight : "",
                Clarity = request.Clarity != null ? request.Clarity : "",
                Color = request.Color != null ? request.Color : "",
                Price = price,
                Cut = request.Cut != null ? request.Cut : "",
                Active = request.Active,
                effectDate = (DateTime)request.effectDate
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

        public async Task<ApiResult<List<GemPriceListVm>>> GetAll()
        {
            var list = await _context.GemPriceLists.ToListAsync();
            var listGemPriceListVm = new List<GemPriceListVm>();

            foreach (var item in list)
            {
                var gemPriceListVm = new GemPriceListVm
                {
                    GemPriceListId = item.GemPriceListId,
                    Active = item.Active,
                    CaratWeight = item.CaratWeight,
                    Clarity = item.Clarity,
                    Color = item.Color,
                    Cut = item.Cut,
                    effectDate = item.effectDate,
                    Price = (decimal)item.Price,
                };
                listGemPriceListVm.Add(gemPriceListVm);
            }
            return new ApiSuccessResult<List<GemPriceListVm>>(listGemPriceListVm.ToList());
        }

        public async Task<ApiResult<GemPriceListVm>> GetGemPriceListById(int GemPriceListId)
        {
            var GemPriceList = await _context.GemPriceLists.FindAsync(GemPriceListId);
            if (GemPriceList == null)
            {
                return new ApiErrorResult<GemPriceListVm>("Không tìm thấy bảng giá kim cương");
            }
            var gemPriceListVm = new GemPriceListVm
            {
                GemPriceListId = GemPriceListId,
                CaratWeight = GemPriceList.CaratWeight,
                Clarity = GemPriceList.Clarity,
                Color = GemPriceList.Color,
                Cut = GemPriceList.Cut,
                Price = (decimal)GemPriceList.Price,
                effectDate = GemPriceList.effectDate,
                Active = GemPriceList.Active,
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

            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.Price))
            {
                errorList.Add("Vui lòng nhập giá kim cương");
            }

            decimal price = 0;
            try
            {
                // Loại bỏ dấu phân cách hàng nghìn và thay dấu thập phân (nếu cần)
                string processedPrice = request.Price.Replace(".", "").Replace(",", ".");

                // Chuyển đổi chuỗi sang kiểu decimal
                if (decimal.TryParse(processedPrice, out price))
                {
                    if (price <= 0)
                    {
                        errorList.Add("Giá kim cương phải lớn hơn 0");
                    }
                }
                else
                {
                    errorList.Add("Giá kim cương không hợp lệ");
                }
            }
            catch (FormatException)
            {
                errorList.Add("Giá kim cương không hợp lệ");
            }

            if (request.effectDate < DateTime.Today.AddDays(-7) || request.effectDate > DateTime.Today)
            {
                errorList.Add("Bảng giá kim cương phải được cập nhật trong khoảng thời gian gần đây.");
            }


            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            GemPriceList.CaratWeight = request.CaratWeight != null ? request.CaratWeight : "";
            GemPriceList.Cut = request.Cut != null ? request.Cut : "";
            GemPriceList.Clarity = request.Clarity != null ? request.Clarity : "";
            GemPriceList.Color = request.Color != null ? request.Color : "";
            GemPriceList.Price = price;
            GemPriceList.effectDate = (DateTime)request.effectDate;
            GemPriceList.Active = request.Active;
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<GemPriceListVm>>> ViewGemPriceList(ViewGemPriceListRequest request)
        {
            var listGemPriceList = await _context.GemPriceLists.ToListAsync();
            if (request.Keyword != null)
            {
                listGemPriceList = listGemPriceList.Where(x => x.CaratWeight.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                || x.Price.ToString().Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                || x.effectDate.ToString().Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                || x.GemPriceListId.ToString().Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();

            }
            listGemPriceList = listGemPriceList.OrderByDescending(x => x.CaratWeight).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listGemPriceList.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();
            var listGemPriceListVm = new List<GemPriceListVm>();
            foreach (var item in listPaging)
            {
                var gemPriceListVm = new GemPriceListVm()
                {
                    GemPriceListId = item.GemPriceListId,
                    CaratWeight = item.CaratWeight,
                    Clarity = item.Clarity,
                    Cut = item.Cut,
                    Color = item.Color,
                    Price = (decimal)item.Price,
                    effectDate = item.effectDate,
                    Active = item.Active,
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
