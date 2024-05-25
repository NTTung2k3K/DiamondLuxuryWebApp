using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Category
{
    public interface ICategoryRepo
    {
        public Task<ApiResult<bool>> CreateCategory(CreateCategoryRequest request);
        public Task<ApiResult<bool>> UpdateCategory(UpdateCategoryRequest request);
        public Task<ApiResult<bool>> DeleteCategory(DeleteCategoryRequest request);
        public Task<ApiResult<CategoryVm>> GetCategoryById(int CategoryId);
        public Task<ApiResult<PageResult<CategoryVm>>> ViewCategoryInCustomer(ViewCategoryRequest request);

        public Task<ApiResult<PageResult<CategoryVm>>> ViewCategoryInManager(ViewCategoryRequest request);

    }
}
