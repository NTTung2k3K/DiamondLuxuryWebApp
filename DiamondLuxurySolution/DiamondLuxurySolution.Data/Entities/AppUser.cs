using DiamondLuxurySolution.Data.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.Entities
{
    public partial class AppUser : IdentityUser<Guid>
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime Dob { get; set; }
        public Status status {  get; set; }
        //public List<AppUserRole> AppUserRoles { get; set; }

        public virtual ICollection<AppUserRole> AppUserRoles { get; set; } = new List<AppUserRole>();
        public List<Order> Orders { get; set; }
        public List<KnowledgeNews> KnowledgeNews { get; set; }
        public List<News> News { get; set; }
    }
}
