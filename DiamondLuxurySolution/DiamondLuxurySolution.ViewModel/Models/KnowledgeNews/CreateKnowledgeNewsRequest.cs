using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.KnowledgeNews
{
    public class CreateKnowledgeNewsRequest
    {
        [Required(ErrorMessage = "Cần Thêm Tên Kiến Thức Tin Tức")]
        public string? KnowledgeNewsName { get; set; }
        [Required(ErrorMessage = "Cần Thêm Hình")]
        public IFormFile? Thumnail { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }
        public string? Description { get; set; }

        public bool Active { get; set; }
        public Guid? WriterId { get; set; }
        [Required(ErrorMessage = "Cần Thêm Chủ Đề")]
        public int? KnowledgeNewCatagoryId { get; set; }
    }
}
