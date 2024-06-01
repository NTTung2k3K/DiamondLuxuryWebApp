using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Utilities.Constants
{
    public class Systemconstant
    {
        public class AppSettings
        {
            public const string BaseAddress = "Address:Base";
            public const int PAGE_SIZE = 10;
        }   
        public class UserRoleDefault
        {
            public const string Customer = "Khách hàng";
            public const string Manager = "Quản lý";
            public const string SalesStaff = "Nhân viên bán hàng";
            public const string DeliveryStaff = "Nhân viên giao hàng";
            public const string Admin = "Quản trị viên";
        }

        public enum OrderStatus
        {
            InProgress,
            Confirmed,
            Shipping,
            Success,
            Canceled
        }
        public enum Status
        {
            Inactive,
            Active
        }
        public enum ProductStatus
        {
            Selling,
            OutOfStock,
            Sales
        }
        public enum TransactionStatus
        {
            Success,
            Failed,
            Waiting
        }
        public enum StaffStatus
        {
            Active,         // Nhân viên đang hoạt động
            Inactive,       // Nhân viên không hoạt động
            OnLeave,        // Nhân viên đang nghỉ phép
            Suspended,      // Nhân viên bị tạm ngưng
            Terminated,     // Nhân viên đã bị chấm dứt hợp đồng
            Probation,      // Nhân viên đang trong thời gian thử việc
            Retired,         // Nhân viên đã nghỉ hưu
            ChangePasswordRequest
        }
        public enum ShiperStatus
        {
            Waiting,
            Working,
        }
        public enum CustomerStatus
        {
            Active,         // Khách hàng đang hoạt động
            Inactive,       // Khách hàng không hoạt động
            New,
            Suspended,      // Khách hàng bị tạm ngưng
            ChangePasswordRequest
        }

    }
}
