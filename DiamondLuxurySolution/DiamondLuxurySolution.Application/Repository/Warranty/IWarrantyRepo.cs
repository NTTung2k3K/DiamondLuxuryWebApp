using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Warranty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Warranty
{
    public interface IWarrantyRepo
    {
        public Task<ApiResult<bool>> CreateWarranty(CreateWarrantyRequest request);
        public Task<ApiResult<bool>> UpdateWarranty(UpdateWarrantyRequest request);
        public Task<ApiResult<bool>> DeleteWarranty(DeleteWarrantyRequest request);
        public Task<ApiResult<WarrantyVm>> GetWarrantyById(Guid WarrantyId);
        public Task<ApiResult<PageResult<WarrantyVm>>> ViewWarrantyInCustomer(ViewWarrantyRequest request);

        public Task<ApiResult<PageResult<WarrantyVm>>> ViewWarrantyInManager(ViewWarrantyRequest request);
    }
}
