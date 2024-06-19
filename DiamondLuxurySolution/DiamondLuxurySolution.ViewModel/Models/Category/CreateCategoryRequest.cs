using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Category
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "Cần Thêm Tên Loại Sản Phẩm")]
        public string? CategoryName { get; set; }

        public string? CategoryType { get; set; }

        [Required(ErrorMessage = "Cần Thêm Hình Loại Sản Phẩm")]
        public IFormFile? CategoryImage { get; set; }

        public bool Status { get; set; }
    }
}
