using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Discount;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Discount
{
    public class DiscountRepo : IDiscountRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public DiscountRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<bool>> CreateDiscount(CreateDiscountRequest request)
        {
            if (string.IsNullOrEmpty(request.DiscountName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên chiết khấu");
            }
            if (request.StartDate == null)
            {
                return new ApiErrorResult<bool>("Vui lòng nhập ngày bắt đầu chiết khấu");
            }
            if (request.EndDate == null)
            {
                return new ApiErrorResult<bool>("Vui lòng nhập ngày kết thúc chiết khấu");
            }
            if (request.StartDate >= request.EndDate)
            {
                return new ApiErrorResult<bool>("Ngày kết thúc phải sau ngày bắt đầu chiết khấu");
            }
            if (request.PercentSale >= 0)
            {
                return new ApiErrorResult<bool>("% Chiết khấu phải >= 0");
            }
            if (request.DiscountImage == null)
            {
                return new ApiErrorResult<bool>("Vui lòng gắn Image");
            }
            string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.DiscountImage);
            var discount = new DiamondLuxurySolution.Data.Entities.Discount
            {
                DiscountId = Guid.NewGuid(),
                DiscountName = request.DiscountName,
                Description = request.Description,
                StartDate = request.StartDate,
                DiscountImage = firebaseUrl,
                EndDate = request.EndDate,
                PercentSale = request.PercentSale,
            };
            _context.Discounts.Add(discount);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> DeleteDiscount(DeleteDiscountRequest request)
        {
            var discount = await _context.Discounts.FindAsync(request.DiscountId);
            if (discount == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy chiết khấu");
            }

            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<DiscountVm>> GetDiscountById(Guid DiscountId)
        {
            var discount = await _context.Discounts.FindAsync(DiscountId);
            if (discount == null)
            {
                return new ApiErrorResult<DiscountVm>("Không tìm thấy chiết khấu");
            }
            var discountVm = new DiscountVm()
            {
                DiscountId = discount.DiscountId,
                DiscountName = discount.DiscountName,
                DiscountImage = discount.DiscountImage,
                Description = discount.Description,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                PercentSale = discount.PercentSale,

            };
            return new ApiSuccessResult<DiscountVm>(discountVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateDiscount(UpdateDiscountRequest request)
        {
            if (string.IsNullOrEmpty(request.DiscountName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên chiết khấu");
            }
            if (request.StartDate == null)
            {
                return new ApiErrorResult<bool>("Vui lòng nhập ngày bắt đầu chiết khấu");
            }
            if (request.EndDate == null)
            {
                return new ApiErrorResult<bool>("Vui lòng nhập ngày kết thúc chiết khấu");
            }
            if (request.StartDate >= request.EndDate)
            {
                return new ApiErrorResult<bool>("Ngày kết thúc phải sau ngày bắt đầu chiết khấu");
            }
            if (request.PercentSale >= 0)
            {
                return new ApiErrorResult<bool>("% Chiết khấu phải >= 0");
            }
            if (request.DiscountImage == null)
            {
                return new ApiErrorResult<bool>("Vui lòng gắn Image");
            }
            var discount = await _context.Discounts.FindAsync(request.DiscountId);
            if (discount == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy chiết khấu");
            }

            string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.DiscountImage);
            discount.DiscountName = request.DiscountName;
            discount.DiscountImage = firebaseUrl;
            discount.Description = request.Description;
            discount.StartDate = request.StartDate;
            discount.EndDate = request.EndDate;
            discount.PercentSale = request.PercentSale;

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<DiscountVm>>> ViewDiscount(ViewDiscountRequest request)
        {
            var listDiscount = await _context.Discounts.ToListAsync();
            if (request.Keyword != null)
            {
                listDiscount = listDiscount.Where(x => x.DiscountName.Contains(request.Keyword)).ToList();

            }
            listDiscount = listDiscount.OrderByDescending(x => x.DiscountName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listDiscount.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listDiscountVm = listPaging.Select(x => new DiscountVm()
            {
                DiscountId = x.DiscountId,
                DiscountName = x.DiscountName,
                Description = x.Description,
                DiscountImage = x.DiscountImage,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                PercentSale = x.PercentSale,
            }).ToList();
            var listResult = new PageResult<DiscountVm>()
            {
                Items = listDiscountVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listDiscount.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<DiscountVm>>(listResult, "Success");
        }
    }
}
