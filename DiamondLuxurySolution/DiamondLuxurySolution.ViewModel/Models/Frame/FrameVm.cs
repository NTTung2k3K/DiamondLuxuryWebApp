using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DiamondLuxurySolution.ViewModel.Models.Frame
{
    public class FrameVm
    {
        public string FrameId { get; set; }
        public string NameFrame { get; set; }
        public double Size {  get; set; }
        public double Weight {  get; set; }

        public virtual Data.Entities.Material Material { get; set; }

    }
}
