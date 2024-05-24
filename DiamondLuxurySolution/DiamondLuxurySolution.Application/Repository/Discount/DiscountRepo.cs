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
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.DiscountName))
            {
                errorList.Add("Vui lòng nhập tên chiết khấu");
                //return new ApiErrorResult<bool>("Vui lòng nhập tên chiết khấu");
            }
            if (request.PercentSale < 0)
            {
                errorList.Add("% Chiết khấu phải >= 0");
                //return new ApiErrorResult<bool>("% Chiết khấu phải >= 0");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            var discount = new DiamondLuxurySolution.Data.Entities.Discount
            {
                DiscountId = Guid.NewGuid(),
                DiscountName = request.DiscountName,
                Description = request.Description,
                PercentSale = request.PercentSale,
                Status = request.Status,
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
                Description = discount.Description,
                PercentSale = discount.PercentSale,
                Status = discount.Status,

            };
            return new ApiSuccessResult<DiscountVm>(discountVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateDiscount(UpdateDiscountRequest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.DiscountName))
            {
                errorList.Add("Vui lòng nhập tên chiết khấu");
                //return new ApiErrorResult<bool>("Vui lòng nhập tên chiết khấu");
            }
            if (request.PercentSale < 0)
            {
                errorList.Add("% Chiết khấu phải >= 0");
                //return new ApiErrorResult<bool>("% Chiết khấu phải >= 0");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            var discount = await _context.Discounts.FindAsync(request.DiscountId);
            if (discount == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy chiết khấu");
            }

            discount.DiscountName = request.DiscountName;
            discount.Description = request.Description;
            discount.PercentSale = request.PercentSale;
            discount.Status = request.Status;

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }


        public async Task<ApiResult<PageResult<DiscountVm>>> ViewDiscountInCustomer(ViewDiscountRequest request)
        {
            var listDiscount = await _context.Discounts.ToListAsync();
            if (request.Keyword != null)
            {
                listDiscount = listDiscount.Where(x => x.DiscountName.Contains(request.Keyword)).ToList();

            }
            listDiscount = listDiscount.Where(x => x.Status).OrderByDescending(x => x.DiscountName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listDiscount.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listDiscountVm = listPaging.Select(x => new DiscountVm()
            {
                DiscountId = x.DiscountId,
                DiscountName = x.DiscountName,
                Description = x.Description,
                PercentSale = x.PercentSale,
                Status = x.Status,
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

        public async Task<ApiResult<PageResult<DiscountVm>>> ViewDiscountInManager(ViewDiscountRequest request)
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
                PercentSale = x.PercentSale,
                Status = x.Status,
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
