using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Frame;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Frame
{
    public interface IFrameApiService
    {
        public Task<ApiResult<List<FrameVm>>> GetAll();
        public Task<ApiResult<bool>> CreateFrame(CreateFrameRequest request);
        public Task<ApiResult<bool>> UpdateFrame(UpdateFrameRequest request);
        public Task<ApiResult<bool>> DeleteFrame(DeleteFrameRequest request);
        public Task<ApiResult<FrameVm>> GetFrameById(string FrameId);
        public Task<ApiResult<PageResult<FrameVm>>> ViewFrameInPaging(ViewFrameRequest request);
    }
}
