using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Payment;
using System;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Payment
{
    public class PaymentApiService : BaseApiService, IPaymentApiService
    {
        public PaymentApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }
        public async Task<ApiResult<bool>> CreatePayment(CreatePaymentRequest request)
        {
            var data = await PostAsync<bool>("api/Payments/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeletePayment(DeletePaymentRequest request)
        {
            var data = await DeleteAsync<bool>($"api/Payments/Delete?PaymentId="+request.PaymentId);
            return data;
        }

        public async Task<ApiResult<List<PaymentVm>>> GetAll()
        {
            var data = await GetAsync<List<PaymentVm>>("api/Payments/GetAll");
            return data;
        }

        public async Task<ApiResult<PaymentVm>> GetPaymentById(Guid PaymentId)
        {
            var data = await GetAsync<PaymentVm>($"api/Payments/GetById?PaymentId={PaymentId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdatePayment(UpdatePaymentRequest request)
        {
            var data = await PutAsync<bool>("api/Payments/Update", request);
            return data;
        }

        public async Task<ApiResult<PageResult<PaymentVm>>> ViewInPayment(ViewPaymentRequest request)
        {
            var data = await GetAsync<PageResult<PaymentVm>>($"api/Payments/ViewInPayment?KeyWord={request.KeyWord}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
