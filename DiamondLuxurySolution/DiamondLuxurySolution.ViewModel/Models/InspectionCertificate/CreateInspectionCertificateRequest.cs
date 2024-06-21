using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.InspectionCertificate
{
    public class CreateInspectionCertificateRequest
    {
        [Required(ErrorMessage = "Cần nhập tên giấy chứng nhận")]
        public string InspectionCertificateName { get; set; }

        [Required(ErrorMessage = "Cần nhập hình ảnh giấy chứng nhận")]
        public IFormFile? Logo { get; set; }

        public bool Status { get; set; }
    }
}
