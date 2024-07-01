using Azure.Core;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Collection;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Contact
{
    public class ContactRepo : IContactRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public ContactRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<List<int>>> ContactAWeek()
        {
            var today = DateTime.Today;

            // Calculate the start of the week (previous Monday)
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday - (today.DayOfWeek == DayOfWeek.Sunday ? 7 : 0));

            // Calculate the end of the week (current Sunday)
            var endOfWeek = startOfWeek.AddDays(6).Date.AddDays(1).AddTicks(-1);

            // Create a list to hold the count of contacts per day
            var contactsCount = new List<int>();

            for (var date = startOfWeek; date <= endOfWeek; date = date.AddDays(1))
            {
                var count = await _context.Contacts
                    .Where(x => x.DateSendRequest.Date == date.Date)
                    .CountAsync();

                contactsCount.Add(count);
            }

            // Return the results
            return new ApiSuccessResult<List<int>>(contactsCount, "Success");
        }


        public async Task<ApiResult<int>> CountContactNotSolve()
        {
            var count = _context.Contacts.Where(x => x.IsResponse == false).Count();
            return new ApiSuccessResult<int>(count, "Success");
        }



        public async Task<ApiResult<bool>> CreateContact(CreateContactRequest request)
        {
            List<string> errorList = new List<string>();
            if (string.IsNullOrWhiteSpace(request.ContactEmailUser))
            {
                errorList.Add("Vui lòng nhập email");
            }
            if (string.IsNullOrWhiteSpace(request.ContactNameUser))
            {
                errorList.Add("Vui lòng nhập tên");
            }

            if (string.IsNullOrWhiteSpace(request.ContactPhoneUser))
            {
                errorList.Add("Vui lòng nhập số điện thoại");
            }
            else
            {
                if (!Regex.IsMatch(request.ContactPhoneUser, "^(09|03|07|08|05)[0-9]{8,9}$"))
                {
                    errorList.Add("Số điện thoại không hợp lệ");
                }
            }

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                errorList.Add("Vui lòng nhập nội dung");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            var contact = new DiamondLuxurySolution.Data.Entities.Contact
            {
                Content = request.Content.Trim(),
                ContactPhoneUser = request.ContactPhoneUser.Trim(),
                ContactNameUser = request.ContactNameUser.Trim(),
                ContactEmailUser = request.ContactEmailUser.Trim(),
                IsResponse = false,
                DateSendRequest = DateTime.Now,
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> DeleteContact(DeleteContactRequest request)
        {
            var contact = await _context.Contacts.FindAsync(request.ContactId);
            if (contact == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy liên hệ");
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<List<ContactVm>>> GetAll()
        {
            var list = await _context.Contacts.ToListAsync();
            var rs = list.Select(x => new ContactVm()
            {
                ContactId = x.ContactId,
                ContactNameUser = x.ContactNameUser,
                ContactEmailUser = x.ContactEmailUser,
                ContactPhoneUser = x.ContactPhoneUser,
                Content = x.Content,
                IsResponse = x.IsResponse,
            }).ToList();
            return new ApiSuccessResult<List<ContactVm>>(rs);
        }

        public async Task<ApiResult<ContactVm>> GetContactById(int ContactId)
        {
            var contact = await _context.Contacts.FindAsync(ContactId);

            if (contact == null)
            {
                return new ApiErrorResult<ContactVm>("Không tìm thấy liên hệ");
            }
            var contactVm = new ContactVm()
            {
                ContactId = contact.ContactId,
                Content = contact.Content.Trim(),
                ContactPhoneUser = contact.ContactPhoneUser.Trim(),
                ContactNameUser = contact.ContactNameUser.Trim(),
                ContactEmailUser = contact.ContactEmailUser.Trim(),
                IsResponse = contact.IsResponse,
                DateSendRequest = contact.DateSendRequest,
            };
            return new ApiSuccessResult<ContactVm>(contactVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateContact(UpdateContactRequest request)
        {
            List<string> errorList = new List<string>();
            if (string.IsNullOrWhiteSpace(request.ContactEmailUser))
            {
                errorList.Add("Vui lòng nhập email");
            }
            if (string.IsNullOrWhiteSpace(request.ContactNameUser))
            {
                errorList.Add("Vui lòng nhập tên");
            }
            if (string.IsNullOrWhiteSpace(request.ContactPhoneUser)
                || Regex.IsMatch(request.ContactPhoneUser, "^[09|03|07|08|05] + [0-9]{8,9}$"))
            {
                errorList.Add("Số điện thoại không hợp lệ");
            }
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                errorList.Add("Vui lòng nhập nội dung");
            }
            var contact = await _context.Contacts.FindAsync(request.ContactId);
            if (contact == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy liên hệ");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            contact.IsResponse = request.IsResponse;
            contact.ContactEmailUser = request.ContactEmailUser;
            contact.ContactPhoneUser = request.ContactPhoneUser;
            contact.Content = request.Content;
            contact.ContactNameUser = request.ContactNameUser;

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }


        public async Task<ApiResult<PageResult<ContactVm>>> ViewContactInPaging(ViewContactRequest request)
        {
            var listContact = await _context.Contacts.ToListAsync();
            if (request.Keyword != null)
            {
                listContact = listContact.Where(x => x.ContactPhoneUser.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                || x.ContactNameUser.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase) || x.IsResponse.ToString().Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            listContact = listContact.OrderByDescending(x => x.IsResponse).ThenBy(x => x.ContactNameUser).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listContact.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listContactVm = listPaging.Select(contact => new ContactVm()
            {
                ContactId = contact.ContactId,
                Content = contact.Content.Trim(),
                ContactPhoneUser = contact.ContactPhoneUser.Trim(),
                ContactNameUser = contact.ContactNameUser.Trim(),
                ContactEmailUser = contact.ContactEmailUser.Trim(),
                IsResponse = contact.IsResponse,
                DateSendRequest = contact.DateSendRequest,

            }).ToList();
            var listResult = new PageResult<ContactVm>()
            {
                Items = listContactVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listContact.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<ContactVm>>(listResult, "Success");
        }
    }
}
