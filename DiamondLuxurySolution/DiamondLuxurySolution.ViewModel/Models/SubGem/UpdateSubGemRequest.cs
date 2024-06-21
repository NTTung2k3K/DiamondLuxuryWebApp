using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.SubGem
{
    public class UpdateSubGemRequest
    {
        public Guid SubGemId { get; set; }

        [Required(ErrorMessage = "Cần nhập tên viên đá phụ")]
        public string SubGemName { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Cần nhập giá viên đá phụ")]
        public string? SubGemPrice { get; set; }
        public bool Active { get; set; }

    }
}
