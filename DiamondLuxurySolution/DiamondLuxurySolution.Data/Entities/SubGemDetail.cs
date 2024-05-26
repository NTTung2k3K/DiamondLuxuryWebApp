using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.Entities
{
    public class SubGemDetail
    {

        public string ProductId { get; set; } = null!;
        public Guid SubGemId {  get; set; } 
        public int Quantity { get; set; } 
        public Product Product { get; set; } = null!;
        public SubGem SubGem { get; set; } = null!;
    }
}
