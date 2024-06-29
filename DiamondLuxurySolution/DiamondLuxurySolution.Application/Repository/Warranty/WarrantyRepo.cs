using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Warranty;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Warranty
{
    public class WarrantyRepo : IWarrantyRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public WarrantyRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateWarranty(CreateWarrantyRequest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.WarrantyName))
            {
                errorList.Add("Vui lòng nhập tên phiếu bảo hành");
            }
            if (request.DateActive > request.DateExpired)
            {
                errorList.Add("Ngày tạo phiếu bảo hành với nhỏ hơn ngày hết hạn");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            Random rd = new Random();
            string WarrantyId = "W" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);

            var warranty = new DiamondLuxurySolution.Data.Entities.Warranty
            {
                WarrantyId = WarrantyId,
                WarrantyName = request.WarrantyName,
                DateActive = (DateTime)request.DateActive,
                DateExpired = (DateTime)request.DateExpired,
                Description = request.Description != null ? request.Description : "",
                Status = request.Status,
            };

            _context.Warrantys.Add(warranty);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> DeleteWarranty(DeleteWarrantyRequest request)
        {
            var warranty = await _context.Warrantys.FindAsync(request.WarrantyId);
            if (warranty == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy phiếu bảo hành");
            }

            _context.Warrantys.Remove(warranty);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<List<WarrantyVm>>> GetAll()
        {
            var list = await _context.Warrantys.ToListAsync();
            var rs = list.Select(x => new WarrantyVm()
            {
                WarrantyId = x.WarrantyId,
                DateActive = x.DateActive,
                DateExpired = x.DateExpired,
                Description = x.Description,
                Status = x.Status,
                WarrantyName = x.WarrantyName,
            }).ToList();
            return new ApiSuccessResult<List<WarrantyVm>>(rs);
        }

        public async Task<ApiResult<WarrantyVm>> GetWarrantyById(string WarrantyId)
        {
            var warranty = await _context.Warrantys.FindAsync(WarrantyId);
            if (warranty == null)
            {
                return new ApiErrorResult<WarrantyVm>("Không tìm thấy phiếu bảo hành");
            }
            var warrantyVm = new WarrantyVm()
            {
                WarrantyId = WarrantyId,
                WarrantyName = warranty.WarrantyName,
                DateActive = warranty.DateActive,
                DateExpired = warranty.DateExpired,
                Description = warranty.Description,
                Status = warranty.Status,
            };
            return new ApiSuccessResult<WarrantyVm>(warrantyVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateWarranty(UpdateWarrantyRequest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.WarrantyName))
            {
                errorList.Add("Vui lòng nhập tên phiếu bảo hành");
            }
            if (request.DateActive > request.DateExpired)
            {
                errorList.Add("Ngày tạo phiếu bảo hành với nhỏ hơn ngày hết hạn");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }

            var warranty = await _context.Warrantys.FindAsync(request.WarrantyId);
            if (warranty == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy phiếu bảo hành");
            }

            warranty.WarrantyName = request.WarrantyName;
            warranty.Description = request.Description != null ? request.Description : "";
            warranty.DateActive = (DateTime)request.DateActive;
            warranty.DateExpired = (DateTime)request.DateExpired;
            warranty.Status = request.Status;

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<WarrantyVm>>> ViewWarrantyInCustomer(ViewWarrantyRequest request)
        {
            var listWarranty = await _context.Warrantys.ToListAsync();
            if (request.Keyword != null)
            {
                listWarranty = listWarranty.Where(x => x.WarrantyName.Contains(request.Keyword)).ToList();

            }
            listWarranty = listWarranty.Where(x => x.Status).OrderByDescending(x => x.DateExpired).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listWarranty.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listWarrantyVm = listPaging.Select(x => new WarrantyVm()
            {
                WarrantyId = x.WarrantyId,
                WarrantyName = x.WarrantyName,
                DateActive = x.DateActive,
                DateExpired = x.DateExpired,
                Description = x.Description,
                Status = x.Status,
            }).ToList();
            var listResult = new PageResult<WarrantyVm>()
            {
                Items = listWarrantyVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listWarranty.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<WarrantyVm>>(listResult, "Success");
        }

        public async Task<ApiResult<PageResult<WarrantyVm>>> ViewWarrantyInManager(ViewWarrantyRequest request)
        {
            var listWarranty = await _context.Warrantys.ToListAsync();
            if (request.Keyword != null)
            {
                listWarranty = listWarranty.Where(x => x.WarrantyName.Contains(request.Keyword)).ToList();

            }
            listWarranty = listWarranty.OrderByDescending(x => x.DateExpired).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listWarranty.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listWarrantyVm = listPaging.Select(x => new WarrantyVm()
            {
                WarrantyId = x.WarrantyId,
                WarrantyName = x.WarrantyName,
                DateActive = x.DateActive,
                DateExpired = x.DateExpired,
                Description = x.Description,
                Status = x.Status,
            }).ToList();
            var listResult = new PageResult<WarrantyVm>()
            {
                Items = listWarrantyVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listWarranty.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<WarrantyVm>>(listResult, "Success");
        }
    }
}
