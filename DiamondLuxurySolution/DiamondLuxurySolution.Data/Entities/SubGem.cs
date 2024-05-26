using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.Entities
{
    public class SubGem
    {
        public Guid SubGemId { get; set; }
        public string SubGemName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal SubGemPrice { get; set; }
        public virtual ICollection<SubGemDetail> SubGemDetails { get; set; } = new List<SubGemDetail>();

    }
}
