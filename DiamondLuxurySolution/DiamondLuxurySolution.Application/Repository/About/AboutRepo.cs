using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.About
{
    public class AboutRepo : IAboutRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public AboutRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateAbout(CreateAboutRequest request)
        {
            if (string.IsNullOrEmpty(request.AboutName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên liên hệ");
            }
            
            var about = new DiamondLuxurySolution.Data.Entities.About
            {
                AboutName = request.AboutName,
                Description = request.Description != null ? request.Description : "",
                Status = request.Status,
            };
            if (request.AboutImage != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.AboutImage);
                about.AboutImage = firebaseUrl;
            } else
            {
                about.AboutImage = "";
            }

            _context.Abouts.Add(about);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> DeleteAbout(DeleteAboutRequest request)
        {
            var about = await _context.Abouts.FindAsync(request.AboutId);
            if (about == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy liên hệ");
            }

            _context.Abouts.Remove(about);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<AboutVm>> GetAboutById(int AboutId)
        {
            var about = await _context.Abouts.FindAsync(AboutId);
            if (about == null)
            {
                return new ApiErrorResult<AboutVm>("Không tìm thấy liên hệ");
            }
            var aboutVm = new AboutVm()
            {
                AboutId = about.AboutId,
                AboutName = about.AboutName,
                AboutImage = about.AboutImage,
                Description = about.Description,
                Status = about.Status,
            };
            return new ApiSuccessResult<AboutVm>(aboutVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateAbout(UpdateAboutRequest request)
        {
            if (string.IsNullOrEmpty(request.AboutName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên liên hệ");
            }
            var about = await _context.Abouts.FindAsync(request.AboutId);
            if (about == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy liên hệ");
            }
            if (request.AboutImage != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.AboutImage);
                about.AboutImage = firebaseUrl;
            }
            else
            {
                about.AboutImage = "";
            }
            about.AboutName = request.AboutName;
            about.Description = request.Description != null ? request.Description : "";
            about.Status = request.Status;

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }


        public async Task<ApiResult<PageResult<AboutVm>>> ViewAboutInCustomer(ViewAboutRequest request)
        {
            var listAbout = await _context.Abouts.ToListAsync();
            if (request.Keyword != null)
            {
                listAbout = listAbout.Where(x => x.AboutName.Contains(request.Keyword)).ToList();

            }
            listAbout = listAbout.Where(x => x.Status).OrderByDescending(x => x.AboutName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listAbout.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listAboutVm = listPaging.Select(x => new AboutVm()
            {
                AboutId = x.AboutId,
                AboutName = x.AboutName,
                Description = x.Description,
                AboutImage = x.AboutImage,
                Status = x.Status,
            }).ToList();
            var listResult = new PageResult<AboutVm>()
            {
                Items = listAboutVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listAbout.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<AboutVm>>(listResult, "Success");
        }

        public async Task<ApiResult<PageResult<AboutVm>>> ViewAboutInManager(ViewAboutRequest request)
        {
            var listAbout = await _context.Abouts.ToListAsync();
            if (request.Keyword != null)
            {
                listAbout = listAbout.Where(x => x.AboutName.Contains(request.Keyword)).ToList();

            }
            listAbout = listAbout.OrderByDescending(x => x.AboutName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listAbout.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listAboutVm = listPaging.Select(x => new AboutVm()
            {
                AboutId = x.AboutId,
                AboutName = x.AboutName,
                Description = x.Description,
                AboutImage = x.AboutImage,
                Status = x.Status,
            }).ToList();
            var listResult = new PageResult<AboutVm>()
            {
                Items = listAboutVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listAbout.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<AboutVm>>(listResult, "Success");
        }
    }
}
