using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Data.Entities
{
    public partial class AppUser : IdentityUser<Guid>
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public DateTime? Dob { get; set; }
        public int? Point { get; set; }
        public string? Status {  get; set; }
        public string? Fullname { get; set; }
        public string? CitizenIDCard { get; set; }
        public string? Image {  get; set; }
        public string? Address { get; set; }
        public string? ShipStatus { get; set; }

        public DateTime LastChangePasswordTime { get; set; }

        public List<Order> Orders { get; set; }
        public List<KnowledgeNews> KnowledgeNews { get; set; }
        public List<News> News { get; set; }
    }
}
