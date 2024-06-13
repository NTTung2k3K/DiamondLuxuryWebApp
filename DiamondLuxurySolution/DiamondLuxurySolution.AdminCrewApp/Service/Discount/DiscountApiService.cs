using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Discount;
using System;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Discount
{
    public class DiscountApiService : BaseApiService, IDiscountApiService
    {
        public DiscountApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }
        public async Task<ApiResult<bool>> CreateDiscount(CreateDiscountRequest request)
        {
            var data = await PostAsync<bool>("api/Discounts/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteDiscount(DeleteDiscountRequest request)
        {
            var data = await DeleteAsync<bool>($"api/Discounts/Delete?discountId={request.DiscountId}");
            return data;
        }

        public async Task<ApiResult<DiscountVm>> GetDiscountById(string DiscountId)
        {
            var data = await GetAsync<DiscountVm>($"api/Discounts/GetById?discountId={DiscountId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdateDiscount(UpdateDiscountRequest request)
        {
            var data = await PutAsync<bool>("api/Discounts/Update", request);
            return data;
        }

        public Task<ApiResult<PageResult<DiscountVm>>> ViewDiscountInCustomer(ViewDiscountRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PageResult<DiscountVm>>> ViewDiscountInManager(ViewDiscountRequest request)
        {
            var data = await GetAsync<PageResult<DiscountVm>>($"api/Discounts/ViewInManager?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
