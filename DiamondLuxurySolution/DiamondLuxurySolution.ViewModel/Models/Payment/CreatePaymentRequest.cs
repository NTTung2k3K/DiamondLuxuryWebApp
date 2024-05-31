using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Payment
{
    public class CreatePaymentRequest
    {
        [Required(ErrorMessage = "Cần nhập phuơng thức thanh toán")]
        public string PaymentMethod { get; set; } = null!;
        public string? Description { get; set; }

        public bool Status { get; set; }

    }
}
