﻿using DiamondLuxurySolution.Application.Repository.Payment;
using DiamondLuxurySolution.Application.Repository.Platform;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.Payment;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IPaymentRepo _payment;

        public PaymentsController(LuxuryDiamondShopContext context, IPaymentRepo payment)
        {
            _context = context;
            _payment = payment;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            try
            {
                var status = await _payment.CreatePayment(request);
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
        public async Task<ActionResult> UpdatePayment([FromBody] UpdatePaymentRequest request)
        {
            try
            {
                var status = await _payment.UpdatePayment(request);
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
        public async Task<IActionResult> DeletePayment([FromQuery] DeletePaymentRequest request)
        {
            try
            {
                var status = await _payment.DeletePayment(request);
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
        public async Task<IActionResult> FindPaymentById([FromQuery] Guid PaymentId)
        {
            try
            {
                var status = await _payment.GetPaymentById(PaymentId);
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
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var status = await _payment.GetAll();
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

        [HttpGet("ViewInPayment")]
        public async Task<IActionResult> ViewAllPaymentPagination([FromQuery] ViewPaymentRequest request)
        {
            try
            {
                var status = await _payment.ViewPayment(request);
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
