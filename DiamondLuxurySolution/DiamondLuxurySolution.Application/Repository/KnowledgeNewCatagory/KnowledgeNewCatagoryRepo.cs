using Azure.Core;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNewsCategory;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.KnowledgeNewCatagory
{
    public class KnowledgeNewCatagoryRepo : IKnowledgeNewCatagoryRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public KnowledgeNewCatagoryRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateKnowledgeNewsCategory(CreateKnowledgeNewsCategoryRequest request)
        {
            if (string.IsNullOrEmpty(request.KnowledgeNewCatagoriesName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên danh mục tin tức");
            }
            var knowledgeNewCatagory = new DiamondLuxurySolution.Data.Entities.KnowledgeNewCatagory
            {
                KnowledgeNewCatagoriesName = request.KnowledgeNewCatagoriesName,
                Description = request.Description != null ? request.Description : "",
            };
            _context.KnowledgeNewCatagories.Add(knowledgeNewCatagory);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> DeleteKnowledgeNewsCategory(DeleteKnowledgeNewsCategoryRequest request)
        {
            var knowledgeNewsCatagory = await _context.KnowledgeNewCatagories.FindAsync(request.KnowledgeNewsCategoryId);
            if (knowledgeNewsCatagory == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy danh mục tin tức");
            }
            _context.KnowledgeNewCatagories.Remove(knowledgeNewsCatagory);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<List<KnowledgeNewsCategoryVm>>> GetAll()
        {

            var list = await _context.KnowledgeNewCatagories.ToListAsync();
            var rs = list.Select(x => new KnowledgeNewsCategoryVm()
            {
                KnowledgeNewCatagoryId=x.KnowledgeNewCatagoryId,
                Description=x.Description,
                KnowledgeNewCatagoriesName=x.KnowledgeNewCatagoriesName,
            }).ToList();
            return new ApiSuccessResult<List<KnowledgeNewsCategoryVm>>(rs);
        }

        public async Task<ApiResult<KnowledgeNewsCategoryVm>> GetKnowledgeNewsCategoryById(int knowledgeNewsCategoryId)
        {
            var knowledgeNewsCatagory = await _context.KnowledgeNewCatagories.FindAsync(knowledgeNewsCategoryId);
            if (knowledgeNewsCatagory == null)
            {
                return new ApiErrorResult<KnowledgeNewsCategoryVm>("Không tìm thấy danh mục tin tức");
            }
            var knowledgeNewsCatagoryVm = new KnowledgeNewsCategoryVm()
            {
                KnowledgeNewCatagoryId = knowledgeNewsCatagory.KnowledgeNewCatagoryId,
                KnowledgeNewCatagoriesName = knowledgeNewsCatagory.KnowledgeNewCatagoriesName,
                Description = knowledgeNewsCatagory.Description,
                
            };
            return new ApiSuccessResult<KnowledgeNewsCategoryVm>(knowledgeNewsCatagoryVm, "Success");
        }
    
        public async Task<ApiResult<bool>> UpdateKnowledgeNewsCategory(UpdateKnowledgeNewsCategoryRequest request)
        {
            if (string.IsNullOrEmpty(request.KnowledgeNewCatagoriesName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên danh mục tin tức");
            }
            var knowledgeNewsCatagory = await _context.KnowledgeNewCatagories.FindAsync(request.KnowledgeNewCatagoryId);
            if (knowledgeNewsCatagory == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy danh mục tin tức");
            }
            knowledgeNewsCatagory.KnowledgeNewCatagoriesName = request.KnowledgeNewCatagoriesName;
            knowledgeNewsCatagory.Description = !string.IsNullOrEmpty(request.Description) ? request.Description : "";
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<KnowledgeNewsCategoryVm>>> ViewKnowledgeNewsCategory(ViewKnowledgeNewsCategoryRequest request)
        {
            var listKnowledgeNewsCategory = await _context.KnowledgeNewCatagories.ToListAsync();
            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                listKnowledgeNewsCategory = listKnowledgeNewsCategory.Where(x => x.KnowledgeNewCatagoriesName.Contains(request.KeyWord, StringComparison.OrdinalIgnoreCase)
                || x.Description.Contains(request.KeyWord, StringComparison.OrdinalIgnoreCase)).ToList();
                

            }
            listKnowledgeNewsCategory = listKnowledgeNewsCategory.OrderByDescending(x => x.KnowledgeNewCatagoriesName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listKnowledgeNewsCategory.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();
            var listKnowledgeNewsCategoryVm = listPaging.Select(x => new KnowledgeNewsCategoryVm()
            {
                KnowledgeNewCatagoriesName = x.KnowledgeNewCatagoriesName,
                Description = x.Description,
                KnowledgeNewCatagoryId = x.KnowledgeNewCatagoryId
            }).ToList();
            var listResult = new PageResult<KnowledgeNewsCategoryVm>()
            {
                Items = listKnowledgeNewsCategoryVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listKnowledgeNewsCategory.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<KnowledgeNewsCategoryVm>>(listResult, "Success");
        }
    }
}
