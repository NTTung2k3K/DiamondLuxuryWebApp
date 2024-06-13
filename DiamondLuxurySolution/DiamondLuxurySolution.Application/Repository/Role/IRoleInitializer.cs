using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Role
{
    public interface IRoleInitializer
    {
        Task CreateDefaultRole();
        Task CreateAdminAccount();
        Task CreateManagerAccount();
        Task CreateSaleStaffAccount();
        Task CreateShipperAccount();
        Task CreateCustomerAccount();
    }
}
