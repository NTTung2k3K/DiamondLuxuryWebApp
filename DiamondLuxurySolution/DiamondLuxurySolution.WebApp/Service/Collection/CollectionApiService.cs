using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Collection;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using DiamondLuxurySolution.ViewModel.Models.Product;
using DiamondLuxurySolution.WebApp.Services;

namespace DiamondLuxurySolution.WebApp.Service.Collection
{
    public class CollectionApiService : BaseApiService, ICollectionApiService
    {
        public CollectionApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> CreateCollection(CreateCollectionRequest request)
        {
            var data = await PostAsyncHasImage<bool>("api/Collections/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteCollection(DeleteCollectionRequest request)
        {
            var data = await DeleteAsync<bool>($"api/Collections/Delete?CollectionId={request.CollectionId}");
            return data;
        }

        public async Task<ApiResult<List<CollectionVm>>> GetAll()
        {
            var data = await GetAsync<List<CollectionVm>>("api/Collections/GetAll");
            return data;
        }
        public async Task<ApiResult<List<ProductVm>>> GetProductsByListId(List<string> ListProductsId)
        {
            var data = await PostAsync<List<ProductVm>>($"api/Collections/GetProductsByListId",ListProductsId);
            return data;
        }
        public async Task<ApiResult<CollectionVm>> GetCollectionById(string CollectionId)
        {
            var data = await GetAsync<CollectionVm>($"api/Collections/GetById?CollectionId={CollectionId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdateCollection(UpdateCollectionRequest request)
        {
            var data = await PutAsyncHasImage<bool>("api/Collections/Update", request);
            return data;
        }

        public async Task<ApiResult<PageResult<CollectionVm>>> ViewCollectionInPaging(ViewCollectionRequest request)
        {
            var data = await GetAsync<PageResult<CollectionVm>>($"api/Collections/ViewInCollection?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}

