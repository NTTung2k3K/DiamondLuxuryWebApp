using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Promotion;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Promotion
{
    public class PromotionRepo : IPromotionRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public PromotionRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreatePromotion(CreatePromotionRequest request)
        {
            if (string.IsNullOrEmpty(request.PromotionName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên khuyến mãi");
            }
            if (request.StartDate == null)
            {
                return new ApiErrorResult<bool>("Vui lòng nhập ngày bắt đầu khuyến mãi");
            }
            if (request.EndDate == null)
            {
                return new ApiErrorResult<bool>("Vui lòng nhập ngày kết thúc khuyến mãi");
            }
            if (request.StartDate >= request.EndDate)
            {
                return new ApiErrorResult<bool>("Ngày kết thúc phải sau ngày bắt đầu khuyến mãi");
            }
            if (request.PromotionImage == null)
            {
                return new ApiErrorResult<bool>("Vui lòng gắn Image");
            }
            string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.PromotionImage);
            var promotion = new DiamondLuxurySolution.Data.Entities.Promotion
            {
                PromotionId = Guid.NewGuid(),
                PromotionName = request.PromotionName,
                Description = request.Description,
                StartDate = request.StartDate,
                PromotionImage = firebaseUrl,
                EndDate = request.EndDate,
            };
            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> DeletePromotion(DeletePromotionRequest request)
        {
            var promotion = await _context.Promotions.FindAsync(request.PromotionId);
            if (promotion == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy khuyến mãi");
            }

            _context.Promotions.Remove(promotion);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }


        public async Task<ApiResult<PromotionVm>> GetPromotionById(Guid PromotionId)
        {
            var promotion = await _context.Promotions.FindAsync(PromotionId);
            if (promotion == null)
            {
                return new ApiErrorResult<PromotionVm>("Không tìm thấy khuyến mãi");
            }
            var promotionVm = new PromotionVm()
            {
                PromotionId = promotion.PromotionId,
                PromotionName = promotion.PromotionName,
                PromotionImage = promotion.PromotionImage,
                Description = promotion.Description,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
            };
            return new ApiSuccessResult<PromotionVm>(promotionVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdatePromotion(UpdatePromotionRequest request)
        {
            if (string.IsNullOrEmpty(request.PromotionName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên khuyến mãi");
            }
            if (request.StartDate == null)
            {
                return new ApiErrorResult<bool>("Vui lòng nhập ngày bắt đầu khuyến mãi");
            }
            if (request.EndDate == null)
            {
                return new ApiErrorResult<bool>("Vui lòng nhập ngày kết thúc khuyến mãi");
            }
            if (request.StartDate >= request.EndDate)
            {
                return new ApiErrorResult<bool>("Ngày kết thúc phải sau ngày bắt đầu khuyến mãi");
            }
            if (request.PromotionImage == null)
            {
                return new ApiErrorResult<bool>("Vui lòng gắn Image");
            }
            var promotion = await _context.Promotions.FindAsync(request.PromotionId);
            if (promotion == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy khuyến mãi");
            }
            string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.PromotionImage);
            promotion.PromotionName = request.PromotionName;
            promotion.PromotionImage = firebaseUrl;
            promotion.Description = request.Description;
            promotion.StartDate = request.StartDate;
            promotion.EndDate = request.EndDate;

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<PromotionVm>>> ViewPromotion(ViewPromotionRequest request)
        {
            var listPromotion = await _context.Promotions.ToListAsync();
            if (request.Keyword != null)
            {
                listPromotion = listPromotion.Where(x => x.PromotionName.Contains(request.Keyword)).ToList();

            }
            listPromotion = listPromotion.OrderByDescending(x => x.PromotionName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listPromotion.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listPromotionVm = listPaging.Select(x => new PromotionVm()
            {
                PromotionId = x.PromotionId,
                PromotionName = x.PromotionName,
                Description = x.Description,
                PromotionImage = x.PromotionImage,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
            }).ToList();
            var listResult = new PageResult<PromotionVm>()
            {
                Items = listPromotionVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listPromotion.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<PromotionVm>>(listResult, "Success");
        }
    }
}
