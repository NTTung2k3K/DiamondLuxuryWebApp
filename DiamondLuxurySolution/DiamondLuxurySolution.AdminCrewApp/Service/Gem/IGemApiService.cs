using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using DiamondLuxurySolution.ViewModel.Models.Gem;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Gem
{
    public interface IGemApiService
    {
        public Task<ApiResult<List<GemVm>>> GetAll();
        public Task<ApiResult<bool>> CreateGem(CreateGemRequest request);
        public Task<ApiResult<bool>> UpdateGem(UpdateGemResquest request);
        public Task<ApiResult<bool>> DeleteGem(DeleteGemRequest request);
        public Task<ApiResult<GemVm>> GetGemById(Guid GemId);
        public Task<ApiResult<PageResult<GemVm>>> ViewGemInCustomer(ViewGemRequest request);
        public Task<ApiResult<PageResult<GemVm>>> ViewGemInManager(ViewGemRequest request);
    }
}
