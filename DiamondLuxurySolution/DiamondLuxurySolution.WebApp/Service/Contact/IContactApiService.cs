using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;

namespace DiamondLuxurySolution.WebApp.Service.Contact
{
	public interface IContactApiService
	{
		public Task<ApiResult<bool>> CreateContact(CreateContactRequest request);
	}
}