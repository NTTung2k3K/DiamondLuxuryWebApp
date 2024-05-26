using DiamondLuxurySolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.SubGem
{
    public class SubGemVm
    {
        public Guid SubGemId { get; set; }
        public string SubGemName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal SubGemPrice { get; set; }
        public bool Active { get; set; }
    }
}
