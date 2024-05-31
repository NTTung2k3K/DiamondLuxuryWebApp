using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Payment
{
    public class UpdatePaymentRequest
    {   
        public Guid PaymentId { get; set; }

        [Required(ErrorMessage = "Cần nhập phương thức thanh toán")]
        public string PaymentMethod { get; set; } = null!;

        public string? Description { get; set; }

        public bool Status { get; set; }
    }
}
