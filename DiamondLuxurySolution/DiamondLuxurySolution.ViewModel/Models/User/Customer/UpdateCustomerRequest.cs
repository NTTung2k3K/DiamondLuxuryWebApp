using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.User.Customer
{
    public class UpdateCustomerRequest
    {
        public Guid CustomerId { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime? Dob { get; set; }
        public string? Address { get; set; }
       
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }

        public string? ConfirmPassword { get; set; }

    }
}
