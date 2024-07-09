using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Slide;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DiamondLuxurySolution.Utilities.Constants.Systemconstant;

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
                Description = request.Description != null ? request.Description : null,
                Status = request.Status,
                AboutAddress = request.AboutAddress != null ? request.AboutAddress : null,
                AboutEmail = request.AboutEmail != null ? request.AboutEmail : null,
                AboutPhoneNumber = request.AboutPhoneNumber != null ? request.AboutPhoneNumber : null,
            };


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
                Description = about.Description != null ? about.Description : null,
                Status = about.Status,
                AboutAddress = about.AboutAddress != null ? about.AboutAddress : null,
                AboutEmail = about.AboutEmail != null ? about.AboutEmail : null,
                AboutPhoneNumber = about.AboutPhoneNumber != null ? about.AboutPhoneNumber : null,
                AboutName = about.AboutName != null ? about.AboutName : null,
            };
            return new ApiSuccessResult<AboutVm>(aboutVm, "Success");
        }

        public async Task<ApiResult<List<AboutVm>>> GetAll()
        {
            var list = await _context.Abouts.ToListAsync();
            var rs = list.Select(about => new AboutVm()
            {
                AboutId = about.AboutId,
                Description = about.Description != null ? about.Description : null,
                Status = about.Status,
                AboutAddress = about.AboutAddress != null ? about.AboutAddress : null,
                AboutEmail = about.AboutEmail != null ? about.AboutEmail : null,
                AboutPhoneNumber = about.AboutPhoneNumber != null ? about.AboutPhoneNumber : null,
                AboutName = about.AboutName != null ? about.AboutName : null,
            }).ToList();
            return new ApiSuccessResult<List<AboutVm>>(rs);
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
            
            about.Description = request.Description != null ? request.Description : null;
            about.Status = request.Status;
            about.AboutAddress = request.AboutAddress != null ? request.AboutAddress : null;
            about.AboutEmail = request.AboutEmail != null ? request.AboutEmail : null;
            about.AboutName= request.AboutName;
            about.AboutPhoneNumber = request.AboutPhoneNumber != null ? request.AboutPhoneNumber : null;

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }


        public async Task<ApiResult<PageResult<AboutVm>>> ViewAboutInCustomer(ViewAboutRequest request)
        {
            var listAbout = await _context.Abouts.ToListAsync();
            if (request.Keyword != null)
            {
                listAbout = listAbout.Where(x => x.AboutName.Contains(request.Keyword,StringComparison.OrdinalIgnoreCase) ||
                                                     (x.Description ?? string.Empty).Contains(request.Keyword, StringComparison.OrdinalIgnoreCase) ||
                                                     (x.AboutAddress ?? string.Empty).Contains(request.Keyword, StringComparison.OrdinalIgnoreCase) ||
                                                     (x.AboutPhoneNumber ?? string.Empty).Contains(request.Keyword, StringComparison.OrdinalIgnoreCase) ||
                                                     (x.AboutEmail ?? string.Empty).Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();

            }
            listAbout = listAbout.Where(x => x.Status).OrderByDescending(x => x.AboutName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listAbout.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listAboutVm = listPaging.Select(about => new AboutVm()
            {
                AboutId = about.AboutId,
                Description = about.Description != null ? about.Description : null,
                Status = about.Status,
                AboutAddress = about.AboutAddress != null ? about.AboutAddress : null,
                AboutEmail = about.AboutEmail != null ? about.AboutEmail : null,
                AboutPhoneNumber = about.AboutPhoneNumber != null ? about.AboutPhoneNumber : null,
                AboutName = about.AboutName != null ? about.AboutName : null,
                
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
                listAbout = listAbout.Where(x => x.AboutName.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase) ||
                                                     (x.Description ?? string.Empty).Contains(request.Keyword, StringComparison.OrdinalIgnoreCase) ||
                                                     (x.AboutAddress ?? string.Empty).Contains(request.Keyword, StringComparison.OrdinalIgnoreCase) ||
                                                     (x.AboutPhoneNumber ?? string.Empty).Contains(request.Keyword, StringComparison.OrdinalIgnoreCase) ||
                                                     (x.AboutEmail ?? string.Empty).Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            listAbout = listAbout.OrderByDescending(x => x.AboutName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listAbout.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listAboutVm = listPaging.Select(about => new AboutVm()
            {
                AboutId = about.AboutId,
                Description = about.Description != null ? about.Description : null,
                Status = about.Status,
                AboutAddress = about.AboutAddress != null ? about.AboutAddress : null,
                AboutEmail = about.AboutEmail != null ? about.AboutEmail : null,
                AboutPhoneNumber = about.AboutPhoneNumber != null ? about.AboutPhoneNumber : null,
                AboutName = about.AboutName != null ? about.AboutName : null,
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
