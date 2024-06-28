using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.InspectionCertificate
{
    public class UpdateInspectionCertificateRequest
    {
        public string InspectionCertificateId { get; set; }

        [Required(ErrorMessage = "Cần tên của giấy chứng nhận")]
        public string InspectionCertificateName { get; set; }

        [Required(ErrorMessage = "Cần ngày khởi tạo của giấy chứng nhận")]
        public DateTime DateGrading { get; set; }

        public IFormFile? Logo { get; set; }

        public bool Status { get; set; }
    }
}
