using DiamondLuxurySolution.Application.Repository.User.Staff;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
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


        [HttpDelete("DeleteStaff/{StaffId}")]
        public async Task<ActionResult> DeleteStaff([FromRoute] Guid StaffId)
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

        [HttpGet("ViewSalesStaff")]
        public async Task<ActionResult> ViewSalesStaff([FromQuery] ViewStaffPaginationCommonRequest request)
        {
            try
            {
                var status = await _Staff.ViewSalesStaffPagination(request);
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


        [HttpGet("ViewAdmin")]
        public async Task<ActionResult> ViewAdmin([FromQuery] ViewStaffPaginationCommonRequest request)
        {
            try
            {
                var status = await _Staff.ViewAdminPagination(request);
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

        [HttpGet("ViewCustomer")]
        public async Task<ActionResult> ViewCustomer([FromQuery] ViewStaffPaginationCommonRequest request)
        {
            try
            {
                var status = await _Staff.ViewCustomerPagination(request);
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
        [HttpGet("ViewDeliveryStaff")]
        public async Task<ActionResult> ViewDeliveryStaff([FromQuery] ViewStaffPaginationCommonRequest request)
        {
            try
            {
                var status = await _Staff.ViewDeliveryStaffPagination(request);
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

        [HttpPost("ForgotStaffPassword/SendCode/{Username}")]
        public async Task<ActionResult> ForgotpasswordStaffCode(string Username)
        {
            try
            {
                var status = await _Staff.ForgotpasswordStaffSendCode(Username);
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
        [HttpPost("ForgotCustomerPassword/ChangePassword")]
        public async Task<ActionResult> ForgotpasswordStaffChange([FromBody] ForgotPasswordStaffChangeRequest request)
        {
            try
            {
                var status = await _Staff.ForgotpassworStaffdChange(request);
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
        [HttpGet("ViewManagerStaff")]
        public async Task<ActionResult> ViewManager([FromQuery] ViewStaffPaginationCommonRequest request)
        {
            try
            {
                var status = await _Staff.ViewManagerPagination(request);
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

        [HttpPost("Update")]
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
