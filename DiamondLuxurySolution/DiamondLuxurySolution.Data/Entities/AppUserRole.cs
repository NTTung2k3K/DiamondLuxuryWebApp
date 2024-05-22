using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.Entities
{
    public class AppUserRole : IdentityUserRole<Guid>
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}
