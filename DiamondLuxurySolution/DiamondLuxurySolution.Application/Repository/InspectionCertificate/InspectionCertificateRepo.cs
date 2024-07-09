using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Contact;
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
            List<string> errorList = new List<string>();
            if (string.IsNullOrWhiteSpace(request.InspectionCertificateId))
            {
                errorList.Add("Vui lòng nhập mã giấy chứng nhận");
            }

            if (string.IsNullOrWhiteSpace(request.InspectionCertificateName))
            {
                errorList.Add("Vui lòng nhập tên giấy chứng nhận");
            }
            


            if (errorList.Any())
            {
                return new ApiErrorResult<bool>();
            }


            var inspectionCertificateExist = await _context.InspectionCertificates.FindAsync(request.InspectionCertificateId);
            if (inspectionCertificateExist != null)
            {
                return new ApiErrorResult<bool>("Mã giấy chứng nhận đã tồn tại");
            }
            var inspectionCertificate = new DiamondLuxurySolution.Data.Entities.InspectionCertificate
            {
                InspectionCertificateId = request.InspectionCertificateId.ToUpper(),
                InspectionCertificateName = request.InspectionCertificateName,
                DateGrading = DateTime.Now,
                Status = request.Status,
            };
            if (request.Logo != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Logo);
                inspectionCertificate.Logo = firebaseUrl;
            }
            else
            {
                return new ApiErrorResult<bool>("Vui lòng thêm hình");
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
            List<string> errorList = new List<string>();
            if (string.IsNullOrWhiteSpace(request.InspectionCertificateId))
            {
                errorList.Add("Vui lòng nhập mã giấy chứng nhận");
            }
            if (string.IsNullOrWhiteSpace(request.InspectionCertificateName))
            {
                errorList.Add("Vui lòng nhập tên giấy chứng nhận");
            }
            var inspectionCertificate = await _context.InspectionCertificates.FindAsync(request.InspectionCertificateId);

            inspectionCertificate.InspectionCertificateName = request.InspectionCertificateName;
            inspectionCertificate.DateGrading = request.DateGrading;
            inspectionCertificate.Status = request.Status;
            if (request.Logo != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Logo);
                inspectionCertificate.Logo = firebaseUrl;
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>();
            }

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<InspectionCertificateVm>>> ViewInspectionCertificateInCustomer(ViewInspectionCertificateRequest request)
        {
            var listInspectionCertificate = await _context.InspectionCertificates.ToListAsync();
            if (request.Keyword != null)
            {
                listInspectionCertificate = listInspectionCertificate.Where(x => x.InspectionCertificateName.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                || x.DateGrading.ToString().Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();

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
            }).OrderByDescending(x => x.DateGrading).ToList();
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
                listInspectionCertificate = listInspectionCertificate.Where(x => x.InspectionCertificateName.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();

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
            }).OrderByDescending(x => x.DateGrading).ToList();
            var listResult = new PageResult<InspectionCertificateVm>()
            {
                Items = listInspectionCertificateVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listInspectionCertificate.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<InspectionCertificateVm>>(listResult, "Success");
        }

        public async Task<ApiResult<List<InspectionCertificateVm>>> GetAll()
        {
            var list = await _context.InspectionCertificates.ToListAsync();
            var rs = list.Select(x => new InspectionCertificateVm()
            {
                InspectionCertificateId = x.InspectionCertificateId,
                InspectionCertificateName = x.InspectionCertificateName,
                DateGrading = x.DateGrading,
                Logo= x.Logo,
                Status = x.Status,
            }).ToList();
            return new ApiSuccessResult<List<InspectionCertificateVm>>(rs);
        }
    }
}
