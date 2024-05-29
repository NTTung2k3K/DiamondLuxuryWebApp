using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Platform;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Platform
{
    public interface IPlatformApiService
    {
        public Task<ApiResult<List<PlatfromVm>>> GetAll();
        public Task<ApiResult<bool>> CreatePlatform(CreatePlatformRequest request);
        public Task<ApiResult<bool>> UpdatePlatform(UpdatePlatformRequest request);
        public Task<ApiResult<bool>> DeletePlatform(DeletePlatformRequest request);
        public Task<ApiResult<PlatfromVm>> GetPlatfromById(int PlatformId);
        public Task<ApiResult<PageResult<PlatfromVm>>> ViewPlatfromInCustomer(ViewPlatformRequest request);

        public Task<ApiResult<PageResult<PlatfromVm>>> ViewPlatfromInManager(ViewPlatformRequest request);
    }
}
