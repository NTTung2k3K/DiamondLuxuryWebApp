using DiamondLuxurySolution.Application.Repository.News;
using DiamondLuxurySolution.Application.Repository.Order;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Models;
using DiamondLuxurySolution.ViewModel.Models.News;
using DiamondLuxurySolution.ViewModel.Models.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IOrderRepo _order;

        public OrdersController(LuxuryDiamondShopContext context, IOrderRepo order)
        {
            _context = context;
            _order = order;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
               
                var status = await _order.CreateOrder(request);
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
        [HttpPut("ChangeStatus")]
        public async Task<ActionResult> ChangeStatus([FromBody]ChangeOrderStatusRequest request)
        {
            try
            {

                var status = await _order.ChangeStatusOrder(request);
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
        public async Task<ActionResult> Update([FromBody] UpdateOrderRequest request)
        {
            try
            {

                var status = await _order.UpdateInfoOrder(request);
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
        [HttpPut("PaidTheRest")]
        public async Task<ActionResult> PaidTheRest([FromBody] ContinuePaymentRequest request)
        {
            try
            {

                var status = await _order.ContinuePayment(request);
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
        public async Task<ActionResult> DeleteOrder([FromBody]string OrderId)
        {
            try
            {

                var status = await _order.DeleteOrder(OrderId);
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
        public async Task<ActionResult> GetById([FromQuery]string OrderId)
        {
            try
            {

                var status = await _order.GetOrderById(OrderId);
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
        public async Task<ActionResult> ViewOrder([FromQuery]ViewOrderRequest request)
        {
            try
            {

                var status = await _order.ViewOrder(request);
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
