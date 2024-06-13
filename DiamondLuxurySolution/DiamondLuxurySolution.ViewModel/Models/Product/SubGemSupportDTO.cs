using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models
{
    public class SubGemSupportDTO
    {
        public Guid SubGemId { get; set; }
        public string? SubGemName { get; set; }
        [Required(ErrorMessage ="Kim cương phụ cần có số lượng")]
        public int Quantity { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is SubGemSupportDTO other)
            {
                return SubGemId == other.SubGemId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (SubGemId).GetHashCode();
        }
    }
}
