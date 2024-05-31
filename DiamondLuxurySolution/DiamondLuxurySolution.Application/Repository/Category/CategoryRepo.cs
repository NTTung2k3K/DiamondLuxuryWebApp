using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Category
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public CategoryRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateCategory(CreateCategoryRequest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.CategoryName))
            {
                errorList.Add("Vui lòng nhập tên loại sản phẩm");
            }

            var category = new DiamondLuxurySolution.Data.Entities.Category
            {
                CategoryName = request.CategoryName,
                CategoryType = request.CategoryType != null ? request.CategoryType : "",
                Status = request.Status,
            };
            if (request.CategoryImage != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.CategoryImage);
                category.CategoryImage = firebaseUrl;
            }
            else
            {
                category.CategoryImage = "";
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> DeleteCategory(DeleteCategoryRequest request)
        {
            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy loại sản phẩm");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<CategoryVm>> GetCategoryById(int CategoryId)
        {
            var category = await _context.Categories.FindAsync(CategoryId);
            if (category == null)
            {
                return new ApiErrorResult<CategoryVm>("Không tìm loại sản phẩm");
            }
            var categoryVm = new CategoryVm()
            {
                CategoryId = CategoryId,
                CategoryName = category.CategoryName,
                CategoryImage = category.CategoryImage,
                CategoryType = category.CategoryType,
                Status = category.Status,
            };
            return new ApiSuccessResult<CategoryVm>(categoryVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateCategory(UpdateCategoryRequest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.CategoryName))
            {
                errorList.Add("Vui lòng nhập tên loại sản phẩm");
            }

            var category = await _context.Categories.FindAsync(request.CategoryId);
            if (category == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy loại sản phẩm");
            }
            if (request.CategoryImage != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.CategoryImage);
                category.CategoryImage = firebaseUrl;
            }
            else
            {
                category.CategoryImage = "";
            }
            category.CategoryName = request.CategoryName;
            category.CategoryType = request.CategoryType != null ? request.CategoryType : "";
            category.Status = request.Status;

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<CategoryVm>>> ViewCategoryInCustomer(ViewCategoryRequest request)
        {
            var listCategory = await _context.Categories.ToListAsync();
            if (request.Keyword != null)
            {
                listCategory = listCategory.Where(x => x.CategoryName.Contains(request.Keyword)).ToList();

            }
            listCategory = listCategory.Where(x => x.Status).OrderByDescending(x => x.CategoryName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listCategory.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listCategoryVm = listPaging.Select(x => new CategoryVm()
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                CategoryType = x.CategoryType,
                CategoryImage = x.CategoryImage,
                Status = x.Status,
            }).ToList();
            var listResult = new PageResult<CategoryVm>()
            {
                Items = listCategoryVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listCategory.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<CategoryVm>>(listResult, "Success");
        }

        public async Task<ApiResult<PageResult<CategoryVm>>> ViewCategoryInManager(ViewCategoryRequest request)
        {
            var listCategory = await _context.Categories.ToListAsync();
            if (request.Keyword != null)
            {
                listCategory = listCategory.Where(x => x.CategoryName.Contains(request.Keyword)).ToList();

            }
            listCategory = listCategory.OrderByDescending(x => x.CategoryName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listCategory.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listCategoryVm = listPaging.Select(x => new CategoryVm()
            {
                CategoryId = x.CategoryId,
                CategoryName = x.CategoryName,
                CategoryType = x.CategoryType,
                CategoryImage = x.CategoryImage,
                Status = x.Status,
            }).ToList();
            var listResult = new PageResult<CategoryVm>()
            {
                Items = listCategoryVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listCategory.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<CategoryVm>>(listResult, "Success");
        }
    }
}
