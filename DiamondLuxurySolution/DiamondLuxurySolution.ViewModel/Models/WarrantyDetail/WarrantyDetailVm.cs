using DiamondLuxurySolution.ViewModel.Models.Product;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using DiamondLuxurySolution.ViewModel.Models.Warranty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.WarrantyDetail
{
    public class WarrantyDetailVm
    {
        public int WarrantyDetailId { get; set; }
        public string WarrantyDetailName { get; set; }
        public string WarrantyType { get; set; }
        public DateTime ReceiveProductDate { get; set; }
        public DateTime? ReturnProductDate { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public string? Image { get; set; }
        public WarrantyVm WarrantyVm { get; set; }
        public ProductVm ProductVm { get; set; }
        public CustomerVm CustomerVm { get; set; }
    }
}
