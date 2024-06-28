using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Category;
using DiamondLuxurySolution.ViewModel.Models.Product;
using DiamondLuxurySolution.ViewModel.Models.SubGem;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Product
{
    public interface IProductApiService
    {
        public Task<ApiResult<bool>> CreateProduct(CreateProductRequest request);
        public Task<ApiResult<bool>> UpdateProduct(UpdateProductRequest request);
        public Task<ApiResult<bool>> DeleteProduct(DeleteProductRequest request);
        public Task<ApiResult<ProductVm>> GetProductById(string ProductId);
        public Task<ApiResult<PageResult<ProductVm>>> ViewProduct(ViewProductRequest request);
        public Task<ApiResult<List<SubGemVm>>> GetAll();
        public Task<ApiResult<List<ProductVm>>> GetAllProduct();



    }
}
