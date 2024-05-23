using DiamondLuxurySolution.Application.Repository.Platform;
using DiamondLuxurySolution.Application.Repository.User.Customer;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly ICustomerRepo _customer;

        public CustomersController(LuxuryDiamondShopContext context, ICustomerRepo customer)
        {
            _context = context;
            _customer = customer;
        }


        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<ActionResult> DeleteCustomer([FromBody] Guid CustomerId)
        {
            try
            {
                var status = await _customer.DeleteCustomer(CustomerId);
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

        [HttpGet("GetCustomerById")]
        public async Task<ActionResult> GetCustomerById([FromQuery] Guid CustomerId)
        {
            try
            {
                var status = await _customer.GetCustomerById(CustomerId);
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
        public async Task<ActionResult> ViewAllCustomer([FromQuery] ViewCustomerPaginationRequest request)
        {
            try
            {
                var status = await _customer.ViewCustomerPagination(request);
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

        [HttpPost("LoginCustomer")]
        public async Task<ActionResult> LoginCustomer([FromBody] LoginCustomerRequest request)
        {
            try
            {
                var status = await _customer.Login(request);
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

        [HttpPost("RegisterCustomer")]
        public async Task<ActionResult> Register([FromBody] RegisterCustomerAccountRequest request)
        {
            try
            {
                var status = await _customer.Register(request);
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
        public async Task<ActionResult> UpdateCustomerAccount([FromBody] UpdateCustomerRequest request)
        {
            try
            {
                var status = await _customer.UpdateCustomerAccount(request);
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
        [HttpPut("ChangePasswordCustomer")]
        public async Task<ActionResult> ChangePassword([FromForm] ChangePasswordCustomerRequest request)
        {
            try
            {
                var status = await _customer.ChangePasswordCustomer(request);
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
