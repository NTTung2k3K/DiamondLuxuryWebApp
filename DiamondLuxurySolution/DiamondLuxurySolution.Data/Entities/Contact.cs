using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.Entities
{
    public class Contact
    {
        public int ContactId { get; set; } 
        public string? ContactNameUser { get; set; }
        public string? ContactEmailUser { get; set; }
        public string? ContactPhoneUser { get; set;}
        public string? Content {  get; set; }
        public DateTime DateSendRequest { get; set; }

        public bool IsResponse{ get; set; }
    }
}
