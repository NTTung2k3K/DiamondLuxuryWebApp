using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.InspectionCertificate
{
    public class CreateInspectionCertificateRequest
    {
        public string InspectionCertificateName { get; set; } = null!;

        public DateTime DateGrading { get; set; }

        public IFormFile Logo { get; set; } = null!;

        public bool Status { get; set; }
    }
}
