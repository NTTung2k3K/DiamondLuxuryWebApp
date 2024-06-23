using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Product;
using DiamondLuxurySolution.WebApp.Services;

namespace DiamondLuxurySolution.WebApp.Service.Product
{
    public class SearchProductApiService : BaseApiService, ISearchProductApiService
    {
        public SearchProductApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<PageResult<ProductVm>>> ViewProduct(ViewProductRequest request)
        {
            var data = await GetAsync<PageResult<ProductVm>>($"api/Products/ViewProduct?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}