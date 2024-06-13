using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Frame
{
    public class UpdateFrameRequest
    {
        public string? FrameId { get; set; }
        [Required(ErrorMessage = "Cần nhập tên khung")]
        public string? NameFrame { get; set; }
        [Required(ErrorMessage = "Cần nhập size khung")]
        public string? Size { get; set; }
        [Required(ErrorMessage = "Cần nhập trọng lượng khung")]
        public string? Weight { get; set; }
        [Required(ErrorMessage = "Cần chọn tên vật liệu")]
        public Guid MaterialId { get; set; }
    }
}
