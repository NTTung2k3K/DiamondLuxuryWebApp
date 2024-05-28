﻿using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Contact
{
    public interface IContactRepo
    {
        public Task<ApiResult<bool>> CreateContact(CreateContactRequest request);
        public Task<ApiResult<bool>> UpdateContact(UpdateContactRequest request);
        public Task<ApiResult<bool>> DeleteContact(DeleteContactRequest request);
        public Task<ApiResult<ContactVm>> GetContactById(int ContactId);
        public Task<ApiResult<PageResult<ContactVm>>> ViewContactInPaging(ViewContactRequest request);
    }
}