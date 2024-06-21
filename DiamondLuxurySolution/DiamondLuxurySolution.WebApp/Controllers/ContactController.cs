using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using Microsoft.AspNetCore.Mvc;
using DiamondLuxurySolution.WebApp.Service.Contact;

namespace DiamondLuxurySolution.WebApp.Controllers
{
	public class ContactController : Controller
	{
		private readonly IContactApiService _contactApiService;

		public ContactController(IContactApiService contactApiService)
		{
			_contactApiService = contactApiService;
		}
		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateContactRequest request)
		{


			var status = await _contactApiService.CreateContact(request);

			if (status is ApiErrorResult<bool> errorResult)
			{
				List<string> listError = new List<string>();

				if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
				{
					foreach (var error in errorResult.ValidationErrors)
					{
						listError.Add(error);
					}
				}
				else if (status.Message != null)
				{
					listError.Add(errorResult.Message);
				}
				ViewBag.Errors = listError;
					return View(request);

			}
			TempData["SuccessMsg"] = "Tạo mới thành công yêu cầu cho " + request.ContactNameUser;

			return RedirectToAction("Create", "Contact");
		}
	}
}
