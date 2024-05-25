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
    public class CategoryRepo : ICategoryRepo
    {
        public Task<ApiResult<bool>> CreateCategory(CreateCategoryRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> DeleteCategory(DeleteCategoryRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<AboutVm>> GetCategoryById(int CategoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdateCategory(UpdateCategoryRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<PageResult<CategoryVm>>> ViewCategory(ViewCategoryRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
