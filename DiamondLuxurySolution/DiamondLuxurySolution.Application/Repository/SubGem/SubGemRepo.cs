using Azure.Core;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Material;
using DiamondLuxurySolution.ViewModel.Models.SubGem;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.SubGem
{
    public class SubGemRepo : ISubGemRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public SubGemRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateSubGem(CreateSubGemRequest request)
        {
            List<string> errorList = new List<string>();
            decimal price = 0;
            try
            {
                // Loại bỏ dấu phân cách hàng nghìn và thay dấu thập phân (nếu cần)
                string processedPrice = request.SubGemPrice.Replace(".", "").Replace(",", ".");

                // Chuyển đổi chuỗi sang kiểu decimal
                if (decimal.TryParse(processedPrice, out price))
                {
                    if (price <= 0)
                    {
                        errorList.Add("Giá kim cương phụ phải lớn hơn 0");
                    }
                }
                else
                {
                    errorList.Add("Giá kim cương phụ không hợp lệ");
                }
            }
            catch (FormatException)
            {
                errorList.Add("Giá kim cương phụ không hợp lệ");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            var subGem = new DiamondLuxurySolution.Data.Entities.SubGem
            {
                Description = !string.IsNullOrEmpty(request.Description) ? request.Description.Trim() : "",
                SubGemName = request.SubGemName.Trim(),
                SubGemPrice = price,
                Active = request.Active
            };

            _context.SubGems.Add(subGem);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> DeleteSubGem(DeleteSubGemRequest request)
        {
            var subGem = await _context.SubGems.FindAsync(request.SubGemId);
            if (subGem == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy kim cương phụ");
            }
            _context.SubGems.Remove(subGem);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<List<SubGemVm>>> GetAll()
        {
            var list = await _context.SubGems.ToListAsync();
            var rs = list.Select(x => new SubGemVm()
            {
                SubGemId = x.SubGemId,
                SubGemName = x.SubGemName,
                SubGemPrice = x.SubGemPrice,
                Description = x.Description,
                Active = x.Active
            }).ToList();
            return new ApiSuccessResult<List<SubGemVm>>(rs);
        }
          

        public async Task<ApiResult<SubGemVm>> GetSubGemById(Guid SubGemId)
        {
            var subGem = await _context.SubGems.FindAsync(SubGemId);
            if (subGem == null)
            {
                return new ApiErrorResult<SubGemVm>("Không tìm thấy kim cương phụ");
            }

            var subGemVm = new SubGemVm()
            {
                SubGemId = subGem.SubGemId,
                SubGemName = subGem.SubGemName,
                SubGemPrice = subGem.SubGemPrice,
                Description = subGem.Description,
                Active = subGem.Active
            };
            return new ApiSuccessResult<SubGemVm>(subGemVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateSubGem(UpdateSubGemRequest request)
        {
            var subGem = await _context.SubGems.FindAsync(request.SubGemId);
            if (subGem == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy kim cương phụ");
            }
            List<string> errorList = new List<string>();
            decimal price = 0;
            try
            {
                // Loại bỏ dấu phân cách hàng nghìn và thay dấu thập phân (nếu cần)
                string processedPrice = request.SubGemPrice.Replace(".", "").Replace(",", ".");

                // Chuyển đổi chuỗi sang kiểu decimal
                if (decimal.TryParse(processedPrice, out price))
                {
                    if (price <= 0)
                    {
                        errorList.Add("Giá kim cương phụ phải lớn hơn 0");
                    }
                }
                else
                {
                    errorList.Add("Giá kim cương phụ không hợp lệ");
                }
            }
            catch (FormatException)
            {
                errorList.Add("Giá kim cương phụ không hợp lệ");
            }
            subGem.SubGemName = request.SubGemName.Trim();
            subGem.SubGemPrice = price;
            subGem.Description = !string.IsNullOrWhiteSpace(request.Description) ? request.Description.Trim() : "";
            subGem.Active = request.Active;
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<SubGemVm>>> ViewSubGemInCustomer(ViewSubGemRequest request)
        {
            var listSubGem = await _context.SubGems.ToListAsync();
            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                listSubGem = listSubGem.Where(x => x.SubGemName.Contains(request.KeyWord, StringComparison.OrdinalIgnoreCase)).ToList();

            }
            listSubGem = listSubGem.Where(x => x.Active == true).OrderByDescending(x => x.SubGemName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listSubGem.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listSubGemVm = listPaging.Select(x => new SubGemVm()
            {
                SubGemName = x.SubGemName,
                Description = x.Description,
                SubGemId = x.SubGemId,
                SubGemPrice = x.SubGemPrice,
                Active = x.Active
            }).ToList();
            var listResult = new PageResult<SubGemVm>()
            {
                Items = listSubGemVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listSubGem.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<SubGemVm>>(listResult, "Success");
        }


        public async Task<ApiResult<PageResult<SubGemVm>>> ViewSubGemInManager(ViewSubGemRequest request)
        {
            var listSubGem = await _context.SubGems.ToListAsync();
            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                listSubGem = listSubGem.Where(x => x.SubGemName.Contains(request.KeyWord, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            listSubGem = listSubGem.OrderByDescending(x => x.SubGemName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listSubGem.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listSubGemVm = listPaging.Select(x => new SubGemVm()
            {
                SubGemName = x.SubGemName,
                Description = x.Description,
                SubGemId = x.SubGemId,
                SubGemPrice = x.SubGemPrice,
                Active = x.Active
            }).ToList();
            var listResult = new PageResult<SubGemVm>()
            {
                Items = listSubGemVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listSubGem.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<SubGemVm>>(listResult, "Success");
        }

    }
}

