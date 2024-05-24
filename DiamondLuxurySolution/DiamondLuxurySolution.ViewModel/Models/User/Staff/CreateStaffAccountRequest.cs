using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.User.Staff
{
    public class CreateStaffAccountRequest
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }

        public string PhoneNumber { get; set; }
        public string CitizenIDCard { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public DateTime? Dob { get; set; }
        public string Password { get; set; }
        public string? Status { get; set; }

        public string ConfirmPassword { get; set; }

        public IFormFile? Image { get; set; }
        public List<Guid> RoleId { get; set; }

    }
}
