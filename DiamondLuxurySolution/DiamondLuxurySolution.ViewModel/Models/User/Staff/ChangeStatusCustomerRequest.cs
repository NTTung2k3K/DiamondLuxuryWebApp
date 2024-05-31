using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.User.Staff
{
    public class ChangeStatusCustomerRequest
    {
        public string Email { get; set; }
        [Required(ErrorMessage ="Yêu cầu trạng thái của khách hàng")]
        public string Status { get; set; }
    }
}
