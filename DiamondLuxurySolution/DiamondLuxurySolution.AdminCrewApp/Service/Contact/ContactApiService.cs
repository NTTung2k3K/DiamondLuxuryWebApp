using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using System;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Contact
{
    public class ContactApiService : BaseApiService, IContactApiService
    {
        public ContactApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public Task<ApiResult<bool>> CreateContact(CreateContactRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> DeleteContact(DeleteContactRequest request)
        {
            var data = await DeleteAsync<ApiResult<bool>>($"api/Contact/Delete?ContactId={request.ContactId}");
            return data;
        }

        public async Task<ApiResult<List<ContactVm>>> GetAll()
        {
            var data = await GetAsync<ApiResult<List<ContactVm>>>("api/Contact/GetAll");
            return data;
        }

        public async Task<ApiResult<ContactVm>> GetContactById(int ContactId)
        {
            var data = await GetAsync<ApiResult<ContactVm>>($"api/Contact/GetById?ContactId={ContactId}");
            return data;
        }

        public Task<ApiResult<bool>> UpdateContact(UpdateContactRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<PageResult<ContactVm>>> ViewContactInPaging(ViewContactRequest request)
        {
            var data = await GetAsync<ApiResult<PageResult<ContactVm>>>($"api/Contact/ViewInContact?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
