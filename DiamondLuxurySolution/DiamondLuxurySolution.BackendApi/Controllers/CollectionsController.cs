﻿using DiamondLuxurySolution.Application.Repository.About;
using DiamondLuxurySolution.Application.Repository.Collection;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Collection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionsController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly ICollectionRepo _collection;

        public CollectionsController(LuxuryDiamondShopContext context, ICollectionRepo collection)
        {
            _context = context;
            _collection = collection;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateCollection([FromForm] CreateCollectionRequest request)
        {
            try
            {
                var status = await _collection.CreateCollection(request);
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
        public async Task<ActionResult> UpdateCollection([FromForm] UpdateCollectionRequest request)
        {
            try
            {
                var status = await _collection.UpdateCollection(request);
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
        public async Task<IActionResult> DeleteCollection([FromBody] DeleteCollectionRequest request)
        {
            try
            {
                var status = await _collection.DeleteCollection(request);
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
        public async Task<IActionResult> FindById([FromQuery] string CollectionId)
        {
            try
            {
                var status = await _collection.GetCollectionById(CollectionId);
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
        public async Task<IActionResult> ViewAllCollectionPaginationInCustomer([FromQuery] ViewCollectionRequest request)
        {
            try
            {
                var status = await _collection.ViewCollectionInCustomer(request);
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
        public async Task<IActionResult> ViewAllCollectionPaginationInManager([FromQuery] ViewCollectionRequest request)
        {
            try
            {
                var status = await _collection.ViewCollectionInManager(request);
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