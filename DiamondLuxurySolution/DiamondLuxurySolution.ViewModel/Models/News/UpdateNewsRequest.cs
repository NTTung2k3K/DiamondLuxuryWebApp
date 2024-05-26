using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.News
{
    public class UpdateNewsRequest
    {
        public int NewsId { get; set; }

        public string NewName { get; set; } = null!;

        public string Title { get; set; } = null!;

        public IFormFile? Image { get; set; }

        public string? Description { get; set; }

        public bool IsOutstanding { get; set; }

        public Guid WriterId { get; set; } 
    }
}
