using DiamondLuxurySolution.AdminCrewApp.Service.Contact;
using DiamondLuxurySolution.AdminCrewApp.Service.Platform;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactApiService _contactApiService;

        public ContactController(IContactApiService contactApiService)
        {
            _contactApiService = contactApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ViewContactRequest request)
        {
            try
            {

                ViewBag.txtLastSeachValue = request.Keyword;
                if (!ModelState.IsValid)
                {
                    return View();
                }
                if (TempData["FailMsg"] != null)
                {
                    ViewBag.FailMsg = TempData["FailMsg"];
                }
                if (TempData["SuccessMsg"] != null)
                {
                    ViewBag.SuccessMsg = TempData["SuccessMsg"];
                }

                var contact = await _contactApiService.ViewContactInPaging(request);
                return View(contact.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int ContactId)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View();
                }

                var User = await _contactApiService.GetContactById(ContactId);
                return View(User.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteContactRequest request)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return View();
                }
                var status = await _contactApiService.DeleteContact(request);
                if (status is ApiErrorResult<bool> errorResult)
                {
                    ViewBag.Errors = errorResult.ValidationErrors;
                    return View();
                }
                return RedirectToAction("Index", "Contact");

            }
            catch
            {
                return View();
            }
        }
    }
}
