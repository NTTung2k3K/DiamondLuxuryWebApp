using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using DiamondLuxurySolution.ViewModel.Models.Platform;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Contact
{
    public interface IContactApiService
    {
        public Task<ApiResult<List<ContactVm>>> GetAll();
        public Task<ApiResult<bool>> CreateContact(CreateContactRequest request);
        public Task<ApiResult<bool>> UpdateContact(UpdateContactRequest request);
        public Task<ApiResult<bool>> DeleteContact(DeleteContactRequest request);
        public Task<ApiResult<ContactVm>> GetContactById(int ContactId);
        public Task<ApiResult<PageResult<ContactVm>>> ViewContactInPaging(ViewContactRequest request);
    }
}
