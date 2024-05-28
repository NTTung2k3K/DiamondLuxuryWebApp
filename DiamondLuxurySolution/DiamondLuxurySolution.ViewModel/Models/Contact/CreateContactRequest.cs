using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Contact
{
    public class CreateContactRequest
    {
        public string? ContactNameUser { get; set; } 
        public string? ContactEmailUser { get; set; }
        public string? ContactPhoneUser { get; set; }
        public string? Content { get; set; }
    }
}
