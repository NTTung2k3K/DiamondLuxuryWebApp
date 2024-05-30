using Azure.Core;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository
{
    public class RoleRepo : IRoleRepo
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleRepo(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ApiResult<bool>> CreateRole(CreateRoleRequest request)
        {
            var role = await _roleManager.FindByNameAsync(request.Name);
            if (role != null)
            {
                return new ApiErrorResult<bool>("Role đã tồn tại");
            }
            var errorList = new List<string>();
            if (request.Name == null)
            {
                errorList.Add("Yêu cầu tên của chức vụ");
            }
            if (request.Description == null)
            {
                errorList.Add("Yêu cầu mô tả của chức vụ");
            }
            if (errorList.Count > 0)
            {
                return new ApiErrorResult<bool>("Lỗi thông tin", errorList);
            }
            var roleAdd = new AppRole()
            {
                Name = request.Name,
                Description = request.Description
            };
            var status = await _roleManager.CreateAsync(roleAdd);
            if (!status.Succeeded)
            {
                return new ApiErrorResult<bool>("Lỗi hệ thống");
            }
            return new ApiSuccessResult<bool>(true, "Success");

        }

        public async Task<ApiResult<bool>> DeleteRole(DeleteRoleRequest request)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
            if (role == null)
            {
                return new ApiErrorResult<bool>("Role không tồn tại");
            }
            var status = await _roleManager.DeleteAsync(role);
            if (!status.Succeeded)
            {
                return new ApiErrorResult<bool>("Lỗi hệ thống");
            }
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<RoleVm>> GetRoleById(Guid RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId.ToString());
            if (role == null)
            {
                return new ApiErrorResult<RoleVm>("Role không tồn tại");
            }
            var RoleVm = new RoleVm()
            {
                RoleId = role.Id,
                Description = role.Description,
                Name = role.Name
            };
            return new ApiSuccessResult<RoleVm>(RoleVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateRole(UpdateRoleRequest request)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
            if (role == null)
            {
                return new ApiErrorResult<bool>("Role không tồn tại");
            }
            var errorList = new List<string>();
            if(request.Name == null)
            {
                errorList.Add("Yêu cầu tên của chức vụ");
            }
            if(request.Description == null)
            {
                errorList.Add("Yêu cầu mô tả của chức vụ");
            }
            if(errorList.Count > 0)
            {
                return new ApiErrorResult<bool>("Lỗi thông tin", errorList);
            }
            role.Name = request.Name;
            role.Description = request.Description;
            var status = await _roleManager.UpdateAsync(role);
            if (!status.Succeeded)
            {
                return new ApiErrorResult<bool>("Lỗi hệ thống");
            }
            return new ApiSuccessResult<bool>(true, "Success");
        }

      

        public async Task<ApiResult<PageResult<RoleVm>>> GetRolePagination(ViewRoleRequest request)
        {
            var listRole = await _roleManager.Roles.ToListAsync();
            if (request.Keyword != null)
            {
                listRole = listRole.Where(x => x.Name.Contains(request.Keyword) ||
                x.Description.Contains(request.Keyword)).ToList();
            }
            listRole = listRole.OrderBy(x => x.Name).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listRole.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listRoleViewModel = listPaging.Select(x => new RoleVm()
            {
                RoleId = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToList();
            var listResult = new PageResult<RoleVm>()
            {
                Items = listRoleViewModel,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listRole.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<RoleVm>>(listResult, "Success");
        }

        public async Task<ApiResult<List<RoleVm>>> GetRolesForView()
        {
            var listRole = await _roleManager.Roles.ToListAsync();
            var listRoleViewModel = listRole.Select(x => new RoleVm()
            {
                RoleId = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToList();
            return new ApiSuccessResult<List<RoleVm>>(listRoleViewModel, "Success");
        }

    }
}
