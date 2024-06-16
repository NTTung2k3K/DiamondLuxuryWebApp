using DiamondLuxurySolution.WebApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;

namespace DiamondLuxurySolution.WebApp.Service.Contact
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
	}
}
