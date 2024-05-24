using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Material;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Material
{
    public class MaterialRepo : IMaterialRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public MaterialRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateMaterial(CreateMaterialRequest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.MaterialName))
            {
                errorList.Add("Vui lòng nhập tên nguyên liệu");
            }
            if (string.IsNullOrEmpty(request.Color))
            {
                errorList.Add("Vui lòng nhập màu nguyên liệu");
            }
            if (request.Weight <= 0)
            {
                errorList.Add("trọng lượng nguyên liệu phải > 0");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            var material = new DiamondLuxurySolution.Data.Entities.Material
            {
                MaterialId = Guid.NewGuid(),
                MaterialName = request.MaterialName,
                Color = request.Color,
                Weight = request.Weight,
                Description = request.Description,
                Status = request.Status,
            };
            _context.Materials.Add(material);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> DeleteMaterial(DeleteMaterialRequest request)
        {
            var material = await _context.Materials.FindAsync(request.MaterialId);
            if (material == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy nguyên liệu");
            }

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<MaterialVm>> GetMaterialById(Guid MaterialId)
        {
            var material = await _context.Materials.FindAsync(MaterialId);
            if (material == null)
            {
                return new ApiErrorResult<MaterialVm>("Không tìm thấy nguyên liệu");
            }
            var materialVm = new MaterialVm()
            {
                MaterialId = MaterialId,
                MaterialName = material.MaterialName,
                Color = material.Color,
                Weight = material.Weight,
                Description = material.Description,
                Status = material.Status,
            };
            return new ApiSuccessResult<MaterialVm>(materialVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateMaterial(UpdateMaterialRequest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.MaterialName))
            {
                errorList.Add("Vui lòng nhập tên nguyên liệu");
            }
            if (string.IsNullOrEmpty(request.Color))
            {
                errorList.Add("Vui lòng nhập màu nguyên liệu");
            }
            if (request.Weight <= 0)
            {
                errorList.Add("trọng lượng nguyên liệu phải > 0");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            var material = await _context.Materials.FindAsync(request.MaterialId);
            if (material == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy nguyên liệu");
            }
            material.MaterialName = request.MaterialName;
            material.Description = request.Description;
            material.Color = request.Color;
            material.Weight = request.Weight;
            material.Status = request.Status;
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<MaterialVm>>> ViewMaterialInCustomer(ViewMaterialRequest request)
        {
            var listMaterial = await _context.Materials.ToListAsync();
            if (request.Keyword != null)
            {
                listMaterial = listMaterial.Where(x => x.MaterialName.Contains(request.Keyword)).ToList();

            }
            listMaterial = listMaterial.Where(x => x.Status).OrderByDescending(x => x.MaterialName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listMaterial.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listMaterialVm = listPaging.Select(x => new MaterialVm()
            {
                MaterialId = x.MaterialId,
                MaterialName = x.MaterialName,
                Description = x.Description,
                Color = x.Color,
                Weight = x.Weight,
                Status = x.Status,
            }).ToList();
            var listResult = new PageResult<MaterialVm>()
            {
                Items = listMaterialVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listMaterial.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<MaterialVm>>(listResult, "Success");
        }

        public async Task<ApiResult<PageResult<MaterialVm>>> ViewMaterialInManager(ViewMaterialRequest request)
        {
            var listMaterial = await _context.Materials.ToListAsync();
            if (request.Keyword != null)
            {
                listMaterial = listMaterial.Where(x => x.MaterialName.Contains(request.Keyword)).ToList();

            }
            listMaterial = listMaterial.OrderByDescending(x => x.MaterialName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listMaterial.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listMaterialVm = listPaging.Select(x => new MaterialVm()
            {
                MaterialId = x.MaterialId,
                MaterialName = x.MaterialName,
                Description = x.Description,
                Color = x.Color,
                Weight = x.Weight,
                Status = x.Status,
            }).ToList();
            var listResult = new PageResult<MaterialVm>()
            {
                Items = listMaterialVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listMaterial.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<MaterialVm>>(listResult, "Success");
        }
    }
}
