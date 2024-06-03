using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using DiamondLuxurySolution.ViewModel.Models.SubGem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.SubGem
{
    public interface ISubGemRepo
    {
        public Task<ApiResult<bool>> CreateSubGem(CreateSubGemRequest request);
        public Task<ApiResult<bool>> UpdateSubGem(UpdateSubGemRequest request);
        public Task<ApiResult<bool>> DeleteSubGem(DeleteSubGemRequest request);
        public Task<ApiResult<SubGemVm>> GetSubGemById(Guid SubGemId);

        public Task<ApiResult<List<SubGemVm>>> GetAll();

        public Task<ApiResult<PageResult<SubGemVm>>> ViewSubGemInCustomer(ViewSubGemRequest request);
        public Task<ApiResult<PageResult<SubGemVm>>> ViewSubGemInManager(ViewSubGemRequest request);
    }
}
