using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Role;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Role
{
    public interface IRoleApiService
    {
        public Task<ApiResult<PageResult<RoleVm>>> GetRolePagination(ViewRoleRequest request);
        public Task<ApiResult<bool>> CreateRole(CreateRoleRequest request);
        public Task<ApiResult<bool>> DeleteRole(DeleteRoleRequest request);
        public Task<ApiResult<bool>> UpdateRole(UpdateRoleRequest request);
        public Task<ApiResult<RoleVm>> GetRoleById(Guid RoleId);
        public Task<ApiResult<List<RoleVm>>> GetRolesForView();
    }
}
