using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.User.Staff
{
    public class UpdateStaffAccountRequest
    {
        public Guid StaffId { get; set; }
        [Required(ErrorMessage = "Nhân viên cần có tài khoản")]

        public string Username { get; set; }

        [Required(ErrorMessage = "Nhân viên cần có tên")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Nhân viên cần có địa chỉ")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Nhân viên cần có số điện thoại")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Nhân viên cần có số căn cước công nhân")]
        public string? CitizenIDCard { get; set; }
        [Required(ErrorMessage = "Nhân viên cần có email")]
        [EmailAddress(ErrorMessage = "Địa chỉ email ko hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Nhân viên cần có ngày sinh")]
        public DateTime Dob { get; set; }
        public IFormFile? Image { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        public string Status { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
       
        public List<Guid>? RoleId { get; set; }
        public List<string>? ListRoleName { get; set; }



    }
}
