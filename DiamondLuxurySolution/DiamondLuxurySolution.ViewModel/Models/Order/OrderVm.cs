using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Models.Discount;
using DiamondLuxurySolution.ViewModel.Models.Payment;
using DiamondLuxurySolution.ViewModel.Models.Product;
using DiamondLuxurySolution.ViewModel.Models.Promotion;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using DiamondLuxurySolution.ViewModel.Models.Warranty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Order
{
    public class OrderVm
    {
        public string OrderId { get; set; }
        public string ShipName { get; set; } = null!;

        public DiscountVm DiscountVm { get; set; }

        public string ShipPhoneNumber { get; set; } = null!;

        public string ShipEmail { get; set; } = null!;

        public string ShipAdress { get; set; }

        public string? Status { get; set; }
        public decimal RemainAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public CustomerVm CustomerVm { get; set; }
        public StaffVm ShiperVm { get; set; }
        public WarrantyVm WarrantyVm { get; set; }

        public virtual ICollection<OrderProductSupport> ListOrderProduct { get; set; } = new List<OrderProductSupport>();

        public virtual ICollection<PromotionVm>? ListPromotionVm { get; set; } = new List<PromotionVm>();

        public virtual ICollection<PaymentVm> ListPaymentVm { get; set; } = new List<PaymentVm>();
        public List<CampaignDetailSupportDTO> CampaignDetailsVm { get; set; }
        public List<OrderPaymentSupportDTO> OrdersPaymentVm { get; set; }
    }
}
