using DiamondLuxurySolution.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Role
{
    public class RoleInitializer : IRoleInitializer
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        public RoleInitializer(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<bool> CreateDefaultRole()
        {
            var AdminRoleName = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin.ToString();
            var CustomerRoleName = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Customer.ToString();
            var DeliveryStaffRoleName = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.DeliveryStaff.ToString();
            var ManagerRoleName = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Manager.ToString();
            var SalesStaffRoleName = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.SalesStaff.ToString();

            var defaultRoleNameList = new List<string>();
            defaultRoleNameList.Add(AdminRoleName);
            defaultRoleNameList.Add(CustomerRoleName);
            defaultRoleNameList.Add(DeliveryStaffRoleName);
            defaultRoleNameList.Add(ManagerRoleName);
            defaultRoleNameList.Add(SalesStaffRoleName);

            foreach (var roleName in defaultRoleNameList)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    continue;
                }
                var roleAdd = new AppRole()
                {
                    Name = roleName,
                    Description = roleName
                };
                var status = await _roleManager.CreateAsync(roleAdd);
            }
            return true;
        }

        public async Task<bool> CreateAdminAccount()
        {
            var user = new AppUser()
            {
                Fullname = "Admin1@",
                Email = "Admin1@gmail.com",
                Dob = DateTime.Now,
                PhoneNumber = "0999999999",
                UserName = "Admin1@",
                Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.StaffStatus.Active.ToString(),
            };

            var status = await _userManager.CreateAsync(user, "Admin1@");

            var roleFindByName = await _roleManager.FindByNameAsync(DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Admin);
            await _userManager.AddToRoleAsync(user, roleFindByName.Name);
            _userManager.UpdateAsync(user);

            return true;
        }
    }

}
