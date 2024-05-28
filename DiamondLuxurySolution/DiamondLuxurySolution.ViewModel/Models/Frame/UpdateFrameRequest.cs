using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Frame
{
    public class UpdateFrameRequest
    {
        public string? FrameId { get; set; }
        public string? NameFrame { get; set; }
        public string? Size { get; set; }
        public string? Weight { get; set; }
        public Guid MaterialId { get; set; }
    }
}
