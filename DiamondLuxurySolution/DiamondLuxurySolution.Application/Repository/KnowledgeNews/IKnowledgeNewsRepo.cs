using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.KnowledgeNews
{
    public interface IKnowledgeNewsRepo
    {
        public Task<ApiResult<bool>> CreateKnowledgeNews(CreateKnowledgeNewsRequest request);
        public Task<ApiResult<bool>> UpdateKnowledgeNews(UpdateKnowledgeNewsRequest request);
        public Task<ApiResult<bool>> DeleteKnowledgeNews(DeleteKnowledgeNewsRequest request);
        public Task<ApiResult<KnowledgeNewsVm>> GetKnowledgeNewsById(int KnowledgeNewsId);
        public Task<ApiResult<PageResult<KnowledgeNewsVm>>> ViewKnowledgeNewsInCustomer(ViewKnowledgeNewsRequest request);
        public Task<ApiResult<PageResult<KnowledgeNewsVm>>> ViewKnowledgeNewsInManager(ViewKnowledgeNewsRequest request);
    }
}
