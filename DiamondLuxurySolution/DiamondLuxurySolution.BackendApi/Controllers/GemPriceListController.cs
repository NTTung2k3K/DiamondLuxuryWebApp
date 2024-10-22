﻿using DiamondLuxurySolution.Application.Repository.GemPriceList;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
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
        public async Task<ActionResult> CreateGemPriceList([FromBody] CreateGemPriceListRequest request)
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
        public async Task<ActionResult> UpdateGemPriceList([FromBody] UpdateGemPriceListRequest request)
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
        public async Task<IActionResult> DeleteGemPriceList([FromQuery] DeleteGemPriceListRequest request)
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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var status = await _news.GetAll();
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


        [HttpGet("ViewGemPriceList")]
        public async Task<IActionResult> ViewAllGemPriceListInMangerPagination([FromQuery] ViewGemPriceListRequest request)
        {
            try
            {
                var status = await _news.ViewGemPriceList(request);
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
