using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Role
{
    public interface IRoleInitializer
    {
        Task<bool> CreateDefaultRole();
         Task<bool> CreateAdminAccount();

    }
}
