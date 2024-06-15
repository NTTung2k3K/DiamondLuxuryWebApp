using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.SubGem;
using Microsoft.AspNetCore.Http;

namespace DiamondLuxurySolution.AdminCrewApp.Service.SubGem
{
    public class SubGemApiService : BaseApiService, ISubGemApiService
    {
        public SubGemApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> CreateSubGem(CreateSubGemRequest request)
        {
            var data = await PostAsync<bool>("api/SubGems/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteSubGem(DeleteSubGemRequest request)
        {
            var data = await DeleteAsync<bool>($"api/SubGems/Delete?SubGemId={request.SubGemId}");
            return data;
        }
        public Task<ApiResult<PageResult<InspectionCertificateVm>>> ViewInspectionCertificateInCustomer(ViewInspectionCertificateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<List<SubGemVm>>> GetAll()
        {
            var data = await GetAsync<List<SubGemVm>>("api/SubGems/GetAll");
            return data;
        }
        public async Task<ApiResult<SubGemVm>> GetSubGemId(Guid SubGemId)
        {
            var data = await GetAsync<SubGemVm>($"api/SubGems/GetById?SubGemId={SubGemId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdateSubGem(UpdateSubGemRequest request)
        {
            var data = await PutAsync<bool>("api/SubGems/Update", request);
            return data;
        }
        public async Task<ApiResult<PageResult<SubGemVm>>> ViewSubGemInManager(ViewSubGemRequest request)
        {
            var data = await GetAsync<PageResult<SubGemVm>>($"api/SubGems/ViewInManager?KeyWord={request.KeyWord}&pageIndex={request.pageIndex}");
            return data;
        }

    }
}
