using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.EF
{
    public class MyDbContext : IdentityDbContext<AppUser,AppRole,Guid>
    {

    }
}
