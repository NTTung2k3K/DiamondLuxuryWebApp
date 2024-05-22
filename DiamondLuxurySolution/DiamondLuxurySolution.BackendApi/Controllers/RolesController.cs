using DiamondLuxurySolution.Application.Repository;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.Role;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IRoleRepo _Role;

        public RolesController(LuxuryDiamondShopContext context, IRoleRepo Role)
        {
            _context = context;
            _Role = Role;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateRole([FromForm] CreateRoleRequest request)
        {
            try
            {
                var status = await _Role.CreateRole(request);
                if (status.IsSuccessed)
                {
                    return Ok(status);
                }
                return BadRequest(status);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<ActionResult> UpdateRole([FromForm] UpdateRoleRequest request)
        {
            try
            {
                var status = await _Role.UpdateRole(request);
                if (status.IsSuccessed)
                {
                    return Ok(status);
                }
                return BadRequest(status);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteRole([FromBody] DeleteRoleRequest request)
        {
            try
            {
                var status = await _Role.DeleteRole(request);
                if (status.IsSuccessed)
                {
                    return Ok(status);
                }
                return BadRequest(status);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> FindById([FromQuery] Guid RoleId)
        {
            try
            {
                var status = await _Role.GetRoleById(RoleId);
                if (status.IsSuccessed)
                {
                    return Ok(status);
                }
                return BadRequest(status);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("View")]
        public async Task<IActionResult> ViewAllRolePagination([FromQuery] ViewRoleRequest request)
        {
            try
            {
                var status = await _Role.GetRolePagination(request);
                if (status.IsSuccessed)
                {
                    return Ok(status);
                }
                return BadRequest(status);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("GetAllRole")]
        public async Task<IActionResult> GetAllRole()
        {
            try
            {
                var status = await _Role.GetRolesForView();
                if (status.IsSuccessed)
                {
                    return Ok(status);
                }
                return BadRequest(status);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
