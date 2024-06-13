using DiamondLuxurySolution.Application.Repository.Warranty;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Models.Warranty;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarrantysController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IWarrantyRepo _warranty;

        public WarrantysController(LuxuryDiamondShopContext context, IWarrantyRepo warranty)
        {
            _context = context;
            _warranty = warranty;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateWarranty([FromBody] CreateWarrantyRequest request)
        {
            try
            {
                var status = await _warranty.CreateWarranty(request);
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
        public async Task<ActionResult> UpdateWarranty([FromBody] UpdateWarrantyRequest request)
        {
            try
            {
                var status = await _warranty.UpdateWarranty(request);
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
        public async Task<IActionResult> DeleteWarranty([FromQuery] DeleteWarrantyRequest request)
        {
            try
            {
                var status = await _warranty.DeleteWarranty(request);
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
        public async Task<IActionResult> FindById([FromQuery] Guid WarrantyId)
        {
            try
            {
                var status = await _warranty.GetWarrantyById(WarrantyId);
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


		[HttpGet("ViewInCustomer")]
        public async Task<IActionResult> ViewAllWarrantyPaginationInCustomer([FromQuery] ViewWarrantyRequest request)
        {
            try
            {
                var status = await _warranty.ViewWarrantyInCustomer(request);
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

        [HttpGet("ViewInManager")]
        public async Task<IActionResult> ViewAllWarrantyPaginationInManager([FromQuery] ViewWarrantyRequest request)
        {
            try
            {
                var status = await _warranty.ViewWarrantyInManager(request);
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
