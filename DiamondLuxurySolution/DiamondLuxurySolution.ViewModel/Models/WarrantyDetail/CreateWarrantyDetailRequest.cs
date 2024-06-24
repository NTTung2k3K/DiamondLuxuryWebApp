using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.WarrantyDetail
{
    public class CreateWarrantyDetailRequest
    {
        [DisplayName("Mã bảo hành")]
        [Required(ErrorMessage = "Vui lòng nhập mã bảo hành")]
        public string WarrantyId { get; set; }
        [DisplayName("Tên bảo hành")]
        [Required(ErrorMessage = "Vui lòng nhập tên bảo hành")]
        public string WarrantyDetailName { get; set; }
        [DisplayName("Loại bảo hành")]
        public string WarrantyType { get; set; }
        [DisplayName("Ngày nhận sản phẩm")]
        [Required(ErrorMessage = "Vui lòng nhập ngày nhận sản phẩm")]
        public DateTime? ReceiveProductDate { get; set; }
        [DisplayName("Ngày giao lại sản phẩm")]
        public DateTime? ReturnProductDate { get; set; }
        [DisplayName("Mô tả thông tin bảo hành")]
        public string? Description { get; set; }
        [DisplayName("Trạng thái")]
        public string Status { get; set; }
        [DisplayName("Hình ảnh")]
        public IFormFile? Image { get; set; }
    }
}
