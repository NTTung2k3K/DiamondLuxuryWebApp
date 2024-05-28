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
        public double Weight { get; set; }
        public string FrameName { get; set; }
        public int Size { get; set; }

        public Guid MaterialId { get; set; }
        public Material Material { get; set; }
        public List<Product> Products { get; set; }

    }
}
