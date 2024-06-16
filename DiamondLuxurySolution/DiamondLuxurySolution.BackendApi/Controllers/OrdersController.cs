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
        [HttpPost("CreateOrderByStaff")]
        public async Task<ActionResult> CreateOrderByStaff([FromBody] CreateOrderByStaffRequest request)
        {
            try
            {

                var status = await _order.CreateOrderByStaff(request);
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
                if (!string.IsNullOrEmpty(request.StatusOrderPaymentJson))
                {
                    request.StatusOrderPayment = (List<OrderStatusSupportDTO>?)JsonConvert.DeserializeObject<ICollection<OrderStatusSupportDTO>>(request.StatusOrderPaymentJson);
                }
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
        [HttpPut("UpdateShipper")]
        public async Task<ActionResult> UpdateShipper([FromBody] UpdateShipperRequest request)
        {
            try
            {

                var status = await _order.UpdateShipper(request);
                if (status.IsSuccessed)
                {
                    return Ok(status);
                }
                return Ok(status);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteOrder([FromQuery]string OrderId)
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
        [HttpGet("GetFullOrderByCustomerId")]
        public async Task<ActionResult> GetFullOrderByCustomerId([FromQuery]ViewOrderRequest request)
        {
            try
            {

                var status = await _order.GetListOrderOfCustomer(request);
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

        [HttpGet("GetTotalIncome")]
        public async Task<ActionResult> GetTotalIncome()
        {
            try
            {

                var status = await _order.TotalIncome();
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
        [HttpGet("GetTotalOrder")]
        public async Task<ActionResult> GetTotalOrder()
        {
            try
            {

                var status = await _order.TotalOrder();
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
        [HttpGet("GetIncomeToday")]
        public async Task<ActionResult> GetIncomeToday()
        {
            try
            {

                var status = await _order.IncomeToday();
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
        [HttpGet("GetAllOrderToday")]
        public async Task<ActionResult> GetAllOrderToday()
        {
            try
            {

                var status = await _order.AllOrderToday();
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
        [HttpGet("GetIncomeAYear")]
        public async Task<ActionResult> GetIncomeAYear()
        {
            try
            {

                var status = await _order.IncomeAYear();
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
        [HttpGet("GetRecentTransaction")]
        public async Task<ActionResult> GetRecentTransaction()
        {
            try
            {

                var status = await _order.RecentTransaction();
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
        [HttpGet("GetRecentSuccessTransaction")]
        public async Task<ActionResult> GetRecentSuccessTransaction()
        {
            try
            {

                var status = await _order.RecentSuccessTransaction();
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
        [HttpGet("GetRecentWaitTransaction")]
        public async Task<ActionResult> GetRecentWaitTransaction()
        {
            try
            {

                var status = await _order.RecentWaitTransaction();
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
        [HttpGet("GetRecentFailTransaction")]
        public async Task<ActionResult> GetRecentFailTransaction()
        {
            try
            {

                var status = await _order.RecentFailTransaction();
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
        [HttpGet("GetOrderByQuarter")]
        public async Task<ActionResult> GetOrderByQuarter()
        {
            try
            {

                var status = await _order.OrderByQuarter();
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
