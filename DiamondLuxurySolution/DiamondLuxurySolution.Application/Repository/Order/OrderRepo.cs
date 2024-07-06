using Azure.Core;
using DiamondLuxurySolution.Application.Repository.Discount;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.Utilities.Constants;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models;
using DiamondLuxurySolution.ViewModel.Models.Order;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Product;
using DiamondLuxurySolution.ViewModel.Models.Promotion;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using DiamondLuxurySolution.ViewModel.Models.Warranty;
using Firebase.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static DiamondLuxurySolution.Application.Repository.Product.ProductRepo;
using System;
using System.IO;
using System.Threading.Tasks;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using HtmlToPdfMaster;
using Microsoft.VisualBasic.FileIO;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.Drawing.Printing;
using System.Reflection.Metadata;
using PdfSharp;
using PaperKind = DinkToPdf.PaperKind;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.Utilities.Helper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DiamondLuxurySolution.Application.Repository.Order
{
    public class OrderRepo : IOrderRepo
    {
        private readonly UserManager<AppUser> _userMananger;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConverter _converter;
        private readonly LuxuryDiamondShopContext _context;
        public OrderRepo(IConverter converter, LuxuryDiamondShopContext context, UserManager<AppUser> userMananger, RoleManager<AppRole> roleManager)
        {
            _userMananger = userMananger;
            _context = context;
            _roleManager = roleManager;
            _converter = converter;
        }
        public async Task<ApiResult<string>> ChangeStatusOrder(ChangeOrderStatusRequest request)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);
            if (order == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy đơn hàng");
            }



            var listOrderPayment = _context.OrdersPayments.Where(x => x.OrderId == request.OrderId).OrderByDescending(x => x.OpenPaymentTime).FirstOrDefault();
            if (listOrderPayment == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy thông tin thanh toán");
            }
            listOrderPayment.Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Success.ToString();
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<string>(order.OrderId, "Success");
        }

        public async Task<ApiResult<bool>> ContinuePayment(ContinuePaymentRequest request)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);
            if (order == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đơn hàng");
            }
            if (request.PaidTheRest <= 0)
            {
                return new ApiErrorResult<bool>("Số tiền không hợp lệ");
            }
            if (request.PaidTheRest == null)
            {
                return new ApiErrorResult<bool>("Số tiền không hợp lệ");
            }
            order.RemainAmount = (decimal)order.RemainAmount - (decimal)request.PaidTheRest;
            if (order.RemainAmount < 0)
            {
                return new ApiErrorResult<bool>("Thanh toán bị dư " + Math.Abs(order.RemainAmount));
            }
            if (order.RemainAmount == 0 && request.TransactionStatus.Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Success))
            {
                order.Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.OrderStatus.Success.ToString();
            }

            var paymentDetail = new OrdersPayment()
            {
                OrdersPaymentId = Guid.NewGuid(),
                OrderId = order.OrderId,
                PaymentAmount = (decimal)request.PaidTheRest,
                PaymentTime = DateTime.Now,
                Status = request.TransactionStatus.ToString(),
                PaymentId = request.PaymentId,
                Message = request.Message,
            };
            await _context.OrdersPayments.AddAsync(paymentDetail);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>("Success số tiền còn lại cần thanh toán là " + order.RemainAmount);
        }

        public async Task<ApiResult<string>> CreateOrder(CreateOrderRequest request)
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
            if (errorList.Any()) return new ApiErrorResult<string>("Lỗi thông tin", errorList);
            if (request.ListOrderProduct == null) return new ApiErrorResult<string>("Đơn hàng không có sản phẩm");
            if (request.ListPaymentId == null) return new ApiErrorResult<string>("Đơn hàng chưa có phương thức thanh toán");
            if (request.CustomerId == Guid.Empty) return new ApiErrorResult<string>("Đơn hàng chưa có người đặt");
            if (request.ListOrderProduct != null)
            {
                if (HasDuplicates(request.ListOrderProduct))
                {
                    return new ApiErrorResult<string>("Sản phẩm bị trùng, vui lòng chọn lại");
                }
            }
            if (request.ListOrderProduct.Count <= 0)
            {
                return new ApiErrorResult<string>("Đơn hàng cần có sản phẩm, vui lòng chọn sản phẩm");

            }
            foreach (var product in request.ListOrderProduct)
            {
                var productEntity = await _context.Products.FindAsync(product.ProductId);
                if (productEntity == null)
                {
                    return new ApiErrorResult<string>("Không tìm thấy sản phẩm");
                }
                var validQuantity = productEntity.Quantity - product.Quantity;
                if (validQuantity < 0)
                {
                    return new ApiErrorResult<string>($"Không đủ hàng cho sản phẩm: {productEntity.ProductId} | {productEntity.ProductName} | Số lượng có thể mua: {productEntity.Quantity}");
                }
            }

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
                Deposit = request.Deposit == null ? 0 : (decimal)request.Deposit,
                Datemodified = DateTime.Now,
                isShip = request.ShipAdress != null ? true : false
            };

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Add and save the order first
                    _context.Orders.Add(order);
                    /*await _context.SaveChangesAsync();*/

                    decimal totalPrice = 0;
                    foreach (var orderProduct in request.ListOrderProduct)
                    {
                        var product = await _context.Products.FindAsync(orderProduct.ProductId);
                        if (product == null) return new ApiErrorResult<string>("Không tìm thấy sản phẩm trong đơn đặt hàng");

                        string WarrantyId = "W" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                        var warranty = new DiamondLuxurySolution.Data.Entities.Warranty()
                        {
                            WarrantyId = WarrantyId,
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
                            WarrantyId = warranty.WarrantyId,
                            Size = orderProduct.Size,

                        };

                        totalPrice += orderDetail.TotalPrice;

                        _context.Warrantys.Add(warranty);
                        _context.OrderDetails.Add(orderDetail);
                    }

                    decimal total = await CalculateTotalPrice(request, totalPrice);
                    order.TotalAmout = total;



                    var user = await _userMananger.FindByIdAsync(request.CustomerId.ToString());
                    if (user != null)
                    {
                        var userPoint = user.Point;
                        var discounts = await _context.Discounts.ToListAsync();
                        foreach (var discount in discounts)
                        {
                            if (discount.From <= userPoint && userPoint <= discount.To)
                            {
                                order.DiscountId = discount.DiscountId;
                                break;
                            }
                        }
                    }
                    var promotion = await _context.Promotions.FindAsync(request.PromotionId);
                    if (promotion != null && promotion.StartDate.Date <= DateTime.Now.Date && DateTime.Now.Date <= promotion.EndDate.Date)
                    {
                        order.PromotionId = promotion.PromotionId;
                    }

                    order.TotalSale = totalPrice - total;

                    if (request.Deposit != null && request.Deposit > 0)
                    {
                        decimal maxDeposit = total * 0.1M;
                        if (request.Deposit < maxDeposit)
                        {
                            return new ApiErrorResult<string>($"Số tiền đặt cọc phải lớn hơn hoặc bằng {maxDeposit}");
                        }
                        if(request.Deposit <=0 || request.Deposit>total) 
                        { 
                            return new ApiErrorResult<string>($"Số tiền đặt cọc không hợp lệ");
                        }

                        order.RemainAmount = total - (decimal)request.Deposit;
                    }
                    else
                    {
                        order.RemainAmount = 0;
                    }
                    var paymentMethod = "";
                    foreach (var paymentId in request.ListPaymentId)
                    {
                        var payment = await _context.Payments.FindAsync(paymentId);
                        if (payment == null) return new ApiErrorResult<string>("Không tìm thấy phương thức thanh toán");

                        var orderPayment = new OrdersPayment()
                        {
                            OrderId = orderId,
                            Message = $"Thanh toán bằng phương thức {payment.PaymentMethod}",
                            PaymentTime = DateTime.Now,
                            Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Waiting.ToString(),
                            OrdersPaymentId = Guid.NewGuid(),
                            PaymentId = paymentId,
                            PaymentAmount = (decimal)order.RemainAmount > 0 ? (decimal)request.Deposit : total,
                            OpenPaymentTime = DateTime.Now
                        };
                        _context.OrdersPayments.Add(orderPayment);
                        paymentMethod += payment.PaymentMethod;
                    }


                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    //Send email customer
                    string relativePath = @"..\..\DiamondLuxurySolution\DiamondLuxurySolution.Utilities\FormSendEmail\CustomerForm.html";
                    // Combine the relative path with the current directory to get the full path
                    string path = Path.GetFullPath(relativePath);

                    if (!System.IO.File.Exists(path))
                    {
                        return new ApiErrorResult<string>("Không tìm thấy email");
                    }
                    string contentCustomer = System.IO.File.ReadAllText(path);
                    contentCustomer = contentCustomer.Replace("{{CustomerName}}", request.ShipName);
                    contentCustomer = contentCustomer.Replace("{{Code}}", orderId);
                    contentCustomer = contentCustomer.Replace("{{Date}}", DateTime.Now.ToString());
                    var dataProduct = "";
                    var listProductExistInDb = _context.OrderDetails.Where(x => x.OrderId == orderId);
                    int index = 0;
                    foreach (var orderDetail in listProductExistInDb)
                    {
                        var productFind = await _context.Products.FindAsync(orderDetail.ProductId);
                        if (productFind == null)
                        {
                            return new ApiErrorResult<string>("Không tìm thấy sản phẩm khi gửi email");
                        }
                        var niSize = orderDetail.Size != null ? orderDetail.Size.ToString() : "Không có";
                        dataProduct += $"<tr>";
                        dataProduct += $"<td style='color: #636363;border: 1px solid #e5e5e5;vertical-align: middle;padding: 12px;text-align: center;'>{++index}</td>"; // Replace Property1 with actual property names
                        dataProduct += $"<td style='color: #636363;border: 1px solid #e5e5e5;vertical-align: middle;padding: 12px;text-align: center;'>{productFind.ProductId}</td>"; // Replace Property1 with actual property names
                        dataProduct += $"<td style='color: #636363;border: 1px solid #e5e5e5;vertical-align: middle;padding: 12px;text-align: center;'>{productFind.ProductName}</td>"; // Replace Property2 with actual property names
                        dataProduct += $"<td style='color: #636363;border: 1px solid #e5e5e5;vertical-align: middle;padding: 12px;text-align: center;'>{orderDetail.Quantity}</td>"; // Replace Property3 with actual property names
                        dataProduct += $"<td style='color: #636363;border: 1px solid #e5e5e5;vertical-align: middle;padding: 12px;text-align: center;'>{niSize}</td>"; // Replace Property3 with actual property names
                        dataProduct += $"<td style='color: #636363;border: 1px solid #e5e5e5;vertical-align: middle;padding: 12px;text-align: center;'>{productFind.SellingPrice.ToString("N0")}₫</td>"; // Replace Property3 with actual property names
                        dataProduct += "</tr>";
                    }
                    contentCustomer = contentCustomer.Replace("{{Product}}", dataProduct);
                    contentCustomer = contentCustomer.Replace("{{NotSalelPrice}}", totalPrice.ToString("N0"));


                    contentCustomer = contentCustomer.Replace("{{Payment}}", paymentMethod);
                    contentCustomer = contentCustomer.Replace("{{TotalSale}}", order.TotalSale?.ToString("N0"));
                    contentCustomer = contentCustomer.Replace("{{Total}}", order.TotalAmout.ToString("N0"));
                    contentCustomer = contentCustomer.Replace("{{Deposit}}", order.Deposit.ToString("N0"));

                    contentCustomer = contentCustomer.Replace("{{Address}}", request.ShipAdress);
                    contentCustomer = contentCustomer.Replace("{{PhoneNumber}}", request.ShipPhoneNumber);
                    contentCustomer = contentCustomer.Replace("{{Email}}", request.ShipEmail);
                    DoingMail.SendMail("LuxuryDiamond", "Đặt hàng thành công", contentCustomer, request.ShipEmail);

                    //Send mail manager
                    string relativePathManager = @"..\..\DiamondLuxurySolution\DiamondLuxurySolution.Utilities\FormSendEmail\ManagerForm.html";
                    // Combine the relative path with the current directory to get the full path
                    string pathManager = Path.GetFullPath(relativePathManager);

                    if (!System.IO.File.Exists(path))
                    {
                        return new ApiErrorResult<string>("Không tìm thấy email");
                    }
                    string contentManager = System.IO.File.ReadAllText(pathManager);
                    contentManager = contentManager.Replace("{{Code}}", orderId);

                    contentManager = contentManager.Replace("{{CustomerName}}", request.ShipName);

                    contentManager = contentManager.Replace("{{Date}}", DateTime.Now.ToString());
                    contentManager = contentManager.Replace("{{Product}}", dataProduct);
                    contentManager = contentManager.Replace("{{Payment}}", paymentMethod);

                    contentManager = contentManager.Replace("{{NotSalelPrice}}", totalPrice.ToString("N0"));
                    contentManager = contentManager.Replace("{{TotalSale}}", order.TotalSale?.ToString("N0"));
                    contentManager = contentManager.Replace("{{Total}}", order.TotalAmout.ToString("N0"));
                    contentManager = contentManager.Replace("{{Deposit}}", order.Deposit.ToString("N0"));

                    contentManager = contentManager.Replace("{{Address}}", request.ShipAdress);
                    contentManager = contentManager.Replace("{{PhoneNumber}}", request.ShipPhoneNumber);
                    contentManager = contentManager.Replace("{{Email}}", request.ShipEmail);
                    DoingMail.SendMail("LuxuryDiamond", "Có đơn hàng mới", contentManager, "diamondluxuryservice@gmail.com");


                    return new ApiSuccessResult<string>(order.OrderId, "Success");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ApiErrorResult<string>(ex.Message);
                }
            }
        }
        public void SendEmailOrder(string code, string toEmail,string filePath)
        {
            // Correct relative path from current directory to the HTML file
            string relativePath = @"..\..\DiamondLuxurySolution\DiamondLuxurySolution.Utilities\FormSendEmail\CustomerForm.html";
            // Combine the relative path with the current directory to get the full path
            string path = Path.GetFullPath(relativePath);

            if (!System.IO.File.Exists(path))
            {
                return;
            }
            string contentCustomer = System.IO.File.ReadAllText(path);
            contentCustomer = contentCustomer.Replace("{{VerifyCode}}", code);
            DoingMail.SendMail("LuxuryDiamond", "Yêu cầu thay đổi mật khẩu", contentCustomer, toEmail);
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
                if (promotion != null && promotion.StartDate.Date <= DateTime.Now.Date && DateTime.Now.Date <= promotion.EndDate.Date && promotion.Status)
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
        private async Task<decimal> CalculateTotalPriceByStaff(CreateOrderByStaffRequest request, decimal totalPrice, Guid cusId)
        {
            decimal total = totalPrice;
            if (request.PromotionId != null)
            {
                var promotion = await _context.Promotions.FindAsync(request.PromotionId);
                if (promotion != null && promotion.StartDate.Date <= DateTime.Now.Date && DateTime.Now.Date <= promotion.EndDate.Date && promotion.Status)
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
        private async Task<decimal> CalculateUpdateTotalPriceByStaff(UpdateOrderRequest request, decimal totalPrice, Guid cusId)
        {
            decimal total = totalPrice;
            if (request.PromotionId != null)
            {
                var promotion = await _context.Promotions.FindAsync(request.PromotionId);
                if (promotion != null && promotion.StartDate.Date <= DateTime.Now.Date && DateTime.Now.Date <= promotion.EndDate.Date && promotion.Status)
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
            List<AppUser> listShipper = await _userMananger.Users.ToListAsync();
            List<AppUser> listShipperWaiting = new List<AppUser>();

            foreach (var shipper in listShipper)
            {
                var role = await _userMananger.GetRolesAsync(shipper);
                if (role.Contains(DiamondLuxurySolution.Utilities.Constants.Systemconstant.UserRoleDefault.DeliveryStaff.ToString()))
                {
                    if (shipper.ShipStatus != null)
                    {
                        if (shipper.ShipStatus.Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.ShiperStatus.Waiting.ToString()))
                        {
                            listShipperWaiting.Add(shipper);
                        }
                    }
                }
            }

            if (listShipperWaiting.Any())
            {
                int index = new Random().Next(listShipperWaiting.Count);
                return listShipperWaiting[index].Id;
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
            if (errorList.Any()) return new ApiErrorResult<bool>("Lỗi thông tin", errorList);
            if (request.ListOrderProduct == null) return new ApiErrorResult<bool>("Đơn hàng không có sản phẩm");
            if (request.ListPaymentId == null) return new ApiErrorResult<bool>("Đơn hàng chưa có phương thức thanh toán");
            if (request.StaffId == Guid.Empty) return new ApiErrorResult<bool>("Đơn hàng chưa có nhân viên tạo");
            if (request.ListOrderProduct != null)
            {
                if (HasDuplicates(request.ListOrderProduct))
                {
                    return new ApiErrorResult<bool>("Sản phẩm bị trùng, vui lòng chọn lại");
                }
            }
            if (request.ListOrderProduct.Count <= 0)
            {
                return new ApiErrorResult<bool>("Đơn hàng cần có sản phẩm, vui lòng chọn sản phẩm");

            }
            foreach (var product in request.ListOrderProduct)
            {
                var productEntityCheck = await _context.Products.FindAsync(product.ProductId);
                if (productEntityCheck == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy sản phẩm");
                }
                var validQuantity = productEntityCheck.Quantity - product.Quantity;
                if (validQuantity < 0)
                {
                    return new ApiErrorResult<bool>($"Không đủ hàng cho sản phẩm: {productEntityCheck.ProductId} | {productEntityCheck.ProductName} | Số lượng có thể mua: {productEntityCheck.Quantity}");
                }
            }
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
                            Point = 0,
                            DateCreated = DateTime.Now
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
                        Status = request.Status,
                        OrderDate = DateTime.Now,
                        Deposit = (decimal)request.Deposit,
                        CustomerId = cusId,
                        isShip = request.isShip,
                        StaffId = request.StaffId,
                        Datemodified = DateTime.Now

                    };



                    // Add and save the order first
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    decimal totalPrice = 0;
                    foreach (var orderProduct in request.ListOrderProduct)
                    {
                        var product = await _context.Products.FindAsync(orderProduct.ProductId);
                        if (product == null) return new ApiErrorResult<bool>("Không tìm thấy sản phẩm trong đơn đặt hàng");
                        string WarrantyId = "W" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);

                        var warranty = new DiamondLuxurySolution.Data.Entities.Warranty()
                        {
                            WarrantyId = WarrantyId,
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
                            WarrantyId = warranty.WarrantyId,
                            Size = orderProduct.Size,
                        };

                        totalPrice += orderDetail.TotalPrice;
                        _context.Warrantys.Add(warranty);
                        _context.OrderDetails.Add(orderDetail);
                    }

                    decimal total = await CalculateTotalPriceByStaff(request, totalPrice, cusId);
                    order.TotalAmout = total;

                    var userCheckPoint = await _userMananger.FindByIdAsync(cusId.ToString());
                    if (userCheckPoint != null)
                    {
                        var userPoint = userCheckPoint.Point;
                        var discounts = await _context.Discounts.ToListAsync();
                        foreach (var discount in discounts)
                        {
                            if (discount.From <= userPoint && userPoint <= discount.To)
                            {
                                order.DiscountId = discount.DiscountId;
                                break;
                            }
                        }
                    }
                    var promotion = await _context.Promotions.FindAsync(request.PromotionId);
                    if (promotion != null && promotion.StartDate.Date <= DateTime.Now.Date && DateTime.Now.Date <= promotion.EndDate.Date)
                    {
                        order.PromotionId = promotion.PromotionId;
                    }
                    order.TotalSale = totalPrice - total;

                    if (request.Deposit != null && request.Deposit > 0)
                    {
                        decimal maxDeposit = total * 0.1M;
                        if (request.Deposit < maxDeposit)
                        {
                            return new ApiErrorResult<bool>($"Số tiền đặt cọc phải lớn hơn hoặc bằng {maxDeposit}");
                        }
                        if (request.Deposit <= 0 || request.Deposit > total)
                        {
                            return new ApiErrorResult<bool>($"Số tiền đặt cọc không hợp lệ");
                        }
                        order.RemainAmount = total - (decimal)request.Deposit;
                    }
                    else
                    {
                        order.RemainAmount = 0;
                    }
                    bool orderPaymentStatusSuccess = false;
                    if (request.Status.ToString().Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.OrderStatus.Success))
                    {
                        orderPaymentStatusSuccess = true;
                    }
                    foreach (var paymentId in request.ListPaymentId)
                    {
                        var payment = await _context.Payments.FindAsync(paymentId);
                        if (payment == null) return new ApiErrorResult<bool>("Không tìm thấy phương thức thanh toán");

                        var orderPayment = new OrdersPayment()
                        {
                            OrderId = orderId,
                            Message = $"Thanh toán bằng phương thức {payment.PaymentMethod}",
                            PaymentTime = DateTime.Now,
                            Status = orderPaymentStatusSuccess == true ? DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Success.ToString() : request.TransactionStatus.ToString(),
                            OrdersPaymentId = Guid.NewGuid(),
                            PaymentId = paymentId,
                            PaymentAmount = (decimal)order.RemainAmount > 0 ? (decimal)request.Deposit : total
                        };
                        _context.OrdersPayments.Add(orderPayment);
                    }




                    if (request.ShipAdress != null && !request.Status.ToString().Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.OrderStatus.InProgress) && !request.Status.ToString().Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.OrderStatus.Canceled))
                    {
                        order.isShip = true;
                        var shipper = await AssignShipper();
                        order.ShipperId = shipper;
                    }





                    await _context.SaveChangesAsync();


                    if (request.Status.ToString().Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.OrderStatus.Success.ToString()))
                    {
                        var orderDetailSellingCount = await _context.OrderDetails.Where(x => x.OrderId == order.OrderId).ToListAsync();

                        //Product Quantity

                        foreach (var item in orderDetailSellingCount)
                        {
                            var product = await _context.Products.FindAsync(item.ProductId);
                            if (product == null)
                            {
                                return new ApiErrorResult<bool>($"Không tìm thấy sản phẩm");
                            }
                            product.Quantity -= item.Quantity;
                            if (product.Quantity < 0)
                            {
                                return new ApiErrorResult<bool>($"Sản phẩm {product.ProductId} | {product.ProductName} đã hết hàng");
                            }
                            if (product.Quantity == 0)
                            {
                                product.Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.ProductStatus.OutOfStock.ToString();
                                product.DateModified = DateTime.Now;
                            }
                            product.SellingCount += item.Quantity;
                        }

                        var point = (int)order.TotalAmout / 10000;
                        var customer = await _userMananger.FindByIdAsync(cusId.ToString());

                        customer.Point = (int?)(customer?.Point + point);

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

        private bool HasDuplicates(ICollection<OrderProductSupport> list)
        {
            return list.GroupBy(x => x).Any(g => g.Count() > 1);
        }
        public async Task<ApiResult<OrderVm>> GetOrderById(string OrderId)
        {
            var order = _context.Orders.Include(x => x.Customer)
                                              .Include(x => x.Discount).Include(x => x.Promotion)
                                              .Include(x => x.OrdersPayment).ThenInclude(x => x.Payment)
                                              .Include(x => x.Shipper).Include(x => x.Promotion)
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
                Deposit = order.Deposit,
                Description = order.Description,
                DateCreated = order.OrderDate,
                Datemodified = order.Datemodified,
                TotalSale = order.TotalSale,
                IsShip = order.isShip,
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
            if (order.Promotion != null)
            {
                orderVms.PromotionVm = new PromotionVm()
                {
                    PromotionName = order.Promotion.PromotionName,
                    BannerImage = order.Promotion.BannerImage,
                    Description = order.Promotion.Description,
                    DiscountPercent = order.Promotion.DiscountPercent,
                    EndDate = order.Promotion.EndDate,
                    MaxDiscount = order.Promotion.MaxDiscount,
                    PromotionId = order.Promotion.PromotionId,
                    PromotionImage = order.Promotion.PromotionImage,
                    StartDate = order.Promotion.StartDate,
                    Status = order.Promotion.Status,

                };
            }
            if (order.Shipper != null)
            {
                var user = await _userMananger.FindByIdAsync(order.ShipperId.ToString());
                var staffVm = new StaffVm()
                {
                    StaffId = user.Id,
                    Dob = (DateTime)(user.Dob ?? DateTime.MinValue),
                    FullName = user.Fullname,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    CitizenIDCard = user.CitizenIDCard,
                    Image = user.Image,
                    Address = user.Address,
                    Status = user.Status,
                    Username = user.UserName
                };

                var listRoleOfUser = await _userMananger.GetRolesAsync(user);

                if (listRoleOfUser.Count > 0)
                {
                    staffVm.ListRoleName = new List<string>();
                    foreach (var role in listRoleOfUser)
                    {
                        staffVm.ListRoleName.Add(role);
                    }
                }
                orderVms.ShiperVm = staffVm;

            }
            if (order.StaffId != null)
            {
                var user = await _userMananger.FindByIdAsync(order.StaffId.ToString());
                var staffVm = new StaffVm()
                {
                    StaffId = user.Id,
                    Dob = (DateTime)(user.Dob ?? DateTime.MinValue),
                    FullName = user.Fullname,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    CitizenIDCard = user.CitizenIDCard,
                    Image = user.Image,
                    Address = user.Address,
                    Status = user.Status,
                    Username = user.UserName
                };

                var listRoleOfUser = await _userMananger.GetRolesAsync(user);

                if (listRoleOfUser.Count > 0)
                {
                    staffVm.ListRoleName = new List<string>();
                    foreach (var role in listRoleOfUser)
                    {
                        staffVm.ListRoleName.Add(role);
                    }
                }
                orderVms.StaffVm = staffVm;
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
                    Message = op.Message,
                    OrderPaymentId = op.OrdersPaymentId,
                    OpenPaymentTime = op.OpenPaymentTime,

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
                        Size = orderDetail.Size,

                    };
                    listOrderSupport.Add(orderProductSupport);
                }

                orderVms.ListOrderProduct = listOrderSupport;
                orderVms.OrdersPaymentVm = orderVms.OrdersPaymentVm.OrderBy(x => x.PaymentTime).ToList();
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
            if (string.IsNullOrEmpty(request.Fullname))
            {
                errorList.Add("Vui lòng nhập tên người nhận hàng");
            }
            if (string.IsNullOrEmpty(request.Email))
            {
                errorList.Add("Vui lòng nhập email nhận hàng");
            }
            if (string.IsNullOrEmpty(request.PhoneNumber))
            {
                errorList.Add("Vui lòng nhập số điện thoại người nhận hàng");
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
            var listCheckDuplicate = new List<OrderProductSupport>();
            foreach (var item in request.ListExistOrderProduct)
            {
                listCheckDuplicate.Add(item);
            }
            foreach (var item in request.ListOrderProduct)
            {
                listCheckDuplicate.Add(item);
            }
            if (listCheckDuplicate.Count <= 0)
            {
                return new ApiErrorResult<bool>("Đơn hàng cần có sản phẩm, vui lòng chọn sản phẩm");

            }
            if (request.ListOrderProduct != null)
            {
                if (HasDuplicates(listCheckDuplicate))
                {
                    return new ApiErrorResult<bool>("Sản phẩm bị trùng, vui lòng chọn lại");
                }
            }
            // Check valid quantity
            var listCheck = new List<OrderProductSupport>();
            foreach (var product in request.ListOrderProduct)
            {
                listCheck.Add(product);
            }
            foreach (var product in request.ListExistOrderProduct)
            {
                listCheck.Add(product);
            }


            foreach (var product in listCheck)
            {
                var productEntityCheck = await _context.Products.FindAsync(product.ProductId);
                if (productEntityCheck == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy sản phẩm");
                }
                var validQuantity = productEntityCheck.Quantity - product.Quantity;
                if (validQuantity < 0)
                {
                    return new ApiErrorResult<bool>($"Không đủ hàng cho sản phẩm: {productEntityCheck.ProductId} | {productEntityCheck.ProductName} | Số lượng: {productEntityCheck.Quantity}");
                }
            }

            // End check valid 



            order.ShipAdress = request.ShipAdress;
            order.ShipPhoneNumber = request.PhoneNumber;
            order.ShipEmail = request.Email;
            order.ShipName = request.Fullname;
            order.Status = request.Status;
            order.Datemodified = DateTime.Now;
            Random rd = new Random();
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
                    Point = 0
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



            // Start Process ****
            // Retrieve existing product details from the database

            var existingProductDetails = await _context.OrderDetails.Include(x => x.Warranty)
                .Where(x => x.OrderId == order.OrderId)
                .ToListAsync();

            bool isChange = false;
            foreach (var orderDetail in request.ListExistOrderProduct)
            {
                var orderDetailEntity = await _context.OrderDetails.Where(x => x.OrderId == request.OrderId && x.ProductId == orderDetail.ProductId).FirstAsync();
                if (orderDetail.ProductId != orderDetailEntity.ProductId)
                {
                    isChange = true;
                }
                if (orderDetail.Quantity != orderDetailEntity.Quantity)
                {
                    isChange = true;
                }
                if (orderDetailEntity.Size != null)
                {
                    if (orderDetail.Size != orderDetailEntity.Size)
                    {
                        isChange = true;
                    }
                }
            }

            if (isChange)
            {
                List<DiamondLuxurySolution.Data.Entities.Warranty> listRemoveWarranty = new List<DiamondLuxurySolution.Data.Entities.Warranty>();
                foreach (var warranty in existingProductDetails)
                {
                    listRemoveWarranty.Add(warranty.Warranty);
                }

                _context.Warrantys.RemoveRange(listRemoveWarranty);

                _context.OrderDetails.RemoveRange(existingProductDetails);
                await _context.SaveChangesAsync();

                if (request.ListExistOrderProduct != null)
                {
                    foreach (var orderDetail in request.ListExistOrderProduct)
                    {
                        var product = await _context.Products.FindAsync(orderDetail.ProductId);
                        if (product == null)
                        {
                            return new ApiErrorResult<bool>("Không tìm thấy kim cương phụ");
                        }
                        if (orderDetail.Quantity <= 0)
                        {
                            return new ApiErrorResult<bool>($"Kim cương phụ cần có số lượng");
                        }
                        string WarrantyId = "W" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);

                        var warranty = new DiamondLuxurySolution.Data.Entities.Warranty()
                        {
                            WarrantyId = WarrantyId,
                            DateActive = DateTime.Now,
                            DateExpired = DateTime.Now.AddMonths(12),
                            WarrantyName = $"Phiếu bảo hành cho sản phẩm {product.ProductName} | {product.ProductId}",
                            Description = "Phiếu bảo hành có giá trị trong vòng 12 tháng",
                            Status = true,
                        };
                        var orderDetailEntity = new OrderDetail
                        {
                            ProductId = orderDetail.ProductId,
                            Quantity = orderDetail.Quantity,
                            OrderId = order.OrderId,
                            UnitPrice = product.SellingPrice,
                            TotalPrice = orderDetail.Quantity * product.SellingPrice,
                            WarrantyId = warranty.WarrantyId,
                            Size = orderDetail.Size,

                        };
                        await _context.Warrantys.AddAsync(warranty);
                        await _context.OrderDetails.AddAsync(orderDetailEntity);
                    }
                }
                await _context.SaveChangesAsync();

                // Update the database with the new product details
                foreach (var updatedProduct in request.ListExistOrderProduct)
                {
                    var orderDetail = existingProductDetails
                        .FirstOrDefault(x => x.ProductId == updatedProduct.ProductId);

                    if (orderDetail != null)
                    {
                        orderDetail.Quantity = updatedProduct.Quantity;
                        orderDetail.Size = updatedProduct.Size;
                    }
                }

                // Save changes to the database
                await _context.SaveChangesAsync();
            }

            //End *****

            if (request.ListOrderProduct != null)
            {
                foreach (var orderDetail in request.ListOrderProduct)
                {
                    var product = await _context.Products.FindAsync(orderDetail.ProductId);
                    if (product == null)
                    {
                        return new ApiErrorResult<bool>("Không tìm thấy kim cương phụ");
                    }
                    if (orderDetail.Quantity <= 0)
                    {
                        return new ApiErrorResult<bool>($"Kim cương phụ cần có số lượng");
                    }
                    string WarrantyId = "W" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);

                    var warranty = new DiamondLuxurySolution.Data.Entities.Warranty()
                    {
                        WarrantyId = WarrantyId,
                        DateActive = DateTime.Now,
                        DateExpired = DateTime.Now.AddMonths(12),
                        WarrantyName = $"Phiếu bảo hành cho sản phẩm {product.ProductName} | {product.ProductId}",
                        Description = "Phiếu bảo hành có giá trị trong vòng 12 tháng",
                        Status = true,
                    };
                    var orderDetailEntity = new OrderDetail
                    {
                        ProductId = orderDetail.ProductId,
                        Quantity = orderDetail.Quantity,
                        OrderId = order.OrderId,
                        UnitPrice = product.SellingPrice,
                        TotalPrice = orderDetail.Quantity * product.SellingPrice,
                        WarrantyId = warranty.WarrantyId,
                        Size = orderDetail.Size,
                    };
                    await _context.Warrantys.AddAsync(warranty);
                    await _context.OrderDetails.AddAsync(orderDetailEntity);
                }
            }
            _context.Orders.Update(order);

            await _context.SaveChangesAsync();
            // Process SubGemPrice
            decimal totalPrice = 0;
            var orderDetailList = _context.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
            if (orderDetailList != null && orderDetailList.Count > 0)
            {
                foreach (var orderDetail in orderDetailList)
                {
                    var productEntity = await _context.Products.FindAsync(orderDetail.ProductId);
                    if (productEntity == null) continue;
                    totalPrice += ((decimal)productEntity.SellingPrice * orderDetail.Quantity);
                }
            }


            decimal total = await CalculateUpdateTotalPriceByStaff(request, totalPrice, cusId);
            order.TotalAmout = total;

            var userCheckPoint = await _userMananger.FindByIdAsync(cusId.ToString());
            if (userCheckPoint != null)
            {
                var userPoint = userCheckPoint.Point;
                var discounts = await _context.Discounts.ToListAsync();
                foreach (var discount in discounts)
                {
                    if (discount.From <= userPoint && userPoint <= discount.To)
                    {
                        order.DiscountId = discount.DiscountId;
                        break;
                    }
                }
            }
            order.TotalSale = totalPrice - total;

            if (request.Deposit != null && request.Deposit > 0)
            {
                decimal maxDeposit = total * 0.1M;
                if (request.Deposit < maxDeposit)
                {
                    return new ApiErrorResult<bool>($"Số tiền đặt cọc phải lớn hơn hoặc bằng {maxDeposit}");
                }
                order.RemainAmount = total - (decimal)request.Deposit;
            }
            else
            {
                order.RemainAmount = 0;
            }
            var checkSuccess = true;
            var checkMoney = (decimal)order.TotalAmout;
            if (request.StatusOrderPayment != null && request.StatusOrderPayment.Count > 0)
            {
                foreach (var item in request.StatusOrderPayment)
                {
                    var orderPayment = await _context.OrdersPayments.FindAsync(item.OrderPaymentId);
                    if (orderPayment != null)
                    {
                        orderPayment.Status = item.Status.ToString();
                    }
                    if (item.Status.Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Success.ToString()))
                    {
                        checkMoney -= orderPayment.PaymentAmount;
                    }
                    if (!item.Status.Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Success.ToString()))
                    {
                        checkSuccess = false;
                    }
                }
            }

            bool orderPaymentStatusSuccess = false;
            if (request.Status.ToString().Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.OrderStatus.Success.ToString()))
            {
                orderPaymentStatusSuccess = true;
                var listPayment = _context.OrdersPayments.Where(x => x.OrderId == order.OrderId).ToList();
                foreach(var item in listPayment)
                {
                    item.Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Success.ToString();
                }
            }

            if (request.ShipAdress != null && !request.Status.ToString().Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.OrderStatus.InProgress.ToString()) && !request.Status.ToString().Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.OrderStatus.Canceled.ToString()))
            {
                order.isShip = true;
                order.ShipperId = await AssignShipper();
            }

            if (order.StaffId == null)
            {
                order.StaffId = request.StaffId;
            }

            await _context.SaveChangesAsync();

            //Process selling Count
            if (request.Status.ToString().Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.OrderStatus.Success.ToString()))
            {
                var orderDetailSellingCount = await _context.OrderDetails.Where(x => x.OrderId == order.OrderId).ToListAsync();
               
                //Product Quantity

                foreach (var item in orderDetailSellingCount)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product == null)
                    {
                        return new ApiErrorResult<bool>($"Không tìm thấy sản phẩm");
                    }
                    product.Quantity -= item.Quantity;
                    if (product.Quantity < 0)
                    {
                        return new ApiErrorResult<bool>($"Sản phẩm {product.ProductId} | {product.ProductName} đã hết hàng");
                    }
                    if (product.Quantity == 0)
                    {
                        product.Status = DiamondLuxurySolution.Utilities.Constants.Systemconstant.ProductStatus.OutOfStock.ToString();
                        product.DateModified = DateTime.Now;
                    }
                    product.SellingCount += item.Quantity;
                }

                var point = (int)order.TotalAmout / 10000;
                var customer = await _userMananger.FindByIdAsync(cusId.ToString());

                customer.Point = (int?)(customer?.Point + point);

            }


            await _context.SaveChangesAsync();

            return new ApiSuccessResult<bool>(true, "Success");

        }
        public class OrderProductSupportComparer : IEqualityComparer<OrderProductSupport>
        {
            public bool Equals(OrderProductSupport x, OrderProductSupport y)
            {
                // Check if the products are equal based on ProductId, Quantity, and Size
                return x.ProductId == y.ProductId;
            }

            public int GetHashCode(OrderProductSupport obj)
            {
                // Generate a hash code based on ProductId, Quantity, and Size
                unchecked
                {
                    int hashCode = obj.ProductId.GetHashCode();
                    return hashCode;
                }
            }
        }
        public async Task<ApiResult<string>> UpdateShipper(UpdateShipperRequest request)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);
            if (order == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy đơn hàng");
            }
            if (string.IsNullOrEmpty(order.ShipAdress))
            {
                return new ApiErrorResult<string>("Chưa có địa chỉ giao hàng");
            }
            if (!order.Status.ToString().Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.OrderStatus.InProgress) && !order.Status.ToString().Equals(DiamondLuxurySolution.Utilities.Constants.Systemconstant.OrderStatus.Canceled))
            {
                order.ShipperId = await AssignShipper();

                if (order.ShipperId == null)
                {
                    return new ApiErrorResult<string>("Chưa có nhân viên giao hàng");
                }
                var shipper = await _userMananger.FindByIdAsync(order.ShipperId.ToString());
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<string>(shipper.Fullname, "Success");
            }
            else
            {
                return new ApiErrorResult<string>("Đơn hàng ở trạng thái không phù hợp");
            }

        }

        public async Task<ApiResult<PageResult<OrderVm>>> ViewOrder(ViewOrderRequest request)
        {
            var listOrder = await _context.Orders.Include(x => x.Customer)
                                              .Include(x => x.Discount).Include(x => x.Promotion)
                                              .Include(x => x.OrdersPayment).ThenInclude(x => x.Payment)
                                              .Include(x => x.Shipper).Include(x => x.Promotion)
                                              .Include(x => x.OrderDetails).ThenInclude(x => x.Warranty).ToListAsync();

            if (request.Keyword != null)
            {
                listOrder = listOrder.Where(x =>
                 x.ShipName.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase) ||
                 x.ShipEmail.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase) ||
                 x.ShipPhoneNumber.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase) ||
                 x.OrderId.Equals(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            listOrder = listOrder.OrderBy(x => x.Datemodified).ToList();

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
                    DateCreated = order.OrderDate,
                    Datemodified = order.Datemodified
                };
                if (order.Promotion != null)
                {
                    orderVms.PromotionVm = new PromotionVm()
                    {
                        PromotionName = order.Promotion.PromotionName,
                        BannerImage = order.Promotion.BannerImage,
                        Description = order.Promotion.Description,
                        DiscountPercent = order.Promotion.DiscountPercent,
                        EndDate = order.Promotion.EndDate,
                        MaxDiscount = order.Promotion.MaxDiscount,
                        PromotionId = order.Promotion.PromotionId,
                        PromotionImage = order.Promotion.PromotionImage,
                        StartDate = order.Promotion.StartDate,
                        Status = order.Promotion.Status,
                    };
                }
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
                            Size = orderDetail.Size,

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

        public async Task<ApiResult<decimal>> TotalIncome()
        {
            var totalOrder = (decimal)_context.OrdersPayments.Where(x => x.Status == DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Success.ToString()).Sum(x => x.PaymentAmount);
            return new ApiSuccessResult<decimal>(totalOrder, "Success");
        }

        public async Task<ApiResult<int>> TotalOrder()
        {
            var totalOrder = _context.Orders.Count();
            return new ApiSuccessResult<int>(totalOrder, "Success");
        }

        public async Task<ApiResult<int>> AllOrderToday()
        {
            var totalOrder = _context.Orders.Where(x => x.OrderDate.Date == DateTime.Now.Date).Count();
            return new ApiSuccessResult<int>(totalOrder, "Success");
        }

        public async Task<ApiResult<List<decimal>>> IncomeAYear()
        {
            var currentYear = DateTime.Now.Year;
            var totalOrder = await _context.OrdersPayments.Where(x => x.Status == DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Success.ToString() &&
                    x.PaymentTime.Year == currentYear).GroupBy(x => x.PaymentTime.Month)
                .Select(x => new
                {
                    Month = x.Key,
                    TotalIncome = x.Sum(x => x.PaymentAmount)
                }).ToListAsync();
            var incomeByMonth = new List<decimal>(new decimal[12]);

            foreach (var item in totalOrder)
            {
                incomeByMonth[item.Month - 1] = item.TotalIncome;
            }

            return new ApiSuccessResult<List<decimal>>(incomeByMonth, "Success");
        }

        public async Task<ApiResult<List<OrderVm>>> RecentTransaction()
        {
            var orders = await _context.Orders.Include(x => x.Customer).Include(x => x.OrdersPayment).ThenInclude(x => x.Payment).OrderByDescending(y => y.OrderDate).Take(8)
                .ToListAsync();

            List<OrderVm> listOrderVm = new List<OrderVm>();
            foreach (var order in orders)
            {
                var orderVms = new OrderVm()
                {
                    OrderId = order.OrderId,
                    ShipName = order.ShipName,
                };

                if (order.OrdersPayment != null)
                {
                    orderVms.OrdersPaymentVm = order.OrdersPayment.OrderByDescending(y => y.PaymentTime).Take(1).Select(op => new OrderPaymentSupportDTO()
                    {
                        PaymentId = op.PaymentId,
                        PaymentMethod = op.Payment.PaymentMethod,
                        PaymentTime = op.PaymentTime,
                        Status = op.Status,
                        PaymentAmount = op.PaymentAmount,
                        Message = op.Message
                    }).ToList();
                }
                listOrderVm.Add(orderVms);
            }


            return new ApiSuccessResult<List<OrderVm>>(listOrderVm, "Success");
        }

        public async Task<ApiResult<List<OrderVm>>> RecentSuccessTransaction()
        {
            var orders = await _context.Orders.Include(x => x.Customer).Include(x => x.OrdersPayment).ThenInclude(x => x.Payment).Where(order => order.OrdersPayment.Any(payment => payment.Status == DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Success.ToString()))
        .OrderByDescending(y => y.OrderDate).Take(8)
                .ToListAsync();

            List<OrderVm> listOrderVm = new List<OrderVm>();
            foreach (var order in orders)
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
                    Deposit = order.Deposit,
                    Description = order.Description,
                    DateCreated = order.OrderDate,
                    Datemodified = order.Datemodified,
                    TotalSale = order.TotalSale,
                    IsShip = order.isShip,
                };

                if (order.OrdersPayment != null)
                {
                    orderVms.OrdersPaymentVm = order.OrdersPayment.OrderByDescending(y => y.PaymentTime).Take(1).Select(op => new OrderPaymentSupportDTO()
                    {
                        PaymentId = op.PaymentId,
                        PaymentMethod = op.Payment.PaymentMethod,
                        PaymentTime = op.PaymentTime,
                        Status = op.Status,
                        PaymentAmount = op.PaymentAmount,
                        Message = op.Message
                    }).ToList();
                }
                listOrderVm.Add(orderVms);
            }


            return new ApiSuccessResult<List<OrderVm>>(listOrderVm, "Success");
        }

        public async Task<ApiResult<List<OrderVm>>> RecentWaitTransaction()
        {
            var orders = await _context.Orders.Include(x => x.Customer).Include(x => x.OrdersPayment).ThenInclude(x => x.Payment).Where(order => order.OrdersPayment.Any(payment => payment.Status == DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Waiting.ToString()))
        .OrderByDescending(y => y.OrderDate).Take(8)
                .ToListAsync();

            List<OrderVm> listOrderVm = new List<OrderVm>();
            foreach (var order in orders)
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
                    Deposit = order.Deposit,
                    Description = order.Description,
                    DateCreated = order.OrderDate,
                    Datemodified = order.Datemodified,
                    TotalSale = order.TotalSale,
                    IsShip = order.isShip,
                };

                if (order.OrdersPayment != null)
                {
                    orderVms.OrdersPaymentVm = order.OrdersPayment.OrderByDescending(y => y.PaymentTime).Take(1).Select(op => new OrderPaymentSupportDTO()
                    {
                        PaymentId = op.PaymentId,
                        PaymentMethod = op.Payment.PaymentMethod,
                        PaymentTime = op.PaymentTime,
                        Status = op.Status,
                        PaymentAmount = op.PaymentAmount,
                        Message = op.Message
                    }).ToList();
                }
                listOrderVm.Add(orderVms);
            }


            return new ApiSuccessResult<List<OrderVm>>(listOrderVm, "Success");
        }

        public async Task<ApiResult<List<OrderVm>>> RecentFailTransaction()
        {
            var orders = await _context.Orders.Include(x => x.Customer).Include(x => x.OrdersPayment).ThenInclude(x => x.Payment).Where(order => order.OrdersPayment.Any(payment => payment.Status == DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Failed.ToString()))
        .OrderByDescending(y => y.OrderDate).Take(8)
                .ToListAsync();

            List<OrderVm> listOrderVm = new List<OrderVm>();
            foreach (var order in orders)
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
                    Deposit = order.Deposit,
                    Description = order.Description,
                    DateCreated = order.OrderDate,
                    Datemodified = order.Datemodified,
                    TotalSale = order.TotalSale,
                    IsShip = order.isShip,
                };

                if (order.OrdersPayment != null)
                {
                    orderVms.OrdersPaymentVm = order.OrdersPayment.OrderByDescending(y => y.PaymentTime).Take(1).Select(op => new OrderPaymentSupportDTO()
                    {
                        PaymentId = op.PaymentId,
                        PaymentMethod = op.Payment.PaymentMethod,
                        PaymentTime = op.PaymentTime,
                        Status = op.Status,
                        PaymentAmount = op.PaymentAmount,
                        Message = op.Message
                    }).ToList();
                }
                listOrderVm.Add(orderVms);
            }


            return new ApiSuccessResult<List<OrderVm>>(listOrderVm, "Success");
        }

        public async Task<ApiResult<List<int>>> OrderByQuarter()
        {
            var currentYear = DateTime.Now.Year;

            var orders = await _context.Orders
                .Where(o => o.OrderDate.Year == currentYear)
                .ToListAsync();

            var quarterlyOrderCounts = orders
                .GroupBy(o => GetQuarter(o.OrderDate))
                .Select(g => new
                {
                    Quarter = g.Key,
                    OrderCount = g.Count()
                })
                .OrderBy(q => q.Quarter)
                .ToList();

            var result = new List<int> { 0, 0, 0, 0 };
            foreach (var item in quarterlyOrderCounts)
            {
                result[item.Quarter - 1] = item.OrderCount;
            }

            return new ApiSuccessResult<List<int>>(result, "Success");
        }

        private int GetQuarter(DateTime date)
        {
            if (date.Month <= 3) return 1;
            if (date.Month <= 6) return 2;
            if (date.Month <= 9) return 3;
            return 4;
        }

        public async Task<ApiResult<decimal>> IncomeToday()
        {
            var orders = _context.OrdersPayments.Where(x => x.Status ==
                                DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Success.ToString()
                                && x.PaymentTime.Date.Equals(DateTime.Today.Date)).Sum(x => x.PaymentAmount);
            return new ApiSuccessResult<decimal>(orders, "Success");


        }

        public async Task<ApiResult<PageResult<OrderVm>>> GetListOrderOfCustomer(ViewOrderRequest request)
        {
            var user = await _userMananger.FindByIdAsync(request.CustomerId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<PageResult<OrderVm>>("Khách hàng không tồn tại");
            }

            var listOrder = await _context.Orders.Include(x => x.Customer)
                                              .Include(x => x.Discount).Include(x => x.Promotion)
                                              .Include(x => x.OrdersPayment).ThenInclude(x => x.Payment)
                                              .Include(x => x.Shipper).Include(x => x.Promotion)
                                              .Include(x => x.OrderDetails).ThenInclude(x => x.Warranty)
                                              .Where(x => x.CustomerId == user.Id).ToListAsync();

            if (listOrder == null)
            {
                return new ApiSuccessResult<PageResult<OrderVm>>(null, "Không tìm thấy đơn hàng");
            }
            listOrder = listOrder.OrderByDescending(x => x.OrderDate).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listOrder.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            List<OrderVm> listOrderVm = new List<OrderVm>();
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
                    DateCreated = order.OrderDate,
                    Datemodified = order.Datemodified
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

                if (order.Promotion != null)
                {
                    orderVms.PromotionVm = new PromotionVm()
                    {
                        PromotionName = order.Promotion.PromotionName,
                        BannerImage = order.Promotion.BannerImage,
                        Description = order.Promotion.Description,
                        DiscountPercent = order.Promotion.DiscountPercent,
                        EndDate = order.Promotion.EndDate,
                        MaxDiscount = order.Promotion.MaxDiscount,
                        PromotionId = order.Promotion.PromotionId,
                        PromotionImage = order.Promotion.PromotionImage,
                        StartDate = order.Promotion.StartDate,
                        Status = order.Promotion.Status,
                    };
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
                            Size = orderDetail.Size,

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

        public async Task<ApiResult<bool>> AcceptProcessOrder(AcceptProcessOrder request)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);
            if (order == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đơn hàng");
            }
            order.StaffId = request.StaffId;
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");

        }

        public async Task<ApiResult<string>> ExportFileInspecertificateAndWarranty(ExportFileRequest request)
        {
            var order = _context.Orders.Include(x => x.Customer)
                .Include(x => x.OrderDetails).ThenInclude(x => x.Warranty)
                .Include(x => x.OrderDetails).ThenInclude(x => x.Product)
                    .ThenInclude(x => x.Gem)
                    .ThenInclude(x => x.InspectionCertificate)
                .Include(x => x.OrderDetails)
                    .ThenInclude(x => x.Product)
                    .ThenInclude(x => x.Gem)
                    .ThenInclude(x => x.GemPriceList)
                .FirstOrDefault(x => x.OrderId == request.OrderId);

            if (order == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy đơn hàng");
            }
            var dataWarranty = "";
            int index = 0;
            foreach (var item in order.OrderDetails)
            {

                dataWarranty += "<tr>";
                dataWarranty += $"<td>{++index}</td>"; // Replace Property1 with actual property names
                dataWarranty += $"<td>{item.WarrantyId}</td>"; // Replace Property1 with actual property names
                dataWarranty += $"<td>{item.ProductId}</td>"; // Replace Property2 with actual property names
                dataWarranty += $"<td>{item.Product.ProductName}</td>"; // Replace Property3 with actual property names
                dataWarranty += $"<td>{item.Quantity}</td>"; // Replace Property3 with actual property names
                dataWarranty += $"<td>{item.Warranty.DateActive}</td>"; // Replace Property3 with actual property names
                dataWarranty += $"<td>{item.Warranty.DateExpired}</td>"; // Replace Property3 with actual property names
                dataWarranty += "</tr>";


                string relativePath = @"..\..\DiamondLuxurySolution\DiamondLuxurySolution.Utilities\FormDiamond\InspectCertificate.html";
                string path = Path.GetFullPath(relativePath);

                if (!System.IO.File.Exists(path))
                {
                    return new ApiErrorResult<string>("Không tìm thấy file");
                }

                string contentCustomer = System.IO.File.ReadAllText(path);
                contentCustomer = contentCustomer.Replace("{{DateTimeNow}}", DateTime.Now.Date.ToString("yyyy-MM-dd"));

                var inspectionCertificate = item.Product.Gem.InspectionCertificate.InspectionCertificateId;
                contentCustomer = contentCustomer.Replace("{{CINumber}}", inspectionCertificate.ToString());
                var cut = item.Product.Gem.GemPriceList.Cut;
                contentCustomer = contentCustomer.Replace("{{Cut}}", cut.ToString());
                var caratWeight = item.Product.Gem.GemPriceList.CaratWeight;
                contentCustomer = contentCustomer.Replace("{{CaratWeight}}", caratWeight.ToString());
                var color = item.Product.Gem.GemPriceList.Color;
                contentCustomer = contentCustomer.Replace("{{Color}}", color.ToString());
                var clarity = item.Product.Gem.GemPriceList.Clarity;
                contentCustomer = contentCustomer.Replace("{{Clarity}}", clarity.ToString());
                var polish = item.Product.Gem.Polish;
                contentCustomer = contentCustomer.Replace("{{Polish}}", polish.ToString());
                var symetry = item.Product.Gem.Symetry;
                contentCustomer = contentCustomer.Replace("{{Symetry}}", symetry.ToString());
                var fluorescenceRaw = item.Product.Gem.Fluoresence;
                var fluorescence = fluorescenceRaw ? "Yes" : "No";
                contentCustomer = contentCustomer.Replace("{{Fluor}}", fluorescence.ToString());
                var isOriginRaw = item.Product.Gem.IsOrigin;
                var isOrigin = isOriginRaw ? "Origin" : "Synthetic";
                contentCustomer = contentCustomer.Replace("{{Origin}}", isOrigin.ToString());

                try
                {
                    string directoryPath = @"C:\DiamondInfo"; // Desired directory

                    // Ensure the directory exists
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    string directoryPathWarranty = @"C:\DiamondInfo\" + order.CustomerId + @"\"; // Desired directory
                    if (!Directory.Exists(directoryPathWarranty))
                    {
                        Directory.CreateDirectory(directoryPathWarranty);
                    }



                    string filename = "GCN_" + inspectionCertificate + ".pdf";
                    var pdfFile = HtmlConverter.FromHtmlString(contentCustomer, 220, 500);
                    string filePath = Path.Combine(@"C:\DiamondInfo\" + order.CustomerId + @"\", filename);
                    FileSystem.WriteAllBytes(filePath, pdfFile, true);
                }
                catch (Exception ex)
                {
                    return new ApiErrorResult<string>($"Không thể xuât file PDF: {ex.Message}");
                }
            }

            // Warranty 
            string relativePathWarranty = @"..\..\DiamondLuxurySolution\DiamondLuxurySolution.Utilities\FormDiamond\Warranty.html";
            string pathWarranty = Path.GetFullPath(relativePathWarranty);

            if (!System.IO.File.Exists(pathWarranty))
            {
                return new ApiErrorResult<string>("Không tìm thấy file");
            }

            string warrantyCustomer = System.IO.File.ReadAllText(pathWarranty);
            warrantyCustomer = warrantyCustomer.Replace("{{data}}", dataWarranty);

            warrantyCustomer = warrantyCustomer.Replace("{{DateTime}}", DateTime.Now.ToString());

            var CusName = order.Customer.Fullname;
            warrantyCustomer = warrantyCustomer.Replace("{{CusName}}", string.IsNullOrEmpty(CusName) ? "Không có" : CusName.ToString());

            var CusAddress = order.Customer.Address;
            warrantyCustomer = warrantyCustomer.Replace("{{CusAddress}}", string.IsNullOrEmpty(CusAddress) ? "Không có" : CusAddress?.ToString());

            var CusDob = order.Customer.Dob;
            warrantyCustomer = warrantyCustomer.Replace("{{CusDob}}", CusDob != null ? CusDob.Value.Date.ToString() : "Không có ");

            var CusPhone = order.Customer.PhoneNumber;
            warrantyCustomer = warrantyCustomer.Replace("{{CusPhone}}", string.IsNullOrEmpty(CusPhone) ? "Không có" : CusPhone.ToString());

            var CusEmail = order.Customer.Email;
            warrantyCustomer = warrantyCustomer.Replace("{{CusEmail}}", string.IsNullOrEmpty(CusEmail) ? "Không có" : CusEmail.ToString());
            try
            {
                string directoryPath = @"C:\DiamondInfo"; // Desired directory

                // Ensure the directory exists
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string directoryPathWarranty = @"C:\DiamondInfo\" + order.CustomerId + @"\"; // Desired directory
                if (!Directory.Exists(directoryPathWarranty))
                {
                    Directory.CreateDirectory(directoryPathWarranty);
                }



                string filename = "PBH_" + CusName + ".pdf";
                var pdfFile = HtmlConverter.FromHtmlString(warrantyCustomer, 220, 900);
                string filePath = Path.Combine(@"C:\DiamondInfo\" + order.CustomerId + @"\", filename);
                FileSystem.WriteAllBytes(filePath, pdfFile, true);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>($"Không thể xuât file PDF: {ex.Message}");
            }



            // END warranty 

            return new ApiSuccessResult<string>(order.CustomerId.ToString(), "Success");
        }

        public async Task<ApiResult<List<decimal>>> IncomeByWeek()
        {
            var today = DateTime.Today;

            // Calculate the start of the week (previous Monday)
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday - (today.DayOfWeek == DayOfWeek.Sunday ? 7 : 0));

            // Calculate the end of the week (current Sunday)
            var endOfWeek = startOfWeek.AddDays(6).Date.AddDays(1).AddTicks(-1);

            // Create a list to hold the daily incomes
            var dailyIncomes = new List<decimal>();

            for (var date = startOfWeek; date <= endOfWeek; date = date.AddDays(1))
            {
                var income = await _context.OrdersPayments
                    .Where(x => x.Status == DiamondLuxurySolution.Utilities.Constants.Systemconstant.TransactionStatus.Success.ToString()
                                && x.PaymentTime.Date == date.Date)
                    .SumAsync(x => x.PaymentAmount);

                dailyIncomes.Add(income);
            }

            // Return the results
            return new ApiSuccessResult<List<decimal>>(dailyIncomes, "Success");
        }

    }
}
