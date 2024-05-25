using DiamondLuxurySolution.Application.Repository.GemPriceList;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.GemPriceList;
using DiamondLuxurySolution.ViewModel.Models.News;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GemPriceListController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IGemPriceListRepo _news;

        public GemPriceListController(LuxuryDiamondShopContext context, IGemPriceListRepo news)
        {
            _context = context;
            _news = news;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateGemPriceList([FromForm] CreateGemPriceListRequest request)
        {
            try
            {
                var status = await _news.CreateGemPriceList(request);
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
        public async Task<ActionResult> UpdateGemPriceList([FromForm] UpdateGemPriceListRequest request)
        {
            try
            {
                var status = await _news.UpdateGemPriceList(request);
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
        public async Task<IActionResult> DeleteGemPriceList([FromBody] DeleteGemPriceListRequest request)
        {
            try
            {
                var status = await _news.DeleteGemPriceList(request);
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
        public async Task<IActionResult> FindById([FromQuery] int GemPriceListId)
        {
            try
            {
                var status = await _news.GetGemPriceListById(GemPriceListId);
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
        public async Task<IActionResult> ViewAllGemPriceListInCustomerPagination([FromQuery] ViewGemPriceListRequest request)
        {
            try
            {
                var status = await _news.ViewGemPriceListInCustomer(request);
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
        public async Task<IActionResult> ViewAllGemPriceListInMangerPagination([FromQuery] ViewGemPriceListRequest request)
        {
            try
            {
                var status = await _news.ViewGemPriceListInManager(request);
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
