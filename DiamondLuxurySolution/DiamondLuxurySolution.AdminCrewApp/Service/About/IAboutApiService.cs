using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;

namespace DiamondLuxurySolution.AdminCrewApp.Service.About
{
    public interface IAboutApiService
    {
        public Task<ApiResult<List<AboutVm>>> GetAll();
        public Task<ApiResult<bool>> CreateAbout(CreateAboutRequest request);
        public Task<ApiResult<bool>> UpdateAbout(UpdateAboutRequest request);
        public Task<ApiResult<bool>> DeleteAbout(DeleteAboutRequest request);
        public Task<ApiResult<AboutVm>> GetAboutById(int AboutId);
        public Task<ApiResult<PageResult<AboutVm>>> ViewAboutInCustomer(ViewAboutRequest request);

        public Task<ApiResult<PageResult<AboutVm>>> ViewAboutInManager(ViewAboutRequest request);
    }
}
