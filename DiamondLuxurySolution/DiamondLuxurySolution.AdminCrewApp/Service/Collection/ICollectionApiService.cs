using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Collection;
using DiamondLuxurySolution.ViewModel.Models.Contact;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Collection
{
    public interface ICollectionApiService
    {
/*        public Task<ApiResult<List<ContactVm>>> GetAll();
*/      public Task<ApiResult<bool>> CreateCollection(CreateCollectionRequest request);
        public Task<ApiResult<bool>> UpdateCollection(UpdateCollectionRequest request);
        public Task<ApiResult<bool>> DeleteCollection(DeleteCollectionRequest request);
        public Task<ApiResult<CollectionVm>> GetCollectionById(string CollectiontId);
        public Task<ApiResult<PageResult<CollectionVm>>> ViewCollectionInPaging(ViewCollectionRequest request);
    }
}
