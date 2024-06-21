using DiamondLuxurySolution.Application.Repository.WarrantyDetail;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.WarrantyDetail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarrantyDetailsController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IWarrantyDetailRepo _warranty;

        public WarrantyDetailsController(LuxuryDiamondShopContext context, IWarrantyDetailRepo warranty)
        {
            _context = context;
            _warranty = warranty;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateWarranty([FromForm] CreateWarrantyDetailRequest request)
        {
            try
            {
                var status = await _warranty.CreateWarrantyDetail(request);
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
        public async Task<ActionResult> UpdateWarranty([FromForm] UpdateWarrantyDetailRequest request)
        {
            try
            {
                var status = await _warranty.UpdateWarrantyDetail(request);
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
        public async Task<IActionResult> DeleteWarranty([FromQuery] DeleteWarrantyDetailRequest request)
        {
            try
            {
                var status = await _warranty.DeleteWarrantyDetail(request);
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
        public async Task<IActionResult> FindById([FromQuery] int WarrantyDetailId)
        {
            try
            {
                var status = await _warranty.GetWarrantyDetaiById(WarrantyDetailId);
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
                var status = await _warranty.GetAll();
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
        public async Task<IActionResult> ViewAllWarrantyPaginationInCustomer([FromQuery] ViewWarrantyDetailRequest request)
        {
            try
            {
                var status = await _warranty.ViewWarrantyDetai(request);
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

        [HttpGet("ValidateWarrantyId")]
        public async Task<IActionResult> ViewAllWarrantyPaginationInManager([FromQuery] string warrantyId)
        {
            try
            {
                var status = await _warranty.CheckValidWarrantyId(warrantyId);
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
