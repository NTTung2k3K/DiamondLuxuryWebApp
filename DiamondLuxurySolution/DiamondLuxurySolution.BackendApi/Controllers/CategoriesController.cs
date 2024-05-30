using DiamondLuxurySolution.Application.Repository.About;
using DiamondLuxurySolution.Application.Repository.Category;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly ICategoryRepo _category;

        public CategoriesController(LuxuryDiamondShopContext context, ICategoryRepo category)
        {
            _context = context;
            _category = category;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateCategory([FromForm] CreateCategoryRequest request)
        {
            try
            {
                var status = await _category.CreateCategory(request);
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
        public async Task<ActionResult> UpdateCategory([FromForm] UpdateCategoryRequest request)
        {
            try
            {
                var status = await _category.UpdateCategory(request);
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
        public async Task<IActionResult> DeleteCateGory([FromQuery] DeleteCategoryRequest request)
        {
            try
            {
                var status = await _category.DeleteCategory(request);
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
        public async Task<IActionResult> FindById([FromQuery] int CategoryId)
        {
            try
            {
                var status = await _category.GetCategoryById(CategoryId);
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
        public async Task<IActionResult> ViewAllCategoryPaginationInCustomer([FromQuery] ViewCategoryRequest request)
        {
            try
            {
                var status = await _category.ViewCategoryInCustomer(request);
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
        public async Task<IActionResult> ViewAllCategoryPaginationInManager([FromQuery] ViewCategoryRequest request)
        {
            try
            {
                var status = await _category.ViewCategoryInManager(request);
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
