using DiamondLuxurySolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Order
{
    public class CreateOrderRequest
    {

        [DisplayName("Tên người đặt hàng")]
        [Required(ErrorMessage ="Đơn hàng cần có tên người đặt")]
        public string ShipName { get; set; } = null!;

        public string? DiscountId { get; set; }

        [DisplayName("Số điện thoại người đặt hàng")]
        [Required(ErrorMessage = "Đơn hàng cần có số điện thoại người đặt")]
        public string ShipPhoneNumber { get; set; } = null!;

        [DisplayName("Email người đặt hàng")]
        [Required(ErrorMessage = "Đơn hàng cần có email người đặt")]
        public string ShipEmail { get; set; } = null!;

        [DisplayName("Địa chỉ người đặt hàng")]
        [Required(ErrorMessage = "Đơn hàng cần có địa chỉ người đặt")]
        public string ShipAdress { get; set; }
        public bool isShip { get; set; }
        public string? Description { get; set; }

        [DisplayName("Trạng thái đơn hàng")]
        public string? Status { get; set; }
        [DisplayName("Số tiền trả trước")]
        public decimal? Deposit{ get; set; }

        public Guid? StaffId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? PromotionId { get; set; }
        public virtual ICollection<OrderProductSupport> ListOrderProduct { get; set; } = new List<OrderProductSupport>();
        public string? ListOrderProductJson { get; set; }

        public virtual ICollection<Guid> ListPaymentId { get; set; } = new List<Guid>();

    }
}
