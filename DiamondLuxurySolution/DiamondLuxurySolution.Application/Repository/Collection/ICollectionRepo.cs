using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Collection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Collection
{
    public interface ICollectionRepo
    {
        public Task<ApiResult<bool>> CreateCollection(CreateCollectionRequest request);
        public Task<ApiResult<bool>> UpdateCollection(UpdateCollectionRequest request);
        public Task<ApiResult<bool>> DeleteCollection(DeleteCollectionRequest request);
        public Task<ApiResult<CollectionVm>> GetCollectionById(string CollectionId);
        public Task<ApiResult<PageResult<CollectionVm>>> ViewCollectionInPaging(ViewCollectionRequest request);

    }
}
