using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Frame;
using DiamondLuxurySolution.ViewModel.Models.Gem;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Frame
{
    public class FrameApiService : BaseApiService, IFrameApiService
    {
        public FrameApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> CreateFrame(CreateFrameRequest request)
        {
            var data = await PostAsync<bool>("api/Frame/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteFrame(DeleteFrameRequest request)
        {
            var data = await DeleteAsync<bool>($"api/Frame/Delete?FrameId={request.FrameId}");
            return data;
        }

        public async Task<ApiResult<List<FrameVm>>> GetAll()
        {
            var data = await GetAsync<List<FrameVm>>("api/Frame/GetAll");
            return data;
        }

        public async Task<ApiResult<FrameVm>> GetFrameById(string FrameId)
        {
            var data = await GetAsync<FrameVm>($"api/Frame/GetById?FrameId={FrameId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdateFrame(UpdateFrameRequest request)
        {
            var data = await PutAsync<bool>("api/Frame/Update", request);
            return data;
        }

        public async Task<ApiResult<PageResult<FrameVm>>> ViewFrameInPaging(ViewFrameRequest request)
        {
            var data = await GetAsync<PageResult<FrameVm>>($"api/Frame/ViewInFrame?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
