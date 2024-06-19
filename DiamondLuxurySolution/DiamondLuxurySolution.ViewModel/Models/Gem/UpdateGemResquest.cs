using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Gem
{
    public class UpdateGemResquest
    {
        public Guid GemId { get; set; }

        [Required(ErrorMessage = "Cần nhập tên kim cương")]
        public string GemName { get; set; }
        public IFormFile? ProportionImage { get; set; }
        [Required(ErrorMessage = "Cần nhập độ đối xứng")]
        public string Symetry { get; set; }
        [Required(ErrorMessage = "Cần nhập độ bóng")]
        public string Polish { get; set; }
        public bool IsOrigin { get; set; }
        public IFormFile? GemImage { get; set; }
        public bool Fluoresence { get; set; }
        [Required(ErrorMessage = "Cần nhập ngày nhận kim cương")]
        public DateTime? AcquisitionDate { get; set; }
        public bool Active { get; set; }

		[Required(ErrorMessage = "Cần chọn mã giá kim cương")]
		public int GemPriceListId { get; set; }
	}
}
