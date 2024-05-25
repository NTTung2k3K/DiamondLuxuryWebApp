using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Material;
using DiamondLuxurySolution.ViewModel.Models.MaterialPriceList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.MaterialPriceList
{
    public interface IMaterialPriceListRepo
    {
        public Task<ApiResult<bool>> CreateMaterialPriceList(CreateMaterialPriceListRequest request);
        public Task<ApiResult<bool>> UpdateMaterialPriceList(UpdateMaterialPriceListRequest request);
        public Task<ApiResult<bool>> DeleteMaterialPriceList(DeleteMaterialPriceListRequest request);
        public Task<ApiResult<MaterialPriceListVm>> GetMaterialPriceListById(Guid MaterialId);
        public Task<ApiResult<PageResult<MaterialPriceListVm>>> ViewMaterialPriceListInCustomer(ViewMaterialPriceListRequest request);
        public Task<ApiResult<PageResult<MaterialPriceListVm>>> ViewMaterialPriceListInManager(ViewMaterialPriceListRequest request);
    }
}
