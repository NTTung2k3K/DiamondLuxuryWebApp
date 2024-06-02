using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNewsCategory;

namespace DiamondLuxurySolution.AdminCrewApp.Service.KnowledgeNewsCategoty
{
    public interface IKnowledgeNewsCategoryApiService
    {
        public Task<ApiResult<List<KnowledgeNewsCategoryVm>>> GetAll();
        public Task<ApiResult<bool>> CreateKnowledgeNewsCategory(CreateKnowledgeNewsCategoryRequest request);
        public Task<ApiResult<bool>> UpdateKnowledgeNewsCategory(UpdateKnowledgeNewsCategoryRequest request);
        public Task<ApiResult<bool>> DeleteKnowledgeNewsCategory(DeleteKnowledgeNewsCategoryRequest request);
        public Task<ApiResult<KnowledgeNewsCategoryVm>> GetKnowledgeNewsCategoryById(int KnowledgeNewsCategoryId);
        public Task<ApiResult<PageResult<KnowledgeNewsCategoryVm>>> ViewKnowledgeNewsCategory(ViewKnowledgeNewsCategoryRequest request);
    }
}
