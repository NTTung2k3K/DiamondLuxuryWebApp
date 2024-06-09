using DiamondLuxurySolution.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;

namespace DiamondLuxurySolution.ViewModel.Models.Order
{
    public class UpdateOrderRequest
    {
        public string OrderId { get; set; } = null!;

        [DisplayName("Tên người đặt hàng")]
        [Required(ErrorMessage = "Đơn hàng cần có tên người đặt")]
        public string Fullname { get; set; } = null!;
        [DisplayName("Giao hàng")]
        public bool isShip { get; set; }
        [DisplayName("Mã giảm giá")]
        public string? DiscountId { get; set; }

        [DisplayName("Số điện thoại người đặt hàng")]
        [Required(ErrorMessage = "Đơn hàng cần có số điện thoại người đặt")]
        public string PhoneNumber { get; set; } = null!;

        [DisplayName("Email người đặt hàng")]
        [Required(ErrorMessage = "Đơn hàng cần có Email người đặt")]
        public string? Email { get; set; } = null!;

        [DisplayName("Địa chỉ giao hàng")]
        public string? ShipAdress { get; set; }
        [DisplayName("Mô tả")]
        public string? Description { get; set; }

        [DisplayName("Trạng thái đơn hàng")]
        public string Status { get; set; }
        [DisplayName("Số tiền trả trước")]
        public decimal? Deposit { get; set; }
        public string? ListOrderProductJson { get; set; }
        public Guid? StaffId { get; set; }
        public Guid? CustomerId { get; set; }
        public virtual ICollection<OrderProductSupport> ListOrderProduct { get; set; } = new List<OrderProductSupport>();
        public virtual ICollection<OrderProductSupport> ListExistOrderProduct { get; set; } = new List<OrderProductSupport>();
        public string? ListExistOrderProductJson { get; set; }

        public Guid? PromotionId { get; set; }

        public virtual ICollection<Guid> ListPaymentId { get; set; } = new List<Guid>();

    }
}

