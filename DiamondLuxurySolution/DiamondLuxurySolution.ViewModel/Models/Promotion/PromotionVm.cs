using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Promotion
{
    public class PromotionVm
    {
        public Guid PromotionId { get; set; }

        public string PromotionName { get; set; } = null!;

        public string? Description { get; set; }

        public string? PromotionImage { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool Status { get; set; }
    }
}
