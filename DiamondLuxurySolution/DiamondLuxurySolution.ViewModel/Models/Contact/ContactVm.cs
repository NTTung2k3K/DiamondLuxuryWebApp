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
        public string Content { get; set; } = null!;
        public bool IsResponse { get; set; }
        public DateTime DateSendRequest { get; set; }

    }
}
