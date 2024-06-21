using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Contact
{
    public class CreateContactRequest
    {
        [Required(ErrorMessage = "Người dùng cần nhập tên")]
        public string? ContactNameUser { get; set; }
		[Required(ErrorMessage = "Người dùng cần nhập email")]
        [EmailAddress]
		public string? ContactEmailUser { get; set; }
        [Required(ErrorMessage = "Người dùng cần nhập SDT")]
        public string? ContactPhoneUser { get; set; }
		[Required(ErrorMessage = "Người dùng cần nhập mô tả cụ thể")]
		public string? Content { get; set; }
    }
}
