using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.KnowledgeNewsCategory
{
    public class CreateKnowledgeNewsCategoryRequest
    {
        [Required(ErrorMessage = "Cần Thêm Tên Loại Kiến Thức Tin Tức")]
        public string? KnowledgeNewCatagoriesName { get; set; }

        public string? Description { get; set; }
    }
}
