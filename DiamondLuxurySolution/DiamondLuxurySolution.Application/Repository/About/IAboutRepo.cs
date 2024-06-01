using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Slide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.About
{
    public interface IAboutRepo
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
