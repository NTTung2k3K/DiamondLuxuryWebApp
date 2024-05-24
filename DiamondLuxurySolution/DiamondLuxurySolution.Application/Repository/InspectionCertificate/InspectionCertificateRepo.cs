using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.InspectionCertificate
{
    public class InspectionCertificateRepo : IInspectionCertificateRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public InspectionCertificateRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateInspectionCertificate(CreateInspectionCertificateRequest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.InspectionCertificateName))
            {
                errorList.Add("Vui lòng nhập tên giấy chứng nhận");
            }
            if (request.DateGrading == null)
            {
                errorList.Add("Vui lòng nhập ngày làm giấy chứng nhận");

            }
            if (request.Logo == null)
            {
                errorList.Add("Vui lòng gắn Logo");

            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Logo);
            var inspectionCertificate = new DiamondLuxurySolution.Data.Entities.InspectionCertificate
            {
                InspectionCertificateId = await GenerateUniqueInspectionCertificateIdAsync(),
                InspectionCertificateName = request.InspectionCertificateName,
                DateGrading = request.DateGrading,
                Logo = firebaseUrl,
                Status = request.Status,
            };
            _context.InspectionCertificates.Add(inspectionCertificate);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<string> GenerateUniqueInspectionCertificateIdAsync()
        {
            string newId;
            bool exists;
            Random random = new Random();

            do
            {
                newId = "IP" + random.Next(1000, 10000).ToString(); // Tạo ID với định dạng "IP" theo sau là 4 chữ số ngẫu nhiên
                exists = await _context.InspectionCertificates.AnyAsync(ic => ic.InspectionCertificateId == newId);
            } while (exists);

            return newId;
        }

        public async Task<ApiResult<bool>> DeleteInspectionCertificate(DeleteInspectionCertificateRequest request)
        {
            var inspectionCertificate = await _context.InspectionCertificates.FindAsync(request.InspectionCertificateId);
            if (inspectionCertificate == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy giấy chứng nhận");
            }

            _context.InspectionCertificates.Remove(inspectionCertificate);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<InspectionCertificateVm>> GetInspectionCertificateById(string InspectionCertificateId)
        {
            var inspectionCertificate = await _context.InspectionCertificates.FindAsync(InspectionCertificateId);
            if (inspectionCertificate == null)
            {
                return new ApiErrorResult<InspectionCertificateVm>("Không tìm thấy giấy chứng nhận");
            }
            var inspectionCertificateVm = new InspectionCertificateVm()
            {
                InspectionCertificateId = InspectionCertificateId,
                InspectionCertificateName = inspectionCertificate.InspectionCertificateName,
                DateGrading = inspectionCertificate.DateGrading,
                Logo = inspectionCertificate.Logo,
                Status = inspectionCertificate.Status,
            };
            return new ApiSuccessResult<InspectionCertificateVm>(inspectionCertificateVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateInspectionCertificate(UpdateInspectionCertificateRequest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.InspectionCertificateName))
            {
                errorList.Add("Vui lòng nhập tên giấy chứng nhận");
            }
            if (request.DateGrading == null)
            {
                errorList.Add("Vui lòng nhập ngày làm giấy chứng nhận");
            }
            if (request.Logo == null)
            {
                errorList.Add("Vui lòng gắn Logo");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            
            var inspectionCertificate = await _context.InspectionCertificates.FindAsync(request.InspectionCertificateId);
            if (inspectionCertificate == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy giấy chứng nhận");
            }
            string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Logo);
            inspectionCertificate.InspectionCertificateName = request.InspectionCertificateName;
            inspectionCertificate.DateGrading = request.DateGrading;
            inspectionCertificate.Logo = firebaseUrl;
            inspectionCertificate.Status = request.Status;
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<InspectionCertificateVm>>> ViewInspectionCertificateInCustomer(ViewInspectionCertificateRequest request)
        {
            var listInspectionCertificate = await _context.InspectionCertificates.ToListAsync();
            if (request.Keyword != null)
            {
                listInspectionCertificate = listInspectionCertificate.Where(x => x.InspectionCertificateName.Contains(request.Keyword)).ToList();

            }
            listInspectionCertificate = listInspectionCertificate.Where(x => x.Status).OrderByDescending(x => x.InspectionCertificateName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listInspectionCertificate.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listInspectionCertificateVm = listPaging.Select(x => new InspectionCertificateVm()
            {
                InspectionCertificateId = x.InspectionCertificateId,
                InspectionCertificateName = x.InspectionCertificateName,
                DateGrading = x.DateGrading,
                Logo = x.Logo,
                Status = x.Status,
            }).ToList();
            var listResult = new PageResult<InspectionCertificateVm>()
            {
                Items = listInspectionCertificateVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listInspectionCertificate.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<InspectionCertificateVm>>(listResult, "Success");
        }

        public async Task<ApiResult<PageResult<InspectionCertificateVm>>> ViewInspectionCertificateInManager(ViewInspectionCertificateRequest request)
        {
            var listInspectionCertificate = await _context.InspectionCertificates.ToListAsync();
            if (request.Keyword != null)
            {
                listInspectionCertificate = listInspectionCertificate.Where(x => x.InspectionCertificateName.Contains(request.Keyword)).ToList();

            }
            listInspectionCertificate = listInspectionCertificate.OrderByDescending(x => x.InspectionCertificateName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listInspectionCertificate.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listInspectionCertificateVm = listPaging.Select(x => new InspectionCertificateVm()
            {
                InspectionCertificateId = x.InspectionCertificateId,
                InspectionCertificateName = x.InspectionCertificateName,
                DateGrading = x.DateGrading,
                Logo = x.Logo,
                Status = x.Status,
            }).ToList();
            var listResult = new PageResult<InspectionCertificateVm>()
            {
                Items = listInspectionCertificateVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listInspectionCertificate.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<InspectionCertificateVm>>(listResult, "Success");
        }
    }
}
