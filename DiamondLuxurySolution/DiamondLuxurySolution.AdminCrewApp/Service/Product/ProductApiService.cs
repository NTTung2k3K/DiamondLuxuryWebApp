using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Product;
using DiamondLuxurySolution.ViewModel.Models.SubGem;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Product
{
    public class ProductApiService : BaseApiService, IProductApiService
    {
        public ProductApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> CreateProduct(CreateProductRequest request)
        {
            var data = await PostAsyncHasImageAndListImage<bool>("api/Products/Create", request,request.Images);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteProduct(DeleteProductRequest request)
        {
            var data = await DeleteAsync<bool>("api/Products/Delete?ProductId="+ request.ProductId);
            return data;
        }

        public async Task<ApiResult<List<SubGemVm>>> GetAll()
        {
            var data = await GetAsync<List<SubGemVm>>("api/SubGems/GetAll");
            return data;
        }

        public async Task<ApiResult<ProductVm>> GetProductById(string ProductId)
        {
            var data = await GetAsync<ProductVm>("api/Products/GetProductById?ProductId=" + ProductId);
            return data;
        }

        public async Task<ApiResult<bool>> UpdateProduct(UpdateProductRequest request)
        {
            if (request.Images != null)
            {
                var data = await PutAsyncHasImageAndListImage<bool>("api/Products/Update", request, request.Images);
                return data;
            }
            else
            {
                var data = await PutAsyncHasImage<bool>("api/Products/Update", request);
                return data;
            }
           
        }

        public async Task<ApiResult<PageResult<ProductVm>>> ViewProduct(ViewProductRequest request)
        {
            var data = await GetAsync<PageResult<ProductVm>> ($"api/Products/ViewProduct?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
