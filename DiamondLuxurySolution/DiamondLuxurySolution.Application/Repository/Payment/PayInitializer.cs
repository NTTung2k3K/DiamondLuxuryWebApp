using Azure.Core;
using DiamondLuxurySolution.Data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Payment
{
    public class PayInitializer : IPaymentInitializer
    {
        private readonly LuxuryDiamondShopContext _context;
        public PayInitializer(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task CreateDefaultPayment()
        {
            var codCount = await _context.Payments.Where(x => x.PaymentMethod == "COD").ToListAsync();
            if(codCount.Count == 0)
            {
                var cod = new DiamondLuxurySolution.Data.Entities.Payment
                {
                    PaymentMethod = "COD",
                    Description = "Thanh toán khi nhận hàng",
                    Status = true,

                };
                _context.Payments.Add(cod);
            }
            var paypalCount = await _context.Payments.Where(x => x.PaymentMethod == "Paypal").ToListAsync();
            if(paypalCount.Count == 0)
            {
                var paypal = new DiamondLuxurySolution.Data.Entities.Payment
                {
                    PaymentMethod = "Paypal",
                    Description = "Thanh toán bằng Paypal",
                    Status = true,

                };
                _context.Payments.Add(paypal);
            }
            var VnPAYCount = await _context.Payments.Where(x => x.PaymentMethod == "VNPAY").ToListAsync();
            if (VnPAYCount.Count == 0)
            {
                var paypal = new DiamondLuxurySolution.Data.Entities.Payment
                {
                    PaymentMethod = "VNPAY",
                    Description = "Thanh toán bằng VNPAY",
                    Status = true,

                };
                _context.Payments.Add(paypal);
            }
            var livePayCount = await _context.Payments.Where(x => x.PaymentMethod == "Thanh Toán Trực Tuyến").ToListAsync();
            if (livePayCount.Count == 0)
            {
                var paypal = new DiamondLuxurySolution.Data.Entities.Payment
                {
                    PaymentMethod = "Thanh Toán Trực Tuyến",
                    Description = "Thanh Toán Trực Tuyến Cho Tất Cả Nền Tảng",
                    Status = true,

                };
                _context.Payments.Add(paypal);
            }
            var cashierCount = await _context.Payments.Where(x => x.PaymentMethod == "Thanh toán tại quầy").ToListAsync();
            if (paypalCount.Count == 0)
            {
                var cashier = new DiamondLuxurySolution.Data.Entities.Payment
                {
                    PaymentMethod = "Thanh toán tại quầy",
                    Description = "Thanh toán tại quầy",
                    Status = true,

                };
                _context.Payments.Add(cashier);
            }

            await _context.SaveChangesAsync();
        }
    }
}
