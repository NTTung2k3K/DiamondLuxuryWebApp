using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Discount
{
    public interface IDiscountRepo
    {
        public Task<ApiResult<bool>> CreateDiscount(CreateDiscountRequest request);
        public Task<ApiResult<bool>> UpdateDiscount(UpdateDiscountRequest request);
        public Task<ApiResult<bool>> DeleteDiscount(DeleteDiscountRequest request);
        public Task<ApiResult<DiscountVm>> GetDiscountById(Guid DiscountId);
        public Task<ApiResult<PageResult<DiscountVm>>> ViewDiscount(ViewDiscountRequest request);
    }
}
