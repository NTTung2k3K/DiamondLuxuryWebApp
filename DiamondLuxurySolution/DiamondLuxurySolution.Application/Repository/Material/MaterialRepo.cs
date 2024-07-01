using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
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

            decimal price = 0;
            try
            {
                // Loại bỏ dấu phân cách hàng nghìn và thay dấu thập phân (nếu cần)
                string processedPrice = request.Price.Replace(".", "").Replace(",", ".");

                // Chuyển đổi chuỗi sang kiểu decimal
                if (decimal.TryParse(processedPrice, out price))
                {
                    if (price <= 0)
                    {
                        errorList.Add("Giá vật liệu phải lớn hơn 0");
                    }
                }
                else
                {
                    errorList.Add("Giá vật liệu không hợp lệ");
                }
            }
            catch (FormatException)
            {
                errorList.Add("Giá vật liệu không hợp lệ");
            }

            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("", errorList);
            }
            var material = new Data.Entities.Material
            {
                MaterialId = Guid.NewGuid(),
                MaterialName = request.MaterialName,
                Color = request.Color != null ? request.Color : "",
                Description = request.Description != null ? request.Description : "",
                Status = request.Status,
                EffectDate = DateTime.Parse(request.EffectDate),
                Price = price,
            };
            if (request.MaterialImage != null)
            {
                string firebaseUrl = await Utilities.Helper.ImageHelper.Upload(request.MaterialImage);
                material.MaterialImage = firebaseUrl;
            }
            else
            {
                material.MaterialImage = "";
            }
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
                Price = material.Price,
                EffectDate = material.EffectDate,
                Description = material.Description,
                MaterialImage = material.MaterialImage,
                Status = material.Status,
            };
            return new ApiSuccessResult<MaterialVm>(materialVm, "Success");
        }

        public async Task<ApiResult<List<MaterialVm>>> GetAll()
        {
            var list = await _context.Materials.ToListAsync();
            var rs = list.Select(x => new MaterialVm()
            {
                MaterialId = x.MaterialId,
                MaterialName = x.MaterialName,
                Color = x.Color,
                Description = x.Description,
                MaterialImage = x.MaterialImage,
                Status = x.Status,
                EffectDate = x.EffectDate,
                Price = x.Price,
                
            }).ToList();
            return new ApiSuccessResult<List<MaterialVm>>(rs);
        }

        public async Task<ApiResult<bool>> UpdateMaterial(UpdateMaterialRequest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.MaterialName))
            {
                errorList.Add("Vui lòng nhập tên nguyên liệu");
            }


            decimal price = 0;
            try
            {
				string processedPrice = request.Price.Replace(".", "").Replace(",", ".");
				// Chuyển đổi chuỗi sang kiểu decimal
				if (decimal.TryParse(processedPrice, out price))
				{
					if (price <= 0)
					{
						errorList.Add("Giá vật liệu phải lớn hơn 0");
					}
				}
				else
				{
					errorList.Add("Giá vật liệu không hợp lệ");
				}
			}
            catch (FormatException)
            {
                errorList.Add("Giá vật liệu không hợp lệ");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("", errorList);
            }
            var material = await _context.Materials.FindAsync(request.MaterialId);
            if (material == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy nguyên liệu");
            }
            material.MaterialName = request.MaterialName;
            material.Description = request.Description != null ? request.Description : "";
            material.Color = request.Color != null ? request.Color : "";
            material.Price = price;
            material.EffectDate = DateTime.Parse(request.EffectDate);
            material.Price = price;
            material.Status = request.Status;
            if (request.MaterialImage != null)
            {
                string firebaseUrl = await Utilities.Helper.ImageHelper.Upload(request.MaterialImage);
                material.MaterialImage = firebaseUrl;
            }

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<MaterialVm>>> ViewMaterialInCustomer(ViewMaterialRequest request)
        {
            var listMaterial = await _context.Materials.ToListAsync();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listMaterial.ToPagedList(pageIndex, Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listMaterialVm = listPaging.Select(x => new MaterialVm()
            {
                MaterialId = x.MaterialId,
                MaterialName = x.MaterialName,
                Description = x.Description,
                Color = x.Color,
                MaterialImage = x.MaterialImage,
                Status = x.Status,
                Price = x.Price,
                EffectDate = x.EffectDate
            }).ToList();
            var listResult = new PageResult<MaterialVm>()
            {
                Items = listMaterialVm,
                PageSize = Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
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
                listMaterial = listMaterial.Where(x => x.MaterialName.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            listMaterial = listMaterial.OrderByDescending(x => x.MaterialName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listMaterial.ToPagedList(pageIndex, Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listMaterialVm = listPaging.Select(x => new MaterialVm()
            {
                MaterialId = x.MaterialId,
                MaterialName = x.MaterialName,
                Description = x.Description,
                Color = x.Color,
                MaterialImage = x.MaterialImage,
                Status = x.Status,
                Price = x.Price,
                EffectDate = x.EffectDate
            }).ToList();
            var listResult = new PageResult<MaterialVm>()
            {
                Items = listMaterialVm,
                PageSize = Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listMaterial.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<MaterialVm>>(listResult, "Success");
        }
    }
}
