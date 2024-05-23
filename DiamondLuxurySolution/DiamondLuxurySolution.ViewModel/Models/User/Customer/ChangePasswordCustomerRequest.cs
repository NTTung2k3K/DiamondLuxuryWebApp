using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.User.Customer
{
    public class ChangePasswordCustomerRequest
    {
        public Guid CustomerId { get; set; }
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
