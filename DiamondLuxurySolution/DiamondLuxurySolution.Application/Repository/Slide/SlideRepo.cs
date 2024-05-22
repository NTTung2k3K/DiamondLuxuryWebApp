﻿using DiamondLuxurySolution.Data.EF;
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
            if (string.IsNullOrEmpty(request.SlideUrl))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập Url của slide");
            }
            if (request.SlideImage == null)
            {
                return new ApiErrorResult<bool>("Vui lòng chèn hình ảnh của slide");
            }

            string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.SlideImage);

            var slide = new DiamondLuxurySolution.Data.Entities.Slide
            {
                SlideName = request.SlideName,
                SlideUrl = request.SlideUrl,
                SlideImage = firebaseUrl,
                Status = request.Status,
                Description = request.Description
            };
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
            if (string.IsNullOrEmpty(request.SlideName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên Slide");
            }
            if (string.IsNullOrEmpty(request.SlideUrl))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập Url của Slide");
            }
            if (request.SlideImage == null)
            {
                return new ApiErrorResult<bool>("Vui lòng chèn hình ảnh Slide");
            }

            var slide = await _context.Slides.FindAsync(request.SlideId);
            if (slide == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy Slide");
            }
            string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.SlideImage);
            slide.SlideId = request.SlideId;
            slide.SlideName = request.SlideName;
            slide.Status = request.Status;
            slide.SlideImage = firebaseUrl;
            slide.SlideUrl = request.SlideUrl;
            slide.Description = request.Description;

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<SlideViewModel>>> ViewSlide(ViewSlideRequest request)
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
    }
}
