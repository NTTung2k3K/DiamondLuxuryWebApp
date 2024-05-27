using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Payment
{
    public class PaymentVm
    {
        public Guid PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public string Description { get; set; }
    }
}
