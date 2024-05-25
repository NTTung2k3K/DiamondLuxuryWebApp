using DiamondLuxurySolution.Application.Repository.Warehouse;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.Warehouse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IWarehouseRepo _warehouse;

        public WarehousesController(LuxuryDiamondShopContext context, IWarehouseRepo warehouse)
        {
            _context = context;
            _warehouse = warehouse;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateWarehouse([FromBody] CreateWarehouseRequest request)
        {
            try
            {
                var status = await _warehouse.CreateWarehouse(request);
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
        public async Task<ActionResult> UpdateWarehouse([FromBody] UpdateWarehouseRequest request)
        {
            try
            {
                var status = await _warehouse.UpdateWarehouse(request);
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
        public async Task<IActionResult> DeleteWarehouse([FromBody] DeleteWarehouseRequest request)
        {
            try
            {
                var status = await _warehouse.DeleteWarehouse(request);
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
        public async Task<IActionResult> FindById([FromQuery] int WarehouseId)
        {
            try
            {
                var status = await _warehouse.GetWarehouseById(WarehouseId);
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
        public async Task<IActionResult> ViewAllWarehousePagination([FromQuery] ViewWarehouseRequest request)
        {
            try
            {
                var status = await _warehouse.ViewWarehouse(request);
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
