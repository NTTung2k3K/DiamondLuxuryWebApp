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
            public const string FirebaseApiKey = "Firebase:ApiKey";
            public const string FirebaseBucket = "Firebase:Bucket";
            public const string FirebaseAuthEmail = "Firebase:AuthEmail";
            public const string FirebaseAuthPassword = "Firebase:AuthPassword";
            public const int PAGE_SIZE = 10;
        }
        public class UserRoleDefault
        {
            public const string Customer = "Khách hàng";
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
        public enum TransactionStatus
        {
            Success,
            Failed
        }
        public enum StaffStatus
        {
            Active,         // Nhân viên đang hoạt động
            Inactive,       // Nhân viên không hoạt động
            OnLeave,        // Nhân viên đang nghỉ phép
            Suspended,      // Nhân viên bị tạm ngưng
            Terminated,     // Nhân viên đã bị chấm dứt hợp đồng
            Probation,      // Nhân viên đang trong thời gian thử việc
            Retired         // Nhân viên đã nghỉ hưu
        }
        public enum CustomerStatus
        {
            Active,         // Khách hàng đang hoạt động
            Inactive,       // Khách hàng không hoạt động
            Pending,        // Khách hàng đang chờ xử lý
            New,
            Suspended,      // Khách hàng bị tạm ngưng
            Deleted,        // Khách hàng đã bị xóa
            Verified,       // Khách hàng đã xác thực
            Unverified      // Khách hàng chưa xác thực
        }

    }
}
