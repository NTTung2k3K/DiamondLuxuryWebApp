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
        public string InspectionCertificateId { get; set; } = null!;

        [Required(ErrorMessage = "Cần tên của giấy chứng nhận")]
        public string InspectionCertificateName { get; set; }

        [Required(ErrorMessage = "Cần ngày khởi tạo của giấy chứng nhận")]
        public DateTime DateGrading { get; set; }

        [Required(ErrorMessage = "Cần hình của giấy chứng nhận")]
        public IFormFile Logo { get; set; }

        public bool Status { get; set; }
    }
}
