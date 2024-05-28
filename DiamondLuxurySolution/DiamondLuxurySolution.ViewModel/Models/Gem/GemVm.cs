using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Gem
{
    public class GemVm
    {
        public Guid GemId { get; set; }

        public string? GemName { get; set; }

        public string? ProportionImage { get; set; }

        public string? Symetry { get; set; }

        public string? Polish { get; set; }

        public bool IsOrigin { get; set; }

        public string? GemImage { get; set; }

        public bool Fluoresence { get; set; }
        public DateTime AcquisitionDate { get; set; }

        public bool Active { get; set; }

        public InspectionCertificateVm InspectionCertificateVm { get; set; }

    }
}
