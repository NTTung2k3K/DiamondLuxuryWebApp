using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Slide;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Slide
{
    public class SlideRepo : ISlideRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public SlideRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateSlide(CreateSlideRequest request)
        {
            if (string.IsNullOrEmpty(request.SlideName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên slide");
            }
            

            var slide = new DiamondLuxurySolution.Data.Entities.Slide
            {
                SlideName = request.SlideName,
                SlideUrl = request.SlideUrl != null ? request.SlideUrl : "",
                Status = request.Status,
                Description = request.Description != null ? request.Description : "",
            };
            if (request.SlideImage != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.SlideImage);
                slide.SlideImage = firebaseUrl;
            }
            else
            {
                slide.SlideImage = "";
            }


            _context.Slides.Add(slide);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }


        public async Task<ApiResult<bool>> DeleteSlide(DeleteSlideRequest request)
        {
            var slide = await _context.Slides.FindAsync(request.SlideId);
            if (slide == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy slide");
            }

            _context.Slides.Remove(slide);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<SlideViewModel>> GetSlideById(int SlideId)
        {
            var slide = await _context.Slides.FindAsync(SlideId);
            if (slide == null)
            {
                return new ApiErrorResult<SlideViewModel>("Không tìm thấy Slide");
            }
            var slideVm = new SlideViewModel()
            {
                SlideImage = slide.SlideImage,
                SlideName = slide.SlideName,
                SlideId = slide.SlideId,
                SlideUrl = slide.SlideUrl,
                Status = slide.Status,
                Description = slide.Description
            };
            return new ApiSuccessResult<SlideViewModel>(slideVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateSlide(UpdateSlideRequest request)
        {
            var slide = await _context.Slides.FindAsync(request.SlideId);
            if (slide == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy slide");
            }

            if (string.IsNullOrEmpty(request.SlideName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên slide");
            }
            
            slide.SlideName = request.SlideName;
            slide.SlideUrl = !string.IsNullOrEmpty(request.SlideUrl)?request.SlideUrl:"";
            slide.Description = !string.IsNullOrEmpty(request.Description)?request.Description:"";
            slide.Status = request.Status;
            if (request.SlideImage != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.SlideImage);
                slide.SlideImage = firebaseUrl;
            }
            else
            {
                slide.SlideImage = "";
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<SlideViewModel>>> ViewSlidesInManager(ViewSlideRequest request)
        {
            var listSlide = await _context.Slides.ToListAsync();
            if (request.Keyword != null)
            {
                listSlide = listSlide.Where(x => x.SlideName.Contains(request.Keyword)).ToList();

            }
            listSlide = listSlide.OrderByDescending(x => x.SlideName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listSlide.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listSlideVm = listPaging.Select(x => new SlideViewModel()
            {
                SlideId = x.SlideId,
                SlideName = x.SlideName,
                SlideUrl = x.SlideUrl,
                SlideImage = x.SlideImage,
                Status = x.Status,
                Description = x.Description
            }).ToList();
            var listResult = new PageResult<SlideViewModel>()
            {
                Items = listSlideVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listSlideVm.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<SlideViewModel>>(listResult, "Success");
        }

        public async Task<ApiResult<PageResult<SlideViewModel>>> ViewSlidesInCustomer(ViewSlideRequest request)
        {
            var listSlide = await _context.Slides.ToListAsync();
            if (request.Keyword != null)
            {
                listSlide = listSlide.Where(x => x.SlideName.Contains(request.Keyword)).ToList();

            }
            listSlide = listSlide.Where(x => x.Status == true).OrderByDescending(x => x.SlideName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listSlide.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listSlideVm = listPaging.Select(x => new SlideViewModel()
            {
                SlideId = x.SlideId,
                SlideName = x.SlideName,
                SlideUrl = x.SlideUrl,
                SlideImage = x.SlideImage,
                Status = x.Status,
                Description = x.Description
            }).ToList();
            var listResult = new PageResult<SlideViewModel>()
            {
                Items = listSlideVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listSlideVm.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<SlideViewModel>>(listResult, "Success");
        }

        public async Task<ApiResult<List<SlideViewModel>>> GetAll()
        {
            var list = await _context.Slides.ToListAsync();
            var rs = list.Select(x => new SlideViewModel()
            {
                SlideId =x.SlideId,
                SlideName=x.SlideName,
                SlideUrl = x.SlideUrl,
                Description = x.Description,
                SlideImage =x.SlideImage,
                Status = x.Status,
            }).ToList();
            return new ApiSuccessResult<List<SlideViewModel>>(rs);
        }
    }
}

