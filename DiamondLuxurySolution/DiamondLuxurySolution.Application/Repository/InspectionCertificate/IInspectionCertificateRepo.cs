using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.InspectionCertificate
{
    public interface IInspectionCertificateRepo
    {
        public Task<ApiResult<bool>> CreateInspectionCertificate(CreateInspectionCertificateRequest request);
        public Task<ApiResult<bool>> UpdateInspectionCertificate(UpdateInspectionCertificateRequest request);
        public Task<ApiResult<bool>> DeleteInspectionCertificate(DeleteInspectionCertificateRequest request);
        public Task<ApiResult<InspectionCertificateVm>> GetInspectionCertificateById(string InspectionCertificateId);
        public Task<ApiResult<PageResult<InspectionCertificateVm>>> ViewInspectionCertificateInCustomer(ViewInspectionCertificateRequest request);

        public Task<ApiResult<PageResult<InspectionCertificateVm>>> ViewInspectionCertificateInManager(ViewInspectionCertificateRequest request);
    }
}
