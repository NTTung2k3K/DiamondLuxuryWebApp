using DiamondLuxurySolution.Application.Repository.About;
using DiamondLuxurySolution.Application.Repository.Material;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Material;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IMaterialRepo _material;

        public MaterialsController(LuxuryDiamondShopContext context, IMaterialRepo material)
        {
            _context = context;
            _material = material;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateMaterial([FromForm] CreateMaterialRequest request)
        {
            try
            {
                var status = await _material.CreateMaterial(request);
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
        public async Task<ActionResult> UpdateMaterial([FromForm] UpdateMaterialRequest request)
        {
            try
            {
                var status = await _material.UpdateMaterial(request);
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
        public async Task<IActionResult> DeleteMaterial([FromQuery] DeleteMaterialRequest request)
        {
            try
            {
                var status = await _material.DeleteMaterial(request);
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
        public async Task<IActionResult> FindById([FromQuery] Guid MaterialId)
        {
            try
            {
                var status = await _material.GetMaterialById(MaterialId);
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
        public async Task<IActionResult> ViewAllMaterialPaginationInCustomer([FromQuery] ViewMaterialRequest request)
        {
            try
            {
                var status = await _material.ViewMaterialInCustomer(request);
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
        public async Task<IActionResult> ViewAllMaterialPaginationInManager([FromQuery] ViewMaterialRequest request)
        {
            try
            {
                var status = await _material.ViewMaterialInManager(request);
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
