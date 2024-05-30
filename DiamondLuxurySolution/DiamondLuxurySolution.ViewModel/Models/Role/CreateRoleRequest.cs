using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Role
{
    public class CreateRoleRequest
    {
        [Required(ErrorMessage = "Yêu cầu tên của chức vụ")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yêu cầu mô tả của chức vụ")]
        public string Description { get; set; }
    }
}
