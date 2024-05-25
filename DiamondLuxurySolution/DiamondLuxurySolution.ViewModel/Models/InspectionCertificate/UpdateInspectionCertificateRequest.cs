using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.InspectionCertificate
{
    public class UpdateInspectionCertificateRequest
    {
        public string InspectionCertificateId { get; set; } = null!;

        public string InspectionCertificateName { get; set; } = null!;

        public DateTime? DateGrading { get; set; }

        public IFormFile? Logo { get; set; }

        public bool Status { get; set; }
    }
}
