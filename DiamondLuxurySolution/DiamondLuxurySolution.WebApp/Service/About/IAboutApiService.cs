using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;

namespace DiamondLuxurySolution.WebApp.Service.About
{
    public interface IAboutApiService
    {
        public Task<ApiResult<List<AboutVm>>> GetAll();

    }
}
