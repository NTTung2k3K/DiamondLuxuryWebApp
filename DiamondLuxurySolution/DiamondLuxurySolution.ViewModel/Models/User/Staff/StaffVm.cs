using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.User.Staff
{
    public class StaffVm
    {
        public Guid StaffId { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }

        public string PhoneNumber { get; set; }
        public string CitizenIDCard { get; set; }

        public string Email { get; set; }
        public string Status { get; set; }

        public DateTime Dob { get; set; }
        public string Image { get; set; }
        public List<string> ListRoleName { get; set; }
    }
}
