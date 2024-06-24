using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Collection;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using DiamondLuxurySolution.ViewModel.Models.Product;

namespace DiamondLuxurySolution.WebApp.Service.Collection
{
    public interface ICollectionApiService
    {
        public Task<ApiResult<List<CollectionVm>>> GetAll();
        public Task<ApiResult<bool>> CreateCollection(CreateCollectionRequest request);
        public Task<ApiResult<bool>> UpdateCollection(UpdateCollectionRequest request);
        public Task<ApiResult<bool>> DeleteCollection(DeleteCollectionRequest request);
        public Task<ApiResult<CollectionVm>> GetCollectionById(string CollectiontId);
        public Task<ApiResult<PageResult<CollectionVm>>> ViewCollectionInPaging(ViewCollectionRequest request);
        public Task<ApiResult<List<ProductVm>>> GetProductsByListId(List<string> ListProductsId);
    }
}
