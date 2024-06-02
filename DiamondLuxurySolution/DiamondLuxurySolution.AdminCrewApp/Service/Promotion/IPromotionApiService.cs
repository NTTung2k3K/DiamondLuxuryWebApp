using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Promotion
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
