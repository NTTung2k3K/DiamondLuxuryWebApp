using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.User.Customer
{
    public class CustomerVm
    {
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Nhân viên cần có tên")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Nhân viên cần có địa chỉ")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Nhân viên cần có số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Nhân viên cần có email")]
        [EmailAddress(ErrorMessage = "Địa chỉ email ko hợp lệ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Nhân viên cần có ngày sinh")]
        public DateTime Dob { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        public string Status { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        
        public List<string>? ListRoleName { get; set; }
    }
}
