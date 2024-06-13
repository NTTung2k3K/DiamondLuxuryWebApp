using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using System;

namespace DiamondLuxurySolution.AdminCrewApp.Service.IInspectionCertificate
{
        public class InspectionCertificateApiService : BaseApiService, IInspectionCertificateApiService {
        public InspectionCertificateApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> CreateInspectionCertificate(CreateInspectionCertificateRequest request)
        {
            var data = await PostAsyncHasImage<bool>("api/InspectionCertificates/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteInspectionCertificate(DeleteInspectionCertificateRequest request)
        {
            var data = await DeleteAsync<bool>($"api/InspectionCertificates/Delete?InspectionCertificateId={request.InspectionCertificateId}");
            return data;
        } 

        public async Task<ApiResult<List<InspectionCertificateVm>>> GetAll()
        {
            var data = await GetAsync<List<InspectionCertificateVm>>("api/InspectionCertificates/GetAll");
            return data;
        }

        public async Task<ApiResult<InspectionCertificateVm>> GetInspectionCertificateById(string InspectionCertificateId)
        {
            var data = await GetAsync<InspectionCertificateVm>($"api/InspectionCertificates/GetById?InspectionCertificateId={InspectionCertificateId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdateInspectionCertificate(UpdateInspectionCertificateRequest request)
        {
            var data = await PutAsyncHasImage<bool>("api/InspectionCertificates/Update", request);
            return data;
        }


        public Task<ApiResult<PageResult<InspectionCertificateVm>>> ViewInspectionCertificateInCustomer(ViewInspectionCertificateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PageResult<InspectionCertificateVm>>> ViewInspectionCertificateInManager(ViewInspectionCertificateRequest request)
        {
            var data = await GetAsync<PageResult<InspectionCertificateVm>>($"api/InspectionCertificates/ViewInManager?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
