using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Gem
{
    public interface IGemRepo
    {
        public Task<ApiResult<bool>> CreateGem(CreateGemRequest request);
        public Task<ApiResult<bool>> UpdateGem(UpdateGemResquest request);
        public Task<ApiResult<bool>> DeleteGem(DeleteGemRequest request);
        public Task<ApiResult<GemVm>> GetGemById(Guid GemId);
        public Task<ApiResult<PageResult<GemVm>>> ViewGem(ViewGemRequest request);
    }
}
