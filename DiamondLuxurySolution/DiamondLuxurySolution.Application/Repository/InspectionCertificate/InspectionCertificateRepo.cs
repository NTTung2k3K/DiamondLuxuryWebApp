using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
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
            if (string.IsNullOrEmpty(request.InspectionCertificateName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên giấy chứng nhận");
            }
            
            var inspectionCertificate = new DiamondLuxurySolution.Data.Entities.InspectionCertificate
            {
                InspectionCertificateId = await GenerateUniqueInspectionCertificateIdAsync(),
                InspectionCertificateName = request.InspectionCertificateName,
                DateGrading = request.DateGrading,
                Status = request.Status,
            };
            if (request.Logo != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Logo);
                inspectionCertificate.Logo = firebaseUrl;
            } else
            {
                inspectionCertificate.Logo = "";
            }

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
                newId = "IC" + random.Next(0, 9).ToString() + random.Next(0, 9).ToString() +
                    random.Next(0, 9).ToString() + random.Next(0, 9).ToString(); 
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
            if (string.IsNullOrEmpty(request.InspectionCertificateName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên giấy chứng nhận");
            }
            
            var inspectionCertificate = await _context.InspectionCertificates.FindAsync(request.InspectionCertificateId);
            if (inspectionCertificate == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy giấy chứng nhận");
            }
            
            inspectionCertificate.InspectionCertificateName = request.InspectionCertificateName;
            inspectionCertificate.DateGrading = request.DateGrading;
            inspectionCertificate.Status = request.Status;
            if (request.Logo != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Logo);
                inspectionCertificate.Logo = firebaseUrl;
            }
            else
            {
                inspectionCertificate.Logo = "";
            }

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
