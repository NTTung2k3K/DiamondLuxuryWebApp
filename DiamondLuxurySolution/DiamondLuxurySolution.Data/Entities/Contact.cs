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
        public string? content {  get; set; }

    }
}
