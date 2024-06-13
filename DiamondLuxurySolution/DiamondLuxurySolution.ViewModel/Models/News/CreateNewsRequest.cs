using DiamondLuxurySolution.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.News
{
    public class CreateNewsRequest
    {
        [Required(ErrorMessage ="Tin tức cần phải có tên")]
        public string NewName { get; set; }
        [Required(ErrorMessage = "Tin tức cần phải có tiêu đề")]

        public string Title { get; set; }

        public IFormFile? Image { get; set; }

        public string? Description { get; set; }

        public bool Status { get; set; }

        public Guid? WriterId { get; set; }


    }
}
