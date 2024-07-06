using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Product;

namespace DiamondLuxurySolution.WebApp.Service.Product
{
    public interface ISearchProductApiService
    {
        public Task<ApiResult<PageResult<ProductVm>>> ViewProduct(ViewProductRequest request);
        public Task<ApiResult<List<ProductVm>>> GetAll();
    }
}