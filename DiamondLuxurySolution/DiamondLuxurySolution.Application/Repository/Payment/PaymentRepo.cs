using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Payment;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Payment
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public PaymentRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreatePayment(CreatePaymentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.PaymentMethod))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên phương thức thanh toán");
            }            
            var payment = new DiamondLuxurySolution.Data.Entities.Payment
            {
                PaymentMethod = request.PaymentMethod,
                Description = !string.IsNullOrWhiteSpace(request.Description) ? request.Description : "",
                Status = request.Status,
                    
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }


        public async Task<ApiResult<bool>> DeletePayment(DeletePaymentRequest request)
        {
            var payment = await _context.Payments.FindAsync(request.PaymentId);
            if (payment == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy phương thức thanh toán");
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<List<PaymentVm>>> GetAll()
        {
            var list = await _context.Payments.ToListAsync();
            var rs = list.Select(x => new PaymentVm()
            {
                PaymentId = x.PaymentId,
                Description = x.Description,
                PaymentMethod = x.PaymentMethod,
                Status = x.Status,
            }).ToList();
            return new ApiSuccessResult<List<PaymentVm>>(rs);
        }

        public async Task<ApiResult<PaymentVm>> GetPaymentById(Guid PaymentId)
        {
            var payment = await _context.Payments.FindAsync(PaymentId);
            if (payment == null)
            {
                return new ApiErrorResult<PaymentVm>("Không tìm thấy phương thức thanh toán");
            }
            var paymentVm = new PaymentVm()
            {
               PaymentId = PaymentId,
               Description = payment.Description,    
               PaymentMethod = payment.PaymentMethod,
               Status = payment.Status
            };
            return new ApiSuccessResult<PaymentVm>(paymentVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdatePayment(UpdatePaymentRequest request)
        {
            if (string.IsNullOrEmpty(request.PaymentMethod))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên phương thức thanh toán");
            }

            var payment = await _context.Payments.FindAsync(request.PaymentId);
            if (payment == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy phương thức thanh toán");
            }
            payment.Status = request.Status;
            payment.Description = !string.IsNullOrWhiteSpace(request.Description) ? request.Description : "";
            payment.PaymentMethod = request.PaymentMethod;

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<PaymentVm>>> ViewPayment(ViewPaymentRequest request)
        {
            var listPayment = await _context.Payments.ToListAsync();
            if (request.KeyWord != null)
            {
                listPayment = listPayment.Where(x => x.PaymentMethod.Contains(request.KeyWord, StringComparison.OrdinalIgnoreCase)).ToList();

            }
            listPayment = listPayment.OrderByDescending(x => x.PaymentMethod).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listPayment.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listPaymentVm = listPaging.Select(x => new PaymentVm()
            {
                PaymentId = x.PaymentId,
                PaymentMethod = x.PaymentMethod,
                Description = x.Description,
                Status = x.Status,
            }).ToList();
            var listResult = new PageResult<PaymentVm>()
            {
                Items = listPaymentVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listPayment.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<PaymentVm>>(listResult, "Success");
        }


    }
}

