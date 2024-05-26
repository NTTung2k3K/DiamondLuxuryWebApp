using DiamondLuxurySolution.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.News
{
    public class CreateNewsRequest
    {
        public string? NewName { get; set; }

        public string? Title { get; set; }

        public IFormFile? Image { get; set; }

        public string? Description { get; set; }

        public bool? IsOutstanding { get; set; }

        public Guid? WriterId { get; set; }


    }
}
