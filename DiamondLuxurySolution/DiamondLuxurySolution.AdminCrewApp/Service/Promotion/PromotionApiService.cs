using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.Application.Repository.Promotion;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Promotion;
using System;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Promotion
{
    public class PromotionApiService : BaseApiService, IPromotionApiService
    {
        public PromotionApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }
        public async Task<ApiResult<bool>> CreatePromotion(CreatePromotionRequest request)
        {
            var data = await PostAsyncHasImage<bool>("api/Promotions/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeletePromotion(DeletePromotionRequest request)
        {
            var data = await DeleteAsync<bool>($"api/Promotions/Delete?PromotionId=" + request.PromotionId);
            return data;
        }
        public async Task<ApiResult<List<PromotionVm>>> GetAll()
        {
            var data = await GetAsync<List<PromotionVm>>("api/Promotions/GetAll");
            return data;
        }

        public async Task<ApiResult<List<PromotionVm>>> GetAllOnTime()
        {
            var data = await GetAsync<List<PromotionVm>>("api/Promotions/GetAllOnTime");
            return data;
        }

        public async Task<ApiResult<PromotionVm>> GetPromotionById(Guid PromotionId)
        {
            var data = await GetAsync<PromotionVm>($"api/Promotions/GetById?PromotionId={PromotionId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdatePromotion(UpdatePromotionRequest request)
        {
            var data = await PutAsyncHasImage<bool>("api/Promotions/Update", request);
            return data;
        }

        public Task<ApiResult<PageResult<PromotionVm>>> ViewPromotionInCustomer(ViewPromotionRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PageResult<PromotionVm>>> ViewPromotionInManager(ViewPromotionRequest request)
        {
            var data = await GetAsync<PageResult<PromotionVm>>($"api/Promotions/ViewInManager?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
