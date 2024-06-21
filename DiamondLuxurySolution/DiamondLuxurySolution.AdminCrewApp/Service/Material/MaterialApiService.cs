using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Material;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Material
{
    public class MaterialApiService : BaseApiService, IMaterialApiService
    {
        public MaterialApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> CreateMaterial(CreateMaterialRequest request)
        {
    /*        if (request.EffectDate != null)
            {
                DateTime date = (DateTime)request.EffectDate;
                request.EffectDate = date;

            }*/
            var data = await PostAsyncHasImage<bool>("api/Materials/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteMaterial(DeleteMaterialRequest request)
        {
            var data = await DeleteAsync<bool>($"api/Materials/Delete?MaterialId={request.MaterialId}");
            return data;
        }

        public async Task<ApiResult<List<MaterialVm>>> GetAll()
        {
            var data = await GetAsync<List<MaterialVm>>("api/Materials/GetAll");
            return data;
        }

        public async Task<ApiResult<MaterialVm>> GetMaterialById(Guid MaterialId)
        {
            var data = await GetAsync<MaterialVm>($"api/Materials/GetById?MaterialId={MaterialId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdateMaterial(UpdateMaterialRequest request)
        {
            var data = await PutAsyncHasImage<bool>("api/Materials/Update", request);
            return data;
        }

        public Task<ApiResult<PageResult<MaterialVm>>> ViewMaterialInCustomer(ViewMaterialRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PageResult<MaterialVm>>> ViewMaterialInManager(ViewMaterialRequest request)
        {
            
            var data = await GetAsync<PageResult<MaterialVm>>($"api/Materials/ViewInManager?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }

    }
}
