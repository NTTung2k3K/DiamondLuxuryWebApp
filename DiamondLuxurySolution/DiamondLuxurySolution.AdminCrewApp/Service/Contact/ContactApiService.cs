using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Contact
{
    public class ContactApiService : BaseApiService, IContactApiService
    {
        public ContactApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> CreateContact(CreateContactRequest request)
        {
            var data = await PostAsync<bool>("api/Contact/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteContact(DeleteContactRequest request)
        {
            var data = await DeleteAsync<bool>($"api/Contact/Delete?ContactId={request.ContactId}");
            return data;
        }

        public async Task<ApiResult<List<ContactVm>>> GetAll()
        {
            var data = await GetAsync<List<ContactVm>>("api/Contact/GetAll");
            return data;
        }

        public async Task<ApiResult<ContactVm>> GetContactById(int ContactId)
        {
            var data = await GetAsync<ContactVm>($"api/Contact/GetById?ContactId={ContactId}");
            return data;
        }

        public async Task<ApiResult<bool>> UpdateContact(UpdateContactRequest request)
        {
            var data = await PutAsync<bool>("api/Contact/Update", request);
            return data;
        }

        public async Task<ApiResult<PageResult<ContactVm>>> ViewContactInPaging(ViewContactRequest request)
        {
            var data = await GetAsync<PageResult<ContactVm>>($"api/Contact/ViewInContact?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }
    }
}
