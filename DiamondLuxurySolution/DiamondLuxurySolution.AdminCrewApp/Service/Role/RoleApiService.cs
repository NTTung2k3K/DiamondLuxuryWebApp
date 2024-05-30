using Azure.Core;
using DiamondLuxurySolution.AdminCrewApp.Services;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Role;

namespace DiamondLuxurySolution.AdminCrewApp.Service.Role
{
    public class RoleApiService : BaseApiService, IRoleApiService
    {
        public RoleApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
        }

        public async Task<ApiResult<bool>> CreateRole(CreateRoleRequest request)
        {
            var data = await PostAsync<bool>("api/Roles/Create", request);
            return data;
        }

        public async Task<ApiResult<bool>> DeleteRole(DeleteRoleRequest request)
        {
            var data = await DeleteAsync<bool>("api/Roles/Delete?roleId="+request.RoleId);
            return data;
        }

        public async Task<ApiResult<RoleVm>> GetRoleById(Guid RoleId)
        {
            var data = await GetAsync<RoleVm>("api/Roles/GetById?RoleId="+RoleId);
            return data;
        }

        public async Task<ApiResult<PageResult<RoleVm>>> GetRolePagination(ViewRoleRequest request)
        {
            var data = await GetAsync<PageResult<RoleVm>>($"api/Roles/View?Keyword={request.Keyword}&pageIndex={request.pageIndex}");
            return data;
        }

        public async Task<ApiResult<List<RoleVm>>> GetRolesForView()
        {
            var data = await GetAsync<List<RoleVm>>("api/Roles/GetAllRole");
            return data;
        }

        public async Task<ApiResult<bool>> UpdateRole(UpdateRoleRequest request)
        {
            var data = await PutAsync<bool>("api/Roles/Update", request);
            return data;
        }
    }
}
