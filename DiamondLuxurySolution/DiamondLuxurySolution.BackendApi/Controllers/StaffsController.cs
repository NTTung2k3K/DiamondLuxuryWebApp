using DiamondLuxurySolution.Application.Repository.User.Staff;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffsController : ControllerBase
    {

        private readonly LuxuryDiamondShopContext _context;
        private readonly IStaffRepo _Staff;

        public StaffsController(LuxuryDiamondShopContext context, IStaffRepo Staff)
        {
            _context = context;
            _Staff = Staff;
        }


        [HttpDelete("DeleteStaff/{id}")]
        public async Task<ActionResult> DeleteStaff([FromBody] Guid StaffId)
        {
            try
            {
                var status = await _Staff.DeleteStaff(StaffId);
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

        [HttpGet("GetStaffById")]
        public async Task<ActionResult> GetStaffById([FromQuery] Guid StaffId)
        {
            try
            {
                var status = await _Staff.GetStaffById(StaffId);
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
        public async Task<ActionResult> ViewAllStaff([FromQuery] ViewStaffPaginationRequest request)
        {
            try
            {
                var status = await _Staff.ViewStaffPagination(request);
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

        [HttpPost("LoginStaff")]
        public async Task<ActionResult> LoginStaff([FromBody] LoginStaffRequest request)
        {
            try
            {
                var status = await _Staff.LoginStaff(request);
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

        [HttpPost("RegisterStaff")]
        public async Task<ActionResult> Register([FromForm] CreateStaffAccountRequest request)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var status = await _Staff.RegisterStaffAccount(request);
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
        public async Task<ActionResult> UpdateStaffAccount([FromForm] UpdateStaffAccountRequest request)
        {
            try
            {
                var status = await _Staff.UpdateStaffAccount(request);
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
        [HttpPut("ChangePasswordStaff")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordStaffRequest request)
        {
            try
            {
                var status = await _Staff.ChangePasswordStaff(request);
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
