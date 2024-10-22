﻿using DiamondLuxurySolution.Application.Repository.About;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Models.About;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutsController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IAboutRepo _about;

        public AboutsController(LuxuryDiamondShopContext context, IAboutRepo about)
        {
            _context = context;
            _about = about;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateAbout([FromBody] CreateAboutRequest request)
        {
            try
            {
                var status = await _about.CreateAbout(request);
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
        public async Task<ActionResult> UpdateAbout([FromBody] UpdateAboutRequest request)
        {
            try
            {
                var status = await _about.UpdateAbout(request);
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
        public async Task<IActionResult> DeleteAbout([FromQuery] DeleteAboutRequest request)
        {
            try
            {
                var status = await _about.DeleteAbout(request);
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
        public async Task<IActionResult> FindById([FromQuery] int AboutId)
        {
            try
            {
                var status = await _about.GetAboutById(AboutId);
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
        public async Task<IActionResult> ViewAllAboutPaginationInCustomer([FromQuery] ViewAboutRequest request)
        {
            try
            {
                var status = await _about.ViewAboutInCustomer(request);
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
        public async Task<IActionResult> ViewAllAboutPaginationInManager([FromQuery] ViewAboutRequest request)
        {
            try
            {
                var status = await _about.ViewAboutInManager(request);
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
                var status = await _about.GetAll();
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
