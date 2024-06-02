using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.GemPriceList
{
    public class CreateGemPriceListRequest
    {
        [Required(ErrorMessage = "Cần nhập giát cắt")]
        public string? Cut { get; set; }
        [Required(ErrorMessage = "Cần nhập độ tinh khiết")]
        public string? Clarity { get; set; }
        [Required(ErrorMessage = "Cần nhập trọng lượng")]
        public string? CaratWeight { get; set; }
        [Required(ErrorMessage = "Cần nhập màu")]
        public string? Color { get; set; }
        [Required(ErrorMessage = "Cần nhập giá")]
        public string? Price { get; set; }
        [Required(ErrorMessage = "Cần nhập tên kim cương")]
        public Guid GemId { get; set; }

        public bool Active { get; set; }
        [Required(ErrorMessage = "Cần nhập ngày ảnh hưởng")]
        public DateTime? effectDate { get; set; }


    }
}
