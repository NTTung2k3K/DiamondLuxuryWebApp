using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Contact
{
    public class UpdateContactRequest
    {
        public int ContactId { get; set; }
        [Required(ErrorMessage = "Người dùng cần nhập tên")]
        public string? ContactNameUser { get; set; }
        public string? ContactEmailUser { get; set; }
        [Required(ErrorMessage = "Người dùng cần nhập SDT")]
        public string? ContactPhoneUser { get; set; }
        public string? Content { get; set; }
        public bool IsResponse { get; set; }

    }
}
