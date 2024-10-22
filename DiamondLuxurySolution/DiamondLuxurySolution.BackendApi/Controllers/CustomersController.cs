﻿using DiamondLuxurySolution.Application.Repository.Platform;
using DiamondLuxurySolution.Application.Repository.User.Customer;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
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


        [HttpDelete("DeleteCustomer")]
        public async Task<ActionResult> DeleteCustomer([FromQuery] Guid CustomerId)
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
        [HttpGet("GetCustomerByEmail")]
        public async Task<ActionResult> GetCustomerByEmail([FromQuery] string Email)
        {
            try
            {
                var status = await _customer.GetCustomerByEmail(Email);
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

        [HttpGet("CountAllCustomerInYear")]
        public async Task<ActionResult> CountAllCustomerInYear()
        {
            try
            {
                var status = await _customer.CountAllCustomerInYear();
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
        [HttpGet("CountAllCustomerInWeek")]
        public async Task<ActionResult> CountAllCustomerInWeek()
        {
            try
            {
                var status = await _customer.CountAllCustomerInWeek();
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
        [HttpPost("CheckRegister")]
        public async Task<ActionResult> CheckRegister([FromBody] RegisterCustomerAccountRequest request)
        {
            try
            {
                var status = await _customer.CheckRegister(request);
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

        [HttpPost("ForgotCustomerPassword/SendCode")]
        public async Task<ActionResult> ForgotpasswordCustomerCode([FromBody] string Email)
        {
            try
            {
                var status = await _customer.ForgotpasswordCustomerSendCode(Email);
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
        public async Task<ActionResult> ForgotpasswordCustomerChange([FromBody] ForgotPasswordCustomerChangeRequest request)
        {
            try
            {
                var status = await _customer.ForgotpasswordCustomerChange(request);
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
