using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.Entities
{
    public partial class AppRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
        public virtual ICollection<AppUserRole> AppUserRoles { get; set; } = new List<AppUserRole>();
    }
}
