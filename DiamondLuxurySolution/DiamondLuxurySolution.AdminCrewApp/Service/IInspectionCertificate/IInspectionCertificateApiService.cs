using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Platform;

namespace DiamondLuxurySolution.AdminCrewApp.Service.IInspectionCertificate
{
    public interface IInspectionCertificateApiService
    {
        public Task<ApiResult<List<InspectionCertificateVm>>> GetAll();
        public Task<ApiResult<bool>> CreateInspectionCertificate(CreateInspectionCertificateRequest request);
        public Task<ApiResult<bool>> UpdateInspectionCertificate(UpdateInspectionCertificateRequest request);
        public Task<ApiResult<bool>> DeleteInspectionCertificate(DeleteInspectionCertificateRequest request);
        public Task<ApiResult<InspectionCertificateVm>> GetInspectionCertificateById(string InspectionCertificateId);
        public Task<ApiResult<PageResult<InspectionCertificateVm>>> ViewInspectionCertificateInCustomer(ViewInspectionCertificateRequest request);

        public Task<ApiResult<PageResult<InspectionCertificateVm>>> ViewInspectionCertificateInManager(ViewInspectionCertificateRequest request);
    }
}
