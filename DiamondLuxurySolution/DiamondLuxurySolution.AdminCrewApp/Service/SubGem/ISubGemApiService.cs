using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.SubGem;

namespace DiamondLuxurySolution.AdminCrewApp.Service.SubGem
{
    public interface ISubGemApiService
    {
        public Task<ApiResult<List<SubGemVm>>> GetAll();
        public Task<ApiResult<bool>> CreateSubGem(CreateSubGemRequest request);
        public Task<ApiResult<bool>> UpdateSubGem(UpdateSubGemRequest request);
        public Task<ApiResult<bool>> DeleteSubGem(DeleteSubGemRequest request);
        public Task<ApiResult<SubGemVm>> GetSubGemId(Guid SubGemId);
        public Task<ApiResult<PageResult<SubGemVm>>> ViewSubGemInManager(ViewSubGemRequest request);
    }
}
