using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Material
{
    public interface IMaterialRepo
    {
        public Task<ApiResult<bool>> CreateMaterial(CreateMaterialRequest request);
        public Task<ApiResult<bool>> UpdateMaterial(UpdateMaterialRequest request);
        public Task<ApiResult<bool>> DeleteMaterial(DeleteMaterialRequest request);
        public Task<ApiResult<MaterialVm>> GetMaterialById(Guid MaterialId);
        public Task<ApiResult<PageResult<MaterialVm>>> ViewMaterialInCustomer(ViewMaterialRequest request);

        public Task<ApiResult<PageResult<MaterialVm>>> ViewMaterialInManager(ViewMaterialRequest request);
    }
}
