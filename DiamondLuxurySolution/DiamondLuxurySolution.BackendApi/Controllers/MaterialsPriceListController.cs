using DiamondLuxurySolution.Application.Repository.About;
using DiamondLuxurySolution.Application.Repository.Material;
using DiamondLuxurySolution.Application.Repository.MaterialPriceList;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Material;
using DiamondLuxurySolution.ViewModel.Models.MaterialPriceList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsPriceListController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IMaterialPriceListRepo _materialPL;

        public MaterialsPriceListController(LuxuryDiamondShopContext context, IMaterialPriceListRepo materialPL)
        {
            _context = context;
            _materialPL = materialPL;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateMaterialsPriceList([FromForm] CreateMaterialPriceListRequest request)
        {
            try
            {
                var status = await _materialPL.CreateMaterialPriceList(request);
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
        public async Task<ActionResult> UpdateMaterialsPriceList([FromForm] UpdateMaterialPriceListRequest request)
        {
            try
            {
                var status = await _materialPL.UpdateMaterialPriceList(request);
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
        public async Task<IActionResult> DeleteMaterialsPriceList([FromBody] DeleteMaterialPriceListRequest request)
        {
            try
            {
                var status = await _materialPL.DeleteMaterialPriceList(request);
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
        public async Task<IActionResult> FindMaterialsPriceListById([FromQuery] int MaterialPriceListId)
        {
            try
            {
                var status = await _materialPL.GetMaterialPriceListById(MaterialPriceListId);
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
        public async Task<IActionResult> ViewAllMaterialsPriceListPaginationInCustomer([FromQuery] ViewMaterialPriceListRequest request)
        {
            try
            {
                var status = await _materialPL.ViewMaterialPriceListInCustomer(request);
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
        public async Task<IActionResult> ViewAllMaterialsPriceListPaginationInManager([FromQuery] ViewMaterialPriceListRequest request)
        {
            try
            {
                var status = await _materialPL.ViewMaterialPriceListInManager(request);
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
