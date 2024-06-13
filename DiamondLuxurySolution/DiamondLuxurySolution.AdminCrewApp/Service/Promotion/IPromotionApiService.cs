using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Promotion;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Promotion
{
    public interface IPromotionApiService
    {
        public Task<ApiResult<List<PromotionVm>>> GetAll();
        public Task<ApiResult<bool>> CreatePromotion(CreatePromotionRequest request);
        public Task<ApiResult<bool>> UpdatePromotion(UpdatePromotionRequest request);
        public Task<ApiResult<bool>> DeletePromotion(DeletePromotionRequest request);
        public Task<ApiResult<PromotionVm>> GetPromotionById(Guid PromotionId);
        public Task<ApiResult<PageResult<PromotionVm>>> ViewPromotionInCustomer(ViewPromotionRequest request);

        public Task<ApiResult<PageResult<PromotionVm>>> ViewPromotionInManager(ViewPromotionRequest request);
    }
}
