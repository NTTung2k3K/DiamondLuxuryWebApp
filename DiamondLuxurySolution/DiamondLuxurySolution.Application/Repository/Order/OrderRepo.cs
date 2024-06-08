using DiamondLuxurySolution.Application.Repository.Discount;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.Utilities.Constants;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Product;
using DiamondLuxurySolution.ViewModel.Models.Promotion;
using DiamondLuxurySolution.ViewModel.Models.Warranty;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Order
{
    public class OrderRepo : IOrderRepo
    {
        private readonly UserManager<AppUser> _userMananger;
        private readonly RoleManager<AppRole> _roleManager;

        private readonly LuxuryDiamondShopContext _context;
        public OrderRepo(LuxuryDiamondShopContext context, UserManager<AppUser> userMananger, RoleManager<AppRole> roleManager)
        {
            _userMananger = userMananger;
            _context = context;
            _roleManager = roleManager;
        }
        public async Task<ApiResult<bool>> ChangeStatusOrder(ChangeOrderStatusRequest request)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);
            if (order == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đơn hàng");
            }
            order.Status = request.Status;
            _context.SaveChanges();
            return new ApiSuccessResult<bool>("Success");
        }

        public async Task<ApiResult<bool>> ContinuePayment(ContinuePaymentRequest request)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);
            if (order == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đơn hàng");
            }
            order.RemainAmount = order.RemainAmount - request.PaidTheRest;
            if (order.RemainAmount < 0)
            {
                return new ApiSuccessResult<bool>(true, "Đã thanh toán thành công và bị dư " + Math.Abs(order.RemainAmount));
            }
            if (order.RemainAmount == 0)
            {
                return new ApiSuccessResult<bool>(true, "Success");
            }
            return new ApiSuccessResult<bool>("Success số tiền còn lại cần thanh toán là " + order.RemainAmount);
        }

        public async Task<ApiResult<bool>> CreateOrder(CreateOrderRequest request)
        {
            List<string> errorList = new List<string>();

            // Validate request
            if (string.IsNullOrEmpty(request.ShipName)) errorList.Add("Vui lòng nhập tên người nhận hàng");
            if (string.IsNullOrEmpty(request.ShipEmail)) errorList.Add("Vui lòng nhập email nhận hàng");
            if (string.IsNullOrWhiteSpace(request.ShipPhoneNumber))
            {
                errorList.Add("Vui lòng nhập số điện thoại");
            }
            else
            {
                if (!Regex.IsMatch(request.ShipPhoneNumber, "^(09|03|07|08|05)[0-9]{8,9}$")) errorList.Add("Số điện thoại không hợp lệ");
            }
            if (string.IsNullOrEmpty(request.ShipAdress)) errorList.Add("Vui lòng nhập địa chỉ nhận hàng");
            if (errorList.Any()) return new ApiErrorResult<bool>("Lỗi thông tin", errorList);
            if (request.ListOrderProduct == null) return new ApiErrorResult<bool>("Đơn hàng không có sản phẩm");
            if (request.ListPaymentId == null) return new ApiErrorResult<bool>("Đơn hàng chưa có phương thức thanh toán");
            if (request.CustomerId == Guid.Empty) return new ApiErrorResult<bool>("Đơn hàng chưa có người đặt");

            Random rd = new Random();
            string orderId = GenerateOrderId(rd);

            while (await _context.Orders.FindAsync(orderId) != null)
            {
                orderId = GenerateOrderId(rd);
            }

            var order = new DiamondLuxurySolution.Data.Entities.Order()
            {
                OrderId = orderId,
                ShipAdress = request.ShipAdress,
                ShipEmail = request.ShipEmail,
                ShipName = request.ShipName,
                ShipPhoneNumber = request.ShipPhoneNumber,
                CustomerId = request.CustomerId,
                Description = request.Description,
                Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.OrderStatus.InProgress.ToString(),
                OrderDate = DateTime.Now,
                Deposit = (decimal)request.Deposit,
            };

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Add and save the order first
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    decimal totalPrice = 0;
                    foreach (var orderProduct in request.ListOrderProduct)
                    {
                        var product = await _context.Products.FindAsync(orderProduct.ProductId);
                        if (product == null) return new ApiErrorResult<bool>("Không tìm thấy sản phẩm trong đơn đặt hàng");

                        var warranty = new DiamondLuxurySolution.Data.Entities.Warranty()
                        {
                            WarrantyId = Guid.NewGuid(),
                            DateActive = DateTime.Now,
                            DateExpired = DateTime.Now.AddMonths(12),
                            WarrantyName = $"Phiếu bảo hành cho sản phẩm {product.ProductName} | {product.ProductId}",
                            Description = "Phiếu bảo hành có giá trị trong vòng 12 tháng",
                            Status = true,
                        };

                        var orderDetail = new OrderDetail()
                        {
                            OrderId = orderId,
                            ProductId = orderProduct.ProductId,
                            Quantity = orderProduct.Quantity,
                            UnitPrice = product.SellingPrice,
                            TotalPrice = orderProduct.Quantity * product.SellingPrice,
                            WarrantyId = warranty.WarrantyId
                        };

                        totalPrice += orderDetail.TotalPrice;
                        _context.Warrantys.Add(warranty);
                        _context.OrderDetails.Add(orderDetail);
                    }

                    decimal total = await CalculateTotalPrice(request, totalPrice);
                    order.TotalAmout = total;

                    decimal maxDeposit = total * 0.1M;
                    if (request.Deposit < maxDeposit)
                    {
                        return new ApiErrorResult<bool>($"Số tiền đặt cọc phải lớn hơn hoặc bằng {maxDeposit}");
                    }
                    order.RemainAmount = total - (decimal)request.Deposit;

                    foreach (var paymentId in request.ListPaymentId)
                    {
                        var payment = await _context.Payments.FindAsync(paymentId);
                        if (payment == null) return new ApiErrorResult<bool>("Không tìm thấy phương thức thanh toán");

                        var orderPayment = new OrdersPayment()
                        {
                            OrderId = orderId,
                            Message = $"Thanh toán bằng phương thức {payment.PaymentMethod}",
                            PaymentTime = DateTime.Now,
                            Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Waiting.ToString(),
                            OrdersPaymentId = Guid.NewGuid(),
                            PaymentId = paymentId,
                            PaymentAmount = (decimal)order.RemainAmount > 0 ? (decimal)request.Deposit : total
                        };
                        _context.OrdersPayments.Add(orderPayment);
                    }

                    if (request.isShip)
                    {
                        order.ShipperId = await AssignShipper();
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ApiSuccessResult<bool>(true, "Success");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ApiErrorResult<bool>(ex.Message);
                }
            }
        }

        private string GenerateOrderId(Random rd)
        {
            return "DMLOD" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
        }

        private async Task<decimal> CalculateTotalPrice(CreateOrderRequest request, decimal totalPrice)
        {
            decimal total = totalPrice;
            if (request.PromotionId != null)
            {
                var promotion = await _context.Promotions.FindAsync(request.PromotionId);
                if (promotion != null && promotion.StartDate.Date <= DateTime.Now.Date && DateTime.Now.Date <= promotion.EndDate.Date)
                {
                    decimal discountPrice = (decimal)totalPrice * (decimal)(promotion.DiscountPercent / 100);
                    if (discountPrice > promotion.MaxDiscount)
                    {
                        discountPrice = (decimal)promotion.MaxDiscount;
                    }
                    total -= discountPrice;
                }
            }

            var user = await _userMananger.FindByIdAsync(request.CustomerId.ToString());
            if (user != null)
            {
                var userPoint = user.Point;
                var discounts = await _context.Discounts.ToListAsync();
                foreach (var discount in discounts)
                {
                    if (discount.From <= userPoint && userPoint <= discount.To)
                    {
                        total -= total * (decimal)discount.PercentSale / 100;
                        break;
                    }
                }
            }

            return total;
        }
        private async Task<decimal> CalculateTotalPriceByStaff(CreateOrderByStaffRequest request, decimal totalPrice,Guid cusId)
        {
            decimal total = totalPrice;
            if (request.PromotionId != null)
            {
                var promotion = await _context.Promotions.FindAsync(request.PromotionId);
                if (promotion != null && promotion.StartDate.Date <= DateTime.Now.Date && DateTime.Now.Date <= promotion.EndDate.Date)
                {
                    decimal discountPrice = (decimal)totalPrice * (decimal)(promotion.DiscountPercent / 100);
                    if (discountPrice > promotion.MaxDiscount)
                    {
                        discountPrice = (decimal)promotion.MaxDiscount;
                    }
                    total -= discountPrice;
                }
            }

            var user = await _userMananger.FindByIdAsync(cusId.ToString());
            if (user != null)
            {
                var userPoint = user.Point;
                var discounts = await _context.Discounts.ToListAsync();
                foreach (var discount in discounts)
                {
                    if (discount.From <= userPoint && userPoint <= discount.To)
                    {
                        total -= total * (decimal)discount.PercentSale / 100;
                        break;
                    }
                }
            }

            return total;
        }
        private async Task<Guid?> AssignShipper()
        {
            var shippers = await _userMananger.Users
                .Where(u => u.Status.Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.ShiperStatus.Waiting))
                .ToListAsync();

            if (shippers.Any())
            {
                int index = new Random().Next(shippers.Count);
                return shippers[index].Id;
            }

            return null;
        }


        public async Task<ApiResult<bool>> CreateOrderByStaff(CreateOrderByStaffRequest request)
        {
            List<string> errorList = new List<string>();

            // Validate request
            if (string.IsNullOrEmpty(request.Fullname)) errorList.Add("Vui lòng nhập tên người nhận hàng");
            if (string.IsNullOrEmpty(request.Email)) errorList.Add("Vui lòng nhập email nhận hàng");
            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                errorList.Add("Vui lòng nhập số điện thoại");
            }
            else
            {
                if (!Regex.IsMatch(request.PhoneNumber, "^(09|03|07|08|05)[0-9]{8,9}$")) errorList.Add("Số điện thoại không hợp lệ");
            }
            if (string.IsNullOrEmpty(request.ShipAdress)) errorList.Add("Vui lòng nhập địa chỉ nhận hàng");
            if (errorList.Any()) return new ApiErrorResult<bool>("Lỗi thông tin", errorList);
            if (request.ListOrderProduct == null) return new ApiErrorResult<bool>("Đơn hàng không có sản phẩm");
            if (request.ListPaymentId == null) return new ApiErrorResult<bool>("Đơn hàng chưa có phương thức thanh toán");
            if (request.StaffId == Guid.Empty) return new ApiErrorResult<bool>("Đơn hàng chưa có nhân viên tạo");

            Random rd = new Random();
            string orderId = GenerateOrderId(rd);

            while (await _context.Orders.FindAsync(orderId) != null)
            {
                orderId = GenerateOrderId(rd);
            }
            
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    Guid cusId = Guid.Empty;
                    var userCheck = await _userMananger.FindByEmailAsync(request.Email.ToString());
                    if (userCheck == null)
                    {
                        //
                        var user = new AppUser()
                        {
                            Id = Guid.NewGuid(),
                            Fullname = request.Fullname,
                            Email = request.Email,
                            PhoneNumber = request.PhoneNumber,
                            Address = request.ShipAdress,
                            UserName = "Kh" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9),
                            Point=0
                        };

                        user.Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.CustomerStatus.New.ToString();
                        var status = await _userMananger.CreateAsync(user, "Kh12345@");


                        var customerRole = new AppRole()
                        {
                            Name = DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.Customer,
                            Description = "Khách hàng mặc định"
                        };
                        var role = await _roleManager.FindByNameAsync(customerRole.Name);
                        if (role == null)
                        {
                            var statusCreateRole = await _roleManager.CreateAsync(customerRole);
                            if (!status.Succeeded)
                            {
                                return new ApiErrorResult<bool>("Lỗi hệ thống, không thể tạo role vui lòng thử lại");
                            }
                        }

                        var statusAddRole = await _userMananger.AddToRoleAsync(user, customerRole.Name);
                        if (!statusAddRole.Succeeded)
                        {
                            return new ApiErrorResult<bool>("Lỗi hệ thống, không thể thêm role vào tải khoản vui lòng thử lại");
                        }
                        cusId = user.Id;
                    }
                    else
                    {
                        cusId = userCheck.Id;
                    }


                    var order = new DiamondLuxurySolution.Data.Entities.Order()
                    {
                        OrderId = orderId,
                        ShipAdress = request.ShipAdress,
                        ShipEmail = request.Email,
                        ShipName = request.Fullname,
                        ShipPhoneNumber = request.PhoneNumber,
                        Description = request.Description,
                        Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.OrderStatus.InProgress.ToString(),
                        OrderDate = DateTime.Now,
                        Deposit = (decimal)request.Deposit,
                        CustomerId = cusId,
                        isShip = request.isShip
                    };



                    // Add and save the order first
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    decimal totalPrice = 0;
                    foreach (var orderProduct in request.ListOrderProduct)
                    {
                        var product = await _context.Products.FindAsync(orderProduct.ProductId);
                        if (product == null) return new ApiErrorResult<bool>("Không tìm thấy sản phẩm trong đơn đặt hàng");

                        var warranty = new DiamondLuxurySolution.Data.Entities.Warranty()
                        {
                            WarrantyId = Guid.NewGuid(),
                            DateActive = DateTime.Now,
                            DateExpired = DateTime.Now.AddMonths(12),
                            WarrantyName = $"Phiếu bảo hành cho sản phẩm {product.ProductName} | {product.ProductId}",
                            Description = "Phiếu bảo hành có giá trị trong vòng 12 tháng",
                            Status = true,
                        };

                        var orderDetail = new OrderDetail()
                        {
                            OrderId = orderId,
                            ProductId = orderProduct.ProductId,
                            Quantity = orderProduct.Quantity,
                            UnitPrice = product.SellingPrice,
                            TotalPrice = orderProduct.Quantity * product.SellingPrice,
                            WarrantyId = warranty.WarrantyId
                        };

                        totalPrice += orderDetail.TotalPrice;
                        _context.Warrantys.Add(warranty);
                        _context.OrderDetails.Add(orderDetail);
                    }

                    decimal total = await CalculateTotalPriceByStaff(request, totalPrice,cusId);
                    order.TotalAmout = total;

                    decimal maxDeposit = total * 0.1M;
                    if (request.Deposit < maxDeposit)
                    {
                        return new ApiErrorResult<bool>($"Số tiền đặt cọc phải lớn hơn hoặc bằng {maxDeposit}");
                    }
                    order.RemainAmount = total - (decimal)request.Deposit;

                    foreach (var paymentId in request.ListPaymentId)
                    {
                        var payment = await _context.Payments.FindAsync(paymentId);
                        if (payment == null) return new ApiErrorResult<bool>("Không tìm thấy phương thức thanh toán");

                        var orderPayment = new OrdersPayment()
                        {
                            OrderId = orderId,
                            Message = $"Thanh toán bằng phương thức {payment.PaymentMethod}",
                            PaymentTime = DateTime.Now,
                            Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Waiting.ToString(),
                            OrdersPaymentId = Guid.NewGuid(),
                            PaymentId = paymentId,
                            PaymentAmount = (decimal)order.RemainAmount > 0 ? (decimal)request.Deposit : total
                        };
                        _context.OrdersPayments.Add(orderPayment);
                    }

                    if (request.isShip)
                    {
                        order.ShipperId = await AssignShipper();
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ApiSuccessResult<bool>(true, "Success");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ApiErrorResult<bool>(ex.Message);
                }
            }
        }

        public async Task<ApiResult<bool>> DeleteOrder(string OrderId)
        {
            var order = await _context.Orders
                             .Include(o => o.OrderDetails)
                             .ThenInclude(od => od.Warranty)
                             .FirstOrDefaultAsync(o => o.OrderId == OrderId);

            if (order == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đơn hàng");
            }
            foreach (var item in order.OrderDetails)
            {
                var warranty = item.Warranty;
                _context.Warrantys.Remove(warranty);
            }


            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }


        public async Task<ApiResult<OrderVm>> GetOrderById(string OrderId)
        {
            var order = _context.Orders.Include(x => x.Customer)
                                              .Include(x => x.Discount).Include(x => x.Promotion)
                                              .Include(x => x.OrdersPayment).ThenInclude(x => x.Payment)
                                              .Include(x => x.Shipper)
                                              .Include(x => x.OrderDetails).ThenInclude(x => x.Warranty)
                                              .FirstOrDefault(x => x.OrderId == OrderId);

            if (order == null)
            {
                return new ApiErrorResult<OrderVm>("Không tìm thấy đơn hàng");
            }


            var orderVms = new OrderVm()
            {
                OrderId = OrderId,
                ShipAdress = order.ShipAdress,
                ShipEmail = order.ShipEmail,
                ShipPhoneNumber = order.ShipPhoneNumber,
                ShipName = order.ShipName,
                Status = order.Status,
                TotalAmount = order.TotalAmout,
                RemainAmount = order.RemainAmount,
            };
            if (order.Customer != null)
            {
                orderVms.CustomerVm = new ViewModel.Models.User.Customer.CustomerVm()
                {
                    CustomerId = order.Customer.Id,
                    FullName = order.Customer.Fullname,
                    Email = order.Customer.Email,
                    PhoneNumber = order.Customer.PhoneNumber,
                    Status = order.Customer.Status
                };
            }
            if (order.Shipper != null)
            {
                orderVms.ShiperVm = new ViewModel.Models.User.Staff.StaffVm()
                {
                    StaffId = order.Customer.Id,
                    FullName = order.Customer.Fullname,
                    Email = order.Customer.Email,
                    PhoneNumber = order.Customer.PhoneNumber,
                    Status = order.Customer.Status
                };
            }
            if (order.Discount != null)
            {
                orderVms.DiscountVm = new ViewModel.Models.Discount.DiscountVm()
                {
                    DiscountId = order.Discount.DiscountId,
                    Description = order.Discount.Description,
                    DiscountName = order.Discount.DiscountName,
                    PercentSale = order.Discount.PercentSale,
                    Status = order.Discount.Status
                };
            }

            if (order.OrdersPayment != null)
            {
                orderVms.OrdersPaymentVm = order.OrdersPayment.Select(op => new OrderPaymentSupportDTO()
                {
                    PaymentId = op.PaymentId,
                    PaymentMethod = op.Payment.PaymentMethod,
                    PaymentTime = op.PaymentTime,
                    Status = op.Status,
                    PaymentAmount = op.PaymentAmount,
                    Message = op.Message
                }).ToList();
            }
            if (order.OrderDetails != null)
            {
                List<OrderProductSupportVm> listOrderSupport = new List<OrderProductSupportVm>();
                foreach (var orderDetail in order.OrderDetails)
                {
                    var product = await _context.Products.FindAsync(orderDetail.ProductId);
                    var warranty = await _context.Warrantys.FindAsync(orderDetail.WarrantyId);
                    var warrantyVm = new WarrantyVm()
                    {
                        WarrantyId = warranty.WarrantyId,
                        Description = warranty.Description,
                        DateActive = warranty.DateActive,
                        DateExpired = warranty.DateExpired,
                        Status = warranty.Status,
                        WarrantyName = warranty.WarrantyName
                    };
                    var orderProductSupport = new OrderProductSupportVm()
                    {
                        ProductId = orderDetail.ProductId,
                        Quantity = orderDetail.Quantity,
                        ProductName = product.ProductName,
                        ProductThumbnail = product.ProductThumbnail,
                        Warranty = warrantyVm,
                        UnitPrice = orderDetail.UnitPrice,
                    };
                    listOrderSupport.Add(orderProductSupport);
                }

                orderVms.ListOrderProduct = listOrderSupport;
            }

            return new ApiSuccessResult<OrderVm>(orderVms);
        }

        public async Task<ApiResult<bool>> UpdateInfoOrder(UpdateOrderRequest request)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);
            if (order == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đơn hàng");
            }
            List<string> errorList = new List<string>();
            if (string.IsNullOrEmpty(request.ShipName))
            {
                errorList.Add("Vui lòng nhập tên người nhận hàng");
            }
            if (string.IsNullOrEmpty(request.ShipEmail))
            {
                errorList.Add("Vui lòng nhập email nhận hàng");
            }
            if (string.IsNullOrEmpty(request.ShipPhoneNumber))
            {
                errorList.Add("Vui lòng nhập số điện thoại người nhận hàng");
            }
            if (string.IsNullOrEmpty(request.ShipAdress))
            {
                errorList.Add("Vui lòng nhập địa chỉ nhận hàng");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Lỗi thông tin", errorList);
            }
            if (request.ListOrderProduct == null)
            {
                return new ApiErrorResult<bool>("Đơn hàng không có sản phẩm");
            }
            if (request.ListPaymentId == null)
            {
                return new ApiErrorResult<bool>("Đơn hàng chưa có phương thức thanh toán");
            }
            order.ShipAdress = request.ShipAdress;
            order.ShipPhoneNumber = request.ShipPhoneNumber;
            order.ShipEmail = request.ShipEmail;
            order.ShipName = request.ShipName;




            // Process OrderDetail
            decimal totalPrice = 0;
            if (request.ListOrderProduct != null)
            {
                var listOrderProduct = _context.OrderDetails.Where(x => x.OrderId == order.OrderId);

                // Collect all warranty IDs from the existing order details
                var warrantyIdsToRemove = listOrderProduct.Select(od => od.WarrantyId).ToList();

                // Remove the existing order details
                _context.OrderDetails.RemoveRange(listOrderProduct);

                // Remove old warranties
                var oldWarranties = _context.Warrantys.Where(w => warrantyIdsToRemove.Contains(w.WarrantyId)).ToList();
                _context.Warrantys.RemoveRange(oldWarranties);


                foreach (var orderProduct in request.ListOrderProduct)
                {
                    var product = await _context.Products.FindAsync(orderProduct.ProductId);
                    if (product == null)
                    {
                        return new ApiErrorResult<bool>("Không tìm thấy sản phẩm trong đơn đặt hàng");
                    }
                    // Process warranty


                    var warranty = new DiamondLuxurySolution.Data.Entities.Warranty()
                    {
                        WarrantyId = Guid.NewGuid(),
                        DateActive = DateTime.Now,
                        DateExpired = DateTime.Now.AddMonths(12),
                        WarrantyName = $"Phiếu bảo hành cho sản phẩm {product.ProductName} | {product.ProductId}  ",
                        Description = "Phiếu bảo hành có giá trị trong vòng 12 tháng",
                        Status = true,
                    };
                    var OrderDetail = new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        ProductId = orderProduct.ProductId,
                        Quantity = orderProduct.Quantity,
                        UnitPrice = product.SellingPrice,
                        TotalPrice = orderProduct.Quantity * product.SellingPrice,
                        WarrantyId = warranty.WarrantyId
                    };
                    totalPrice += OrderDetail.TotalPrice;
                    _context.Warrantys.Add(warranty);
                    _context.OrderDetails.Add(OrderDetail);
                }
            }
            else
            {
                var listOrderProduct = _context.OrderDetails.Where(x => x.OrderId == order.OrderId);

                // Collect all warranty IDs from the existing order details
                var warrantyIdsToRemove = listOrderProduct.Select(od => od.WarrantyId).ToList();

                // Remove the existing order details
                _context.OrderDetails.RemoveRange(listOrderProduct);

                // Remove old warranties
                var oldWarranties = _context.Warrantys.Where(w => warrantyIdsToRemove.Contains(w.WarrantyId)).ToList();
                _context.Warrantys.RemoveRange(oldWarranties);


            }

            // Process Discount

            bool isSale = false;

            // Process Discount
            decimal totalDiscount = 0;
            if (request.DiscountId != null)
            {
                var discount = await _context.Discounts.FindAsync(request.DiscountId);
                if (discount == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy mã giảm giá");
                }
                totalDiscount = ((decimal)totalPrice * (decimal)discount.PercentSale);
                isSale = true;
                order.DiscountId = request.DiscountId;
            }
            else
            {
                order.DiscountId = null;
            }
            // Process Sale
            decimal totalSales = 0;

            var promotion = await _context.Promotions.FindAsync(request.PromotionId);
            if (promotion == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy chương trình khuyến mãi");
            }
            decimal salesPrice = 0;
            if (promotion.StartDate.Date <= DateTime.Now.Date && DateTime.Now.Date <= promotion.EndDate.Date)
            {
                var discountPrice = ((decimal)totalPrice * (decimal)promotion.DiscountPercent);
                if (discountPrice >= promotion.MaxDiscount)
                {
                    discountPrice = (decimal)promotion.MaxDiscount;
                }
                isSale = true;
                salesPrice += discountPrice;
            }

            totalSales += salesPrice;

            decimal total = totalPrice - (totalSales + totalDiscount);

            order.Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.OrderStatus.InProgress.ToString();
            order.TotalAmout = total;
            order.OrderDate = DateTime.Now;
            // Process Deposit
            decimal maxDeposit = (decimal)order.TotalAmout * (decimal)0.1;
            if (request.Deposit < maxDeposit)
            {
                return new ApiErrorResult<bool>($"Số tiền đặt cọc phải lớn hơn hoặc bằng {maxDeposit}");
            }
            order.RemainAmount = (decimal)order.TotalAmout - (decimal)request.Deposit;

            // Process Payment 
            if (request.ListPaymentId != null)
            {
                var OrderPayment = _context.OrdersPayments.Where(x => x.OrderId == order.OrderId);
                _context.OrdersPayments.RemoveRange(OrderPayment);
            }


            foreach (var paymentId in request.ListPaymentId)
            {
                var payment = await _context.Payments.FindAsync(paymentId);
                if (payment == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy phương thức thanh toán");
                }
                var orderPayment = new OrdersPayment()
                {
                    OrderId = order.OrderId,
                    Message = $"Thanh toán bằng phương thức {payment.PaymentMethod}",
                    PaymentTime = DateTime.Now,
                    Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Waiting.ToString(),
                    OrdersPaymentId = Guid.NewGuid(),
                    PaymentId = paymentId,
                    PaymentAmount = order.RemainAmount > 0 ? order.RemainAmount : total
                };
                _context.OrdersPayments.Add(orderPayment);
            }
            order.Deposit = (decimal)request.Deposit;
            _context.Orders.Update(order);

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Cập nhật đơn hàng thành công");
        }

        public async Task<ApiResult<bool>> UpdateShipper(UpdateShipperRequest request)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);
            if (order == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đơn hàng");
            }
            order.ShipperId = request.ShipperId;
            await _context.SaveChangesAsync();
            return
                new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<OrderVm>>> ViewOrder(ViewOrderRequest request)
        {
            var listOrder = await _context.Orders.Include(x => x.Customer)
                                              .Include(x => x.Discount).Include(x => x.Promotion)
                                              .Include(x => x.OrdersPayment).ThenInclude(x => x.Payment)
                                              .Include(x => x.Shipper)
                                              .Include(x => x.OrderDetails).ThenInclude(x => x.Warranty).ToListAsync();

            if (request.Keyword != null)
            {
                listOrder = listOrder.Where(x => x.ShipAdress.Contains(request.Keyword) ||
                 x.ShipName.Contains(request.Keyword) ||
                 x.ShipEmail.Contains(request.Keyword) ||
                 x.ShipPhoneNumber.Contains(request.Keyword) ||
                 x.Customer.Fullname.Contains(request.Keyword) ||
                 x.OrderId.Contains(request.Keyword)).ToList();
            }
            listOrder = listOrder.OrderBy(x => x.ShipName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listOrder.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();
            var listOrderVm = new List<OrderVm>();

            foreach (var order in listPaging)
            {

                var orderVms = new OrderVm()
                {
                    OrderId = order.OrderId,
                    ShipAdress = order.ShipAdress,
                    ShipEmail = order.ShipEmail,
                    ShipPhoneNumber = order.ShipPhoneNumber,
                    ShipName = order.ShipName,
                    Status = order.Status,
                    TotalAmount = order.TotalAmout,
                    RemainAmount = order.RemainAmount,
                };
                if (order.Customer != null)
                {
                    orderVms.CustomerVm = new ViewModel.Models.User.Customer.CustomerVm()
                    {
                        CustomerId = order.Customer.Id,
                        FullName = order.Customer.Fullname,
                        Email = order.Customer.Email,
                        PhoneNumber = order.Customer.PhoneNumber,
                        Status = order.Customer.Status
                    };
                }
                if (order.Shipper != null)
                {
                    orderVms.ShiperVm = new ViewModel.Models.User.Staff.StaffVm()
                    {
                        StaffId = order.Customer.Id,
                        FullName = order.Customer.Fullname,
                        Email = order.Customer.Email,
                        PhoneNumber = order.Customer.PhoneNumber,
                        Status = order.Customer.Status
                    };
                }
                if (order.Discount != null)
                {
                    orderVms.DiscountVm = new ViewModel.Models.Discount.DiscountVm()
                    {
                        DiscountId = order.Discount.DiscountId,
                        Description = order.Discount.Description,
                        DiscountName = order.Discount.DiscountName,
                        PercentSale = order.Discount.PercentSale,
                        Status = order.Discount.Status
                    };
                }


                if (order.OrdersPayment != null)
                {
                    orderVms.OrdersPaymentVm = order.OrdersPayment.Select(op => new OrderPaymentSupportDTO()
                    {
                        PaymentId = op.PaymentId,
                        PaymentMethod = op.Payment.PaymentMethod,
                        PaymentTime = op.PaymentTime,
                        Status = op.Status,
                        PaymentAmount = op.PaymentAmount,
                        Message = op.Message
                    }).ToList();
                }
                if (order.OrderDetails != null)
                {
                    List<OrderProductSupportVm> listOrderSupport = new List<OrderProductSupportVm>();
                    foreach (var orderDetail in order.OrderDetails)
                    {
                        var product = await _context.Products.FindAsync(orderDetail.ProductId);
                        var warranty = await _context.Warrantys.FindAsync(orderDetail.WarrantyId);
                        var warrantyVm = new WarrantyVm()
                        {
                            WarrantyId = warranty.WarrantyId,
                            Description = warranty.Description,
                            DateActive = warranty.DateActive,
                            DateExpired = warranty.DateExpired,
                            Status = warranty.Status,
                            WarrantyName = warranty.WarrantyName
                        };
                        var orderProductSupport = new OrderProductSupportVm()
                        {
                            ProductId = orderDetail.ProductId,
                            Quantity = orderDetail.Quantity,
                            ProductName = product.ProductName,
                            ProductThumbnail = product.ProductThumbnail,
                            Warranty = warrantyVm,
                            UnitPrice = orderDetail.UnitPrice,
                        };
                        listOrderSupport.Add(orderProductSupport);
                    }

                    orderVms.ListOrderProduct = listOrderSupport;
                }
                listOrderVm.Add(orderVms);

            }

            var listResult = new PageResult<OrderVm>()
            {
                Items = listOrderVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listOrder.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<OrderVm>>(listResult, "Success");
        }


    }
}
