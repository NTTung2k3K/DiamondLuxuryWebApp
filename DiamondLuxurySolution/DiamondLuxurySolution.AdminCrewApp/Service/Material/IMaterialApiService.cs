using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Material;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Material
{
    public interface IMaterialApiService
    {
        public Task<ApiResult<List<MaterialVm>>> GetAll();
        public Task<ApiResult<bool>> CreateMaterial(CreateMaterialRequest request);
        public Task<ApiResult<bool>> UpdateMaterial(UpdateMaterialRequest request);
        public Task<ApiResult<bool>> DeleteMaterial(DeleteMaterialRequest request);
        public Task<ApiResult<MaterialVm>> GetMaterialById(Guid MaterialId);
        public Task<ApiResult<PageResult<MaterialVm>>> ViewMaterialInCustomer(ViewMaterialRequest request);
        public Task<ApiResult<PageResult<MaterialVm>>> ViewMaterialInManager(ViewMaterialRequest request);

    }
}
