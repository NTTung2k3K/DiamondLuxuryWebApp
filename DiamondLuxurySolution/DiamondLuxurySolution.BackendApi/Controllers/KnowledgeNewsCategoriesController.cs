using DiamondLuxurySolution.Application.Repository.KnowledgeNewCatagory;
using DiamondLuxurySolution.Application.Repository.Material;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNewsCategory;
using DiamondLuxurySolution.ViewModel.Models.Material;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KnowledgeNewsCategoriesController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IKnowledgeNewCatagoryRepo _knowledgeNewCatagory;

        public KnowledgeNewsCategoriesController(LuxuryDiamondShopContext context, IKnowledgeNewCatagoryRepo knowledgeNewCatagory)
        {
            _context = context;
            _knowledgeNewCatagory = knowledgeNewCatagory;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateKnowledgeNewCatagory([FromForm] CreateKnowledgeNewsCategoryRequest request)
        {
            try
            {
                var status = await _knowledgeNewCatagory.CreateKnowledgeNewsCategory(request);
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
        public async Task<ActionResult> UpdateKnowledgeNewCatagory([FromForm] UpdateKnowledgeNewsCategoryRequest request)
        {
            try
            {
                var status = await _knowledgeNewCatagory.UpdateKnowledgeNewsCategory(request);
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
        public async Task<IActionResult> DeleteKnowledgeNewCatagory([FromBody] DeleteKnowledgeNewsCategoryRequest request)
        {
            try
            {
                var status = await _knowledgeNewCatagory.DeleteKnowledgeNewsCategory(request);
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
        public async Task<IActionResult> FindById([FromQuery] int KnowledgeNewCatagoryId)
        {
            try
            {
                var status = await _knowledgeNewCatagory.GetKnowledgeNewsCategoryById(KnowledgeNewCatagoryId);
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
        public async Task<IActionResult> ViewAllKnowledgeNewCatagoryPagination([FromQuery] ViewKnowledgeNewsCategoryRequest request)
        {
            try
            {
                var status = await _knowledgeNewCatagory.ViewKnowledgeNewsCategory(request);
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
