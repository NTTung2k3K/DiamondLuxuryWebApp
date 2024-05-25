using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Slide;
using DiamondLuxurySolution.ViewModel.Models.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Warehouse
{
    public interface IWarehouseRepo
    {
        public Task<ApiResult<bool>> CreateWarehouse(CreateWarehouseRequest request);
        public Task<ApiResult<bool>> UpdateWarehouse(UpdateWarehouseRequest request);
        public Task<ApiResult<bool>> DeleteWarehouse(DeleteWarehouseRequest request);
        public Task<ApiResult<WarehouseVm>> GetWarehouseById(int WarehouseId);
        public Task<ApiResult<PageResult<WarehouseVm>>> ViewWarehouse(ViewWarehouseRequest request);
    }
}
