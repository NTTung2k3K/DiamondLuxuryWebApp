using DiamondLuxurySolution.Application.Repository.News;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.News;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly INewsRepo _news;

        public NewsController(LuxuryDiamondShopContext context, INewsRepo news)
        {
            _context = context;
            _news = news;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateNews([FromForm] CreateNewsRequest request)
        {
            try
            {
                var status = await _news.CreateNews(request);
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
        public async Task<ActionResult> UpdateNews([FromForm] UpdateNewsRequest request)
        {
            try
            {
                var status = await _news.UpdateNews(request);
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
        public async Task<IActionResult> DeleteNews([FromQuery] DeleteNewsRequest request)
        {
            try
            {
                var status = await _news.DeleteNews(request);
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
        public async Task<IActionResult> FindById([FromQuery] int NewsId)
        {
            try
            {
                var status = await _news.GetNewsById(NewsId);
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
        public async Task<IActionResult> ViewAlNewsPagination([FromQuery] ViewNewsRequest request)
        {
            try
            {
                var status = await _news.ViewNews(request);
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


