using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Contact
{
    public class ContactVm
    {
        public int ContactId { get; set; }
        public string ContactNameUser { get; set; } = null!;
        public string ContactEmailUser { get; set; } = null!;
        public string ContactPhoneUser { get; set; } = null!;
        public string content { get; set; } = null!;
    }
}
