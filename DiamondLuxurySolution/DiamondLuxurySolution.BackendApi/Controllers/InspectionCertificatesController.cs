using DiamondLuxurySolution.Application.Repository.About;
using DiamondLuxurySolution.Application.Repository.InspectionCertificate;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionCertificatesController : ControllerBase
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly IInspectionCertificateRepo _inspectionCertificate;

        public InspectionCertificatesController(LuxuryDiamondShopContext context, IInspectionCertificateRepo inspectionCertificate)
        {
            _context = context;
            _inspectionCertificate = inspectionCertificate;
        }



        [HttpPost("Create")]
        public async Task<ActionResult> CreateInspectionCertificate([FromForm] CreateInspectionCertificateRequest request)
        {
            try
            {
                var status = await _inspectionCertificate.CreateInspectionCertificate(request);
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
        public async Task<ActionResult> UpdateInspectionCertificate([FromForm] UpdateInspectionCertificateRequest request)
        {
            try
            {
                var status = await _inspectionCertificate.UpdateInspectionCertificate(request);
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
        public async Task<IActionResult> DeleteInspectionCertificate([FromQuery] DeleteInspectionCertificateRequest request)
        {
            try
            {
                var status = await _inspectionCertificate.DeleteInspectionCertificate(request);
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
        public async Task<IActionResult> FindById([FromQuery] string InspectionCertificateId)
        {
            try
            {
                var status = await _inspectionCertificate.GetInspectionCertificateById(InspectionCertificateId);
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
                var status = await _inspectionCertificate.GetAll();
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
        public async Task<IActionResult> ViewAllInspectionCertificatePaginationInCustomer([FromQuery] ViewInspectionCertificateRequest request)
        {
            try
            {
                var status = await _inspectionCertificate.ViewInspectionCertificateInCustomer(request);
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
        public async Task<IActionResult> ViewAllInspectionCertificatePaginationInManager([FromQuery] ViewInspectionCertificateRequest request)
        {
            try
            {
                var status = await _inspectionCertificate.ViewInspectionCertificateInManager(request);
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
