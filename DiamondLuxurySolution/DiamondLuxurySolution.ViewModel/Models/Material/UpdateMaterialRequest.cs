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
        public string? MaterialName { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Cần nhập màu")]
        public string? Color { get; set; }

        [Required(ErrorMessage = "Cần nhập hình nguyên liệu")]
        public IFormFile? MaterialImage { get; set; }

        public bool Status { get; set; }

        [Required(ErrorMessage = "Cần nhập ngày ảnh hưởng")]
<<<<<<< HEAD
        public string EffectDate { get; set; }
=======
        public DateTime? EffectDate { get; set; }
>>>>>>> ab1161713e5312992752fa39bc33406b42bf4661

        [Required(ErrorMessage = "Cần nhập giá")]
        public decimal? Price { get; set; }
    }
}
