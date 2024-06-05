using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;

namespace DiamondLuxurySolution.AdminCrewApp.Service.KnowledgeNews
{
    public interface IKnowLedgeNewsApiService
    {
        public Task<ApiResult<bool>> CreateKnowledgeNews(CreateKnowledgeNewsRequest request);
        public Task<ApiResult<bool>> UpdateKnowledgeNews(UpdateKnowledgeNewsRequest request);
        public Task<ApiResult<bool>> DeleteKnowledgeNews(DeleteKnowledgeNewsRequest request);
        public Task<ApiResult<KnowledgeNewsVm>> GetKnowledgeNewsById(int KnowledgeNewsId);
        public Task<ApiResult<PageResult<KnowledgeNewsVm>>> ViewKnowledgeNews(ViewKnowledgeNewsRequest request);
    }
}
