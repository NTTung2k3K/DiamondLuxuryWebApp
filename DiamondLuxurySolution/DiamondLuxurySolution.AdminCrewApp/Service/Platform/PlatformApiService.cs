using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using System;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Platform
{
    public class PlatformApiService : BaseApiService, IPlatformApiService
    {
        public PlatformApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }
        public async Task<ApiResult<bool>> CreatePlatform(CreatePlatformRequest request)
        {
            var data = await PostAsyncHasImage<ApiResult<bool>>("api/Platforms/Create",request);
            return data;
        }

        public async Task<ApiResult<bool>> DeletePlatform(DeletePlatformRequest request)
        {
            var data = await DeleteAsync<ApiResult<bool>>($"api/Platforms/Delete?platformId={request.PlatformId}");
            return data;
        }

        public async Task<ApiResult<List<PlatfromVm>>> GetAll()
        {
            var data = await GetAsync<ApiResult<List<PlatfromVm>>>("api/Platforms/GetAll");
            return data;
        }

        public async Task<ApiResult<PlatfromVm>> GetPlatfromById(int PlatformId)
        {
            var data = await GetAsync<ApiResult<PlatfromVm>>($"api/Platforms/GetById?PlatformId={PlatformId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdatePlatform(UpdatePlatformRequest request)
        {
            var data = await PutAsyncHasImage<ApiResult<bool>>("api/Platforms/Update",request);
            return data;
        }

        public Task<ApiResult<PageResult<PlatfromVm>>> ViewPlatfromInCustomer(ViewPlatformRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PageResult<PlatfromVm>>> ViewPlatfromInManager(ViewPlatformRequest request)
        {
            var data = await GetAsync<ApiResult<PageResult<PlatfromVm>>>($"api/Platforms/ViewInManager?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
