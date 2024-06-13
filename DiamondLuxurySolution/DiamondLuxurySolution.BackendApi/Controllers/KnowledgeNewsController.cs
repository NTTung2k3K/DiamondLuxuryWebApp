using DiamondLuxurySolution.Application.Repository.KnowledgeNews;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KnowledgeNewsController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IKnowledgeNewsRepo _knowledgeNews;

        public KnowledgeNewsController(LuxuryDiamondShopContext context, IKnowledgeNewsRepo knowledgeNews)
        {
            _context = context;
            _knowledgeNews = knowledgeNews;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateKnowledgeNews([FromForm] CreateKnowledgeNewsRequest request)
        {
            try
            {
                var status = await _knowledgeNews.CreateKnowledgeNews(request);
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
        public async Task<ActionResult> UpdateKnowledgeNews([FromForm] UpdateKnowledgeNewsRequest request)
        {
            try
            {
                var status = await _knowledgeNews.UpdateKnowledgeNews(request);
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
        public async Task<IActionResult> DeleteKnowledgeNew([FromQuery] DeleteKnowledgeNewsRequest request)
        {
            try
            {
                var status = await _knowledgeNews.DeleteKnowledgeNews(request);
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
        public async Task<IActionResult> FindById([FromQuery] int KnowledgeNewsId)
        {
            try
            {
                var status = await _knowledgeNews.GetKnowledgeNewsById(KnowledgeNewsId);
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


        [HttpGet("ViewKnowledgeNews")]
        public async Task<IActionResult> ViewAllKnowledgeNews([FromQuery] ViewKnowledgeNewsRequest request)
        {
            try
            {
                var status = await _knowledgeNews.ViewKnowledgeNews(request);
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
