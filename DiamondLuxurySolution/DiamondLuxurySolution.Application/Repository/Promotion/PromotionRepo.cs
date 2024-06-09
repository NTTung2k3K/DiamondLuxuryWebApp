using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Promotion;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.PromotionName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên khuyến mãi");
            }

            if (string.IsNullOrEmpty(request.DiscountPercent))
            {
                errorList.Add("Vui lòng nhập % giảm giá");
            }

            decimal percentDiscount = 0;
            try
            {
                percentDiscount = Convert.ToDecimal(request.DiscountPercent);

                if (percentDiscount < 0 || percentDiscount > 100)
                {
                    errorList.Add("% Giảm giá phải >= 0 và <= 100");
                }
            }
            catch (FormatException)
            {
                errorList.Add("% giảm giá không hợp lệ");
            }

            if (string.IsNullOrEmpty(request.MaxDiscount))
            {
                errorList.Add("Vui lòng nhập giá max giảm");
            }

            decimal maxDiscount = 0;
            try
            {
                maxDiscount = Convert.ToDecimal(request.MaxDiscount);

                if (maxDiscount < 0)
                {
                    errorList.Add("Max giảm giá phải >= 0");
                }
            }
            catch (FormatException)
            {
                errorList.Add("Max giảm giá không hợp lệ");
            }

            if (request.StartDate >= request.EndDate)
            {
                errorList.Add("Ngày kết thúc phải sau ngày bắt đầu khuyến mãi");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            
            var promotion = new DiamondLuxurySolution.Data.Entities.Promotion
            {
                PromotionId = Guid.NewGuid(),
                PromotionName = request.PromotionName,
                Description = request.Description != null ? request.Description : "",
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                DiscountPercent = percentDiscount,
                MaxDiscount = maxDiscount,
                Status = request.Status,
            };
            if (request.PromotionImage != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.PromotionImage);
                promotion.PromotionImage = firebaseUrl;
            } else
            {
                promotion.PromotionImage = "";
            }

            if (request.BannerImage != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.BannerImage);
                promotion.BannerImage = firebaseUrl;
            }
            else
            {
                promotion.BannerImage = "";
            }

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


        public async Task<ApiResult<List<PromotionVm>>> GetAll()
        {
            var list = await _context.Promotions.ToListAsync();
            var listPromotionVm = new List<PromotionVm>();

            foreach (var item in listPromotionVm)
            {
                var promotionVm = new PromotionVm()
                {
                    PromotionId = item.PromotionId,
                    PromotionName = item.PromotionName,
                    Description = item.Description,
                    PromotionImage = item.PromotionImage,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate,
                    BannerImage = item.BannerImage,
                    DiscountPercent = item.DiscountPercent,
                    MaxDiscount = item.MaxDiscount,
                    Status = item.Status,
                };
                listPromotionVm.Add(promotionVm);
            }
            return new ApiSuccessResult<List<PromotionVm>>(listPromotionVm.ToList());
        }

        public async Task<ApiResult<List<PromotionVm>>> GetAllOnTime()
        {
            var list = await _context.Promotions.ToListAsync();
            var listPromotionVm = new List<PromotionVm>();

            foreach (var item in list)
            {
                if(item.StartDate.Date <= DateTime.Now && DateTime.Now <= item.EndDate)
                {
                    var promotionVm = new PromotionVm()
                    {
                        PromotionId = item.PromotionId,
                        PromotionName = item.PromotionName,
                        Description = item.Description,
                        PromotionImage = item.PromotionImage,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        BannerImage = item.BannerImage,
                        DiscountPercent = item.DiscountPercent,
                        MaxDiscount = item.MaxDiscount,
                        Status = item.Status,
                    };
                    listPromotionVm.Add(promotionVm);
                }
            }
            return new ApiSuccessResult<List<PromotionVm>>(listPromotionVm.ToList());
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
                BannerImage = promotion.BannerImage,
                DiscountPercent = promotion.DiscountPercent,    
                MaxDiscount = promotion.MaxDiscount,
                Status = promotion.Status,
            };
            return new ApiSuccessResult<PromotionVm>(promotionVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdatePromotion(UpdatePromotionRequest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.PromotionName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên khuyến mãi");
            }
            if (string.IsNullOrEmpty(request.DiscountPercent))
            {
                errorList.Add("Vui lòng nhập % giảm giá");
            }

            decimal percentDiscount = 0;
            try
            {
                percentDiscount = Convert.ToDecimal(request.DiscountPercent);

                if (percentDiscount < 0 || percentDiscount > 100)
                {
                    errorList.Add("% Giảm giá phải >= 0 và <= 100");
                }
            }
            catch (FormatException)
            {
                return new ApiErrorResult<bool>("% giảm giá không hợp lệ hoặc chưa được nhập");
            }

            if (string.IsNullOrEmpty(request.MaxDiscount))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập giá max giảm");
            }

            decimal maxDiscount = 0;
            try
            {
                maxDiscount = Convert.ToDecimal(request.MaxDiscount);

                if (maxDiscount < 0)
                {
                    return new ApiErrorResult<bool>("Max giảm giá phải >= 0");
                }
            }
            catch (FormatException)
            {
                return new ApiErrorResult<bool>("Giảm giá tối đa không hợp lệ hoặc chưa được nhập");
            }

            if (request.StartDate >= request.EndDate)
            {
                return new ApiErrorResult<bool>("Ngày kết thúc phải sau ngày bắt đầu khuyến mãi");
            }

            var promotion = await _context.Promotions.FindAsync(request.PromotionId);
            if (promotion == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy khuyến mãi");
            }

            promotion.PromotionName = request.PromotionName;
            promotion.Description = request.Description ?? "";
            promotion.StartDate = request.StartDate;
            promotion.EndDate = request.EndDate;
            promotion.DiscountPercent = percentDiscount;
            promotion.MaxDiscount = maxDiscount;
            promotion.Status = request.Status;

            if (request.PromotionImage != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.PromotionImage);
                promotion.PromotionImage = firebaseUrl;
            }

            if (request.BannerImage != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.BannerImage);
                promotion.BannerImage = firebaseUrl;
            }
            _context.Promotions.Update(promotion);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }


        public async Task<ApiResult<PageResult<PromotionVm>>> ViewPromotionInCustomer(ViewPromotionRequest request)
        {
            var listPromotion = await _context.Promotions.ToListAsync();
            if (request.Keyword != null)
            {
                listPromotion = listPromotion.Where(x => x.PromotionName.Contains(request.Keyword)).ToList();

            }
            listPromotion = listPromotion.Where(x => x.Status).OrderByDescending(x => x.PromotionName).ToList();

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
                BannerImage = x.BannerImage,
                Status = x.Status,
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

        public async Task<ApiResult<PageResult<PromotionVm>>> ViewPromotionInManager(ViewPromotionRequest request)
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
                BannerImage = x.BannerImage,
                Status = x.Status,
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
