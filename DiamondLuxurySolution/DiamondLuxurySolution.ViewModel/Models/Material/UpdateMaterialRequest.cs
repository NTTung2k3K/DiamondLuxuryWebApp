using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Material
{
    public class UpdateMaterialRequest
    {
        public Guid MaterialId { get; set; }

        [Required(ErrorMessage = "Cần nhập tên nguyên liệu")]
        public string MaterialName { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Cần nhập màu")]
        public string Color { get; set; }

        public IFormFile? MaterialImage { get; set; }

        public bool Status { get; set; }

        [Required(ErrorMessage = "Cần nhập ngày ảnh hưởng")]
        public string? EffectDate { get; set; }

        [Required(ErrorMessage = "Cần nhập giá")]
        public decimal? Price { get; set; }
    }
}
