using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.News
{
    public class NewsVm
    {
        public int NewsId { get; set; }
        [Required(ErrorMessage ="Tin tức cần phải có tên")]
        public string NewName { get; set; }
        [Required(ErrorMessage = "Tin tức cần phải có tiêu đề")]
        public string Title { get; set; }

        public string? Image { get; set; }

        public string? Description { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public bool Status { get; set; }

        public virtual StaffVm? Writer { get; set; }
    }
}
