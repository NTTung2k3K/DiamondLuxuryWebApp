using DiamondLuxurySolution.Application.Repository.About;
using DiamondLuxurySolution.Application.Repository.Contact;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Contact;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IContactRepo _contact;

        public ContactController(LuxuryDiamondShopContext context, IContactRepo contact)
        {
            _context = context;
            _contact = contact;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateContact([FromBody] CreateContactRequest request)
        {
            try
            {
                var status = await _contact.CreateContact(request);
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
        public async Task<ActionResult> UpdateContact([FromBody] UpdateContactRequest request)
        {
            try
            {
                var status = await _contact.UpdateContact(request);
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
        [HttpGet("CountContactNotSolve")]
        public async Task<ActionResult> CountContactNotSolve()
        {
            try
            {
                var status = await _contact.CountContactNotSolve();
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
        public async Task<IActionResult> DeleteContact([FromQuery] DeleteContactRequest request)
        {
            try
            {
                var status = await _contact.DeleteContact(request);
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
        public async Task<IActionResult> FindContactById([FromQuery] int ContactId)
        {
            try
            {
                var status = await _contact.GetContactById(ContactId);
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
                var status = await _contact.GetAll();
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


        [HttpGet("ViewInContact")]
        public async Task<IActionResult> ViewAllContactPagination([FromQuery] ViewContactRequest request)
        {
            try
            {
                var status = await _contact.ViewContactInPaging(request);
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
