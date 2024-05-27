using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Order
{
    public class CampaignDetailSupportDTO
    {
        public Guid PromotionId { get; set; }

        public string PromotionName { get; set; } = null!;

        public string? Description { get; set; }

        public string? PromotionImage { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool Status { get; set; }
        public decimal SalesPrice { get; set; }
    }
}
