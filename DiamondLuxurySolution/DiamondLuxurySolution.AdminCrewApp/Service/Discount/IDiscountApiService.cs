using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Discount;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Discount
{
    public interface IDiscountApiService
    {
        public Task<ApiResult<bool>> CreateDiscount(CreateDiscountRequest request);
        public Task<ApiResult<bool>> UpdateDiscount(UpdateDiscountRequest request);
        public Task<ApiResult<bool>> DeleteDiscount(DeleteDiscountRequest request);
        public Task<ApiResult<DiscountVm>> GetDiscountById(string DiscountId);
        public Task<ApiResult<PageResult<DiscountVm>>> ViewDiscountInCustomer(ViewDiscountRequest request);

        public Task<ApiResult<PageResult<DiscountVm>>> ViewDiscountInManager(ViewDiscountRequest request);
    }
}