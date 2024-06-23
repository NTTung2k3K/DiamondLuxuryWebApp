using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.About
{
    public class UpdateAboutRequest
    {
        public int AboutId { get; set; }
        [Required(ErrorMessage = "Cần Thêm Thông Tin Liên Hệ")]
        [DisplayName("Tên chính")]
        public string AboutName { get; set; }

        [DisplayName("Email của cửa hàng")]
        public string? AboutEmail { get; set; }
        [DisplayName("Địa chỉ của cửa hàng")]
        public string? AboutAddress { get; set; }
        [DisplayName("Số điện thoại của cửa hàng")]
        public string? AboutPhoneNumber { get; set; }
        [DisplayName("Mô tả của cửa hàng")]
        public string? Description { get; set; }
        [DisplayName("Trạng thái")]
        public bool Status { get; set; }
    }
}
