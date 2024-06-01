using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Discount
{
    public class DiscountVm
    {
        public string DiscountId { get; set; }

        [Required(ErrorMessage = "Cần nhập tên mã giảm giá")]
        public string DiscountName { get; set; }

        [Required(ErrorMessage = "Cần nhập mô tả mã giảm giá")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Cần nhập phần trăm chiết khấu")]
        public double PercentSale { get; set; }

        public bool Status { get; set; }
    }
}
