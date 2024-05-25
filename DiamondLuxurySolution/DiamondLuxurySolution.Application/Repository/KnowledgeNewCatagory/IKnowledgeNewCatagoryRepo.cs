using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNewsCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.KnowledgeNewCatagory
{
    public interface IKnowledgeNewCatagoryRepo
    {
        public Task<ApiResult<bool>> CreateKnowledgeNewsCategory(CreateKnowledgeNewsCategoryRequest request);
        public Task<ApiResult<bool>> UpdateKnowledgeNewsCategory(UpdateKnowledgeNewsCategoryRequest request);
        public Task<ApiResult<bool>> DeleteKnowledgeNewsCategory(DeleteKnowledgeNewsCategoryRequest request);
        public Task<ApiResult<KnowledgeNewsCategoryVm>> GetKnowledgeNewsCategoryById(int KnowledgeNewsCategoryId);
        public Task<ApiResult<PageResult<KnowledgeNewsCategoryVm>>> ViewKnowledgeNewsCategory(ViewKnowledgeNewsCategoryRequest request);

    }
}
