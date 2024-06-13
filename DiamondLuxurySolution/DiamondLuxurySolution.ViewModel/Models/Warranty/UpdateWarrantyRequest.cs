using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Warranty
{
    public class UpdateWarrantyRequest
    {
        public Guid WarrantyId { get; set; }

		[Required(ErrorMessage = "Cần thêm tên phiếu bảo hành")]
		public string WarrantyName { get; set; }

		public string? Description { get; set; }

		[Required(ErrorMessage = "Cần thêm ngày bắt đầu")]
		public DateTime? DateActive { get; set; }

		[Required(ErrorMessage = "Cần thêm ngày hết hạn")]
		public DateTime? DateExpired { get; set; }

		public bool Status { get; set; }
    }
}
