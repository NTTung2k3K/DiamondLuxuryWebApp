using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNewsCategory;
using DiamondLuxurySolution.ViewModel.Models.MaterialPriceList;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.KnowledgeNews
{
    public class KnowledgeNewsRepo : IKnowledgeNewsRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly UserManager<AppUser> _userManager;
        public KnowledgeNewsRepo(LuxuryDiamondShopContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<ApiResult<bool>> CreateKnowledgeNews(CreateKnowledgeNewsRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.KnowledgeNewsName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên tin tức");
            }
            if (request.WriterId == Guid.Empty)
            {
                return new ApiErrorResult<bool>("Không tìm thấy người viết");
            }
            if (string.IsNullOrWhiteSpace(request.KnowledgeNewCatagoryId.ToString()))
            {
                return new ApiErrorResult<bool>("Không tìm thấy danh mục của tin tức");
            }
            var writer = await _userManager.FindByIdAsync(request.WriterId.ToString());
            if (writer == null)
            {
                return new ApiErrorResult<bool>("Tin tức không tìm thấy người viết");
            }
            var newsCategory = await _context.KnowledgeNewCatagories.FindAsync(request.KnowledgeNewCatagoryId);
            if (newsCategory == null)
            {
                return new ApiErrorResult<bool>("Tin tức không có danh mục cụ thể");
            }

            var knowledgeNews = new DiamondLuxurySolution.Data.Entities.KnowledgeNews
            {
                KnowledgeNewsName = request.KnowledgeNewsName.Trim(),
                WriterId = request.WriterId,
                KnowledgeNewCatagoryId = request.KnowledgeNewCatagoryId,
                Active = request.Active,
                DateCreated = DateTime.Now,
                DateModified = null,
                Description = !string.IsNullOrEmpty(request.Description) ? request.Description.Trim() : "",
                Thumnail = request.Thumnail != null ? await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Thumnail) : "",
                Writer = writer,
                KnowledgeNewCatagory = newsCategory
            };

            _context.KnowledgeNews.Add(knowledgeNews);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> DeleteKnowledgeNews(DeleteKnowledgeNewsRequest request)
        {
            var knowledgeNews = await _context.KnowledgeNews.FindAsync(request.KnowledgeNewsId);
            if (knowledgeNews == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy tin tức cần xóa");
            }
            _context.KnowledgeNews.Remove(knowledgeNews);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }


        public async Task<ApiResult<KnowledgeNewsVm>> GetKnowledgeNewsById(int KnowledgeNewsId)
        {
            var knowledgeNews = await _context.KnowledgeNews.FindAsync(KnowledgeNewsId);
            if (knowledgeNews == null)
            {
                return new ApiErrorResult<KnowledgeNewsVm>("Không tìm thấy tin tức");
            }
            var writer = await _userManager.FindByIdAsync(knowledgeNews.WriterId.ToString());
            var knowledgeNewsCategory = await _context.KnowledgeNewCatagories.FindAsync(knowledgeNews.KnowledgeNewCatagoryId);

            var knowledgeNewsVm = new KnowledgeNewsVm()
            {
                KnowledgeNewsId = KnowledgeNewsId,
                KnowledgeNewsName = knowledgeNews.KnowledgeNewsName,
                Description = knowledgeNews.Description,
                DateCreated = knowledgeNews.DateCreated,
                DateModified = knowledgeNews.DateModified,
                Thumnail = knowledgeNews.Thumnail,
                Active = knowledgeNews.Active,
                KnowledgeNewCatagory = knowledgeNewsCategory,
                Writer = writer
            };
            return new ApiSuccessResult<KnowledgeNewsVm>(knowledgeNewsVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateKnowledgeNews(UpdateKnowledgeNewsRequest request)
        {
            var knowledgeNews = await _context.KnowledgeNews.FindAsync(request.KnowledgeNewsId);
            if (knowledgeNews == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy danh mục tin tức");
            }
            if (string.IsNullOrWhiteSpace(request.KnowledgeNewsName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên tin tức");
            }
            if (request.DateModified == DateTime.MinValue)
            {
                return new ApiErrorResult<bool>("Vui lòng nhập ngày thay đổi tin tức");
            }
            knowledgeNews.KnowledgeNewsName = request.KnowledgeNewsName;
            knowledgeNews.Thumnail = request.Thumnail != null ? await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Thumnail) : "";
            knowledgeNews.Description = !string.IsNullOrWhiteSpace(request.Description) ? request.Description : "";
            knowledgeNews.Active = request.Active;
            knowledgeNews.DateModified = request.DateModified;
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<KnowledgeNewsVm>>> ViewKnowledgeNewsInCustomer(ViewKnowledgeNewsRequest request)
        {
            var listKnowledgeNews = await _context.KnowledgeNews.ToListAsync();
            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                listKnowledgeNews = listKnowledgeNews.Where(x => x.KnowledgeNewsName.Contains(request.KeyWord)).ToList();

            }
            listKnowledgeNews = listKnowledgeNews.Where(x => x.Active == true).OrderByDescending(x => x.KnowledgeNewsName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listKnowledgeNews.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listKnowledgeNewslVm = new List<KnowledgeNewsVm>();
            foreach (var x in listPaging)
            {
                var writer = await _userManager.FindByIdAsync(x.WriterId.ToString());
                var knowledgeCategory = await _context.KnowledgeNewCatagories.FindAsync(x.KnowledgeNewCatagoryId);
                var knowledgeNewsVm = new KnowledgeNewsVm()
                {
                    Active = x.Active,
                    DateCreated = x.DateCreated,
                    DateModified = x.DateCreated,
                    Description = x.Description,
                    KnowledgeNewCatagory = knowledgeCategory,
                    KnowledgeNewsId = x.KnowledgeNewsId,
                    KnowledgeNewsName = x.KnowledgeNewsName,
                    Thumnail = x.Thumnail,
                    Writer = x.Writer
                };
                listKnowledgeNewslVm.Add(knowledgeNewsVm);
            }
            var listResult = new PageResult<KnowledgeNewsVm>()
            {
                Items = listKnowledgeNewslVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listKnowledgeNews.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<KnowledgeNewsVm>>(listResult, "Success");
        }


        public async Task<ApiResult<PageResult<KnowledgeNewsVm>>> ViewKnowledgeNewsInManager(ViewKnowledgeNewsRequest request)
        {
            var listKnowledgeNews = await _context.KnowledgeNews.ToListAsync();
            if (!string.IsNullOrEmpty(request.KeyWord))
            {
                listKnowledgeNews = listKnowledgeNews.Where(x => x.KnowledgeNewsName.Contains(request.KeyWord)).ToList();

            }
            listKnowledgeNews = listKnowledgeNews.OrderByDescending(x => x.KnowledgeNewsName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listKnowledgeNews.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listKnowledgeNewslVm = new List<KnowledgeNewsVm>();
            foreach (var x in listPaging)
            {
                var writer = await _userManager.FindByIdAsync(x.WriterId.ToString());
                var knowledgeCategory = await _context.KnowledgeNewCatagories.FindAsync(x.KnowledgeNewCatagoryId);
                var knowledgeNewsVm = new KnowledgeNewsVm()
                {
                    Active = x.Active,
                    DateCreated = x.DateCreated,
                    DateModified = x.DateCreated,
                    Description = x.Description,
                    KnowledgeNewCatagory = knowledgeCategory,
                    KnowledgeNewsId = x.KnowledgeNewsId,
                    KnowledgeNewsName = x.KnowledgeNewsName,
                    Thumnail = x.Thumnail,
                    Writer = x.Writer
                };
                listKnowledgeNewslVm.Add(knowledgeNewsVm);
            }
            var listResult = new PageResult<KnowledgeNewsVm>()
            {
                Items = listKnowledgeNewslVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listKnowledgeNews.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<KnowledgeNewsVm>>(listResult, "Success");
        }

    }
}

