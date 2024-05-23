﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.User.Customer
{
    public class RegisterCustomerAccountRequest
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime Dob {  get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
}
