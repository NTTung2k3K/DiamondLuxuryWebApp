using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.Entities
{
    public class Frame
    {
        public string FrameId { get; set; }
        public string NameFrame { get; set; }
        public double Size { get; set; }
        public double Weight { get; set; }

        public Guid MaterialId { get; set; }
    }
}
