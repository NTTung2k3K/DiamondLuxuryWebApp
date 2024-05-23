using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Gem
{
    public class GemRepo : IGemRepo
    {
        public Task<ApiResult<bool>> CreateGem(CreateGemRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> DeleteGem(DeleteGemRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<GemVm>> GetGemById(Guid GemId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdateGem(UpdateGemResquest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<PageResult<GemVm>>> ViewGem(ViewGemRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
