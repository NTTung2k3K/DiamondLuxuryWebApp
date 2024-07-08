using Azure.Core;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNewsCategory;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
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
                return new ApiErrorResult<bool>("Vui lòng nhập tên tin tức kiến thức");
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
                WriterId = (Guid)request.WriterId,
                KnowledgeNewCatagoryId = (int)request.KnowledgeNewCatagoryId,
                Active = request.Active,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
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

        public async Task<ApiResult<List<KnowledgeNewsVm>>> GetAll()
        {
            var listKnowledgeNewslVm = new List<KnowledgeNewsVm>();
            var knowledgeNews = await _context.KnowledgeNews.ToListAsync();
            foreach (var x in knowledgeNews)
            {
                var user = await _userManager.FindByIdAsync(x.WriterId.ToString());
                var writer = new StaffVm()
                {
                    StaffId = user.Id,
                    FullName = user.Fullname,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Dob = (DateTime)(user.Dob ?? DateTime.MinValue),
                    Status = user.Status,
                    CitizenIDCard = user.CitizenIDCard,
                    Address = user.Address,
                    Image = user.Image,
                };
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count > 0)
                {
                    writer.ListRoleName = new List<string>();
                    foreach (var role in roles)
                    {
                        writer.ListRoleName.Add(role);
                    }
                }
                var knowledgeCategory = await _context.KnowledgeNewCatagories.FindAsync(x.KnowledgeNewCatagoryId);
                var knowledgeNewsCategoryVm = new KnowledgeNewsCategoryVm
                {
                    Description = knowledgeCategory.Description,
                    KnowledgeNewCatagoriesName = knowledgeCategory.KnowledgeNewCatagoriesName,
                    KnowledgeNewCatagoryId = knowledgeCategory.KnowledgeNewCatagoryId
                };

                var knowledgeNewsVm = new KnowledgeNewsVm()
                {
                    Active = x.Active,
                    DateCreated = x.DateCreated,
                    DateModified = x.DateCreated,
                    Description = x.Description,
                    KnowledgeNewsId = x.KnowledgeNewsId,
                    KnowledgeNewsName = x.KnowledgeNewsName,
                    Thumnail = x.Thumnail,
                    KnowledgeNewCatagoryVm = knowledgeNewsCategoryVm,
                    Writer = writer
                };
                listKnowledgeNewslVm.Add(knowledgeNewsVm);
            }
            return new ApiSuccessResult<List<KnowledgeNewsVm>>(listKnowledgeNewslVm);
        }

        public async Task<ApiResult<KnowledgeNewsVm>> GetKnowledgeNewsById(int KnowledgeNewsId)
        {
            var knowledgeNews = await _context.KnowledgeNews.FindAsync(KnowledgeNewsId);
            if (knowledgeNews == null)
            {
                return new ApiErrorResult<KnowledgeNewsVm>("Không tìm thấy tin tức");
            }
            var user = await _userManager.FindByIdAsync(knowledgeNews.WriterId.ToString());
            var writer = new StaffVm()
            {
                StaffId = user.Id,
                FullName = user.Fullname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Dob = (DateTime)(user.Dob ?? DateTime.MinValue),
                Status = user.Status,
                CitizenIDCard = user.CitizenIDCard,
                Address = user.Address,
                Image = user.Image,
            };
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Count > 0)
            {
                writer.ListRoleName = new List<string>();
                foreach (var role in roles)
                {
                    writer.ListRoleName.Add(role);
                }
            }
            var knowledgeNewsCategory = await _context.KnowledgeNewCatagories.FindAsync(knowledgeNews.KnowledgeNewCatagoryId);
            var knowledgeNewsCategoryVm = new KnowledgeNewsCategoryVm
            {
                Description = knowledgeNewsCategory.Description,
                KnowledgeNewCatagoriesName = knowledgeNewsCategory.KnowledgeNewCatagoriesName,
                KnowledgeNewCatagoryId = knowledgeNewsCategory.KnowledgeNewCatagoryId
            };
            var knowledgeNewsVm = new KnowledgeNewsVm()
            {
                KnowledgeNewsId = KnowledgeNewsId,
                KnowledgeNewsName = knowledgeNews.KnowledgeNewsName,
                Description = knowledgeNews.Description,
                DateCreated = knowledgeNews.DateCreated,
                DateModified = knowledgeNews.DateModified,
                Thumnail = knowledgeNews.Thumnail,
                Active = knowledgeNews.Active,
                KnowledgeNewCatagoryVm = knowledgeNewsCategoryVm,
                Writer = writer
            };
            return new ApiSuccessResult<KnowledgeNewsVm>(knowledgeNewsVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateKnowledgeNews(UpdateKnowledgeNewsRequest request)
        {
            var knowledgeNews = await _context.KnowledgeNews.FindAsync(request.KnowledgeNewsId);
            if (knowledgeNews == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy kiến thức tin tức ");
            }
            var knowledgeNewCategory = await _context.KnowledgeNewCatagories.FindAsync(request.KnowledgeNewCatagoryId);
            if (knowledgeNewCategory == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy danh mục tin tức ");
            }
            var writer = await _userManager.FindByIdAsync(request.WriterId.ToString());
            if (writer == null)
            {
                return new ApiErrorResult<bool>("Tin tức không tìm thấy người viết");
            }
            if (string.IsNullOrWhiteSpace(request.KnowledgeNewsName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên tin tức");
            }

            knowledgeNews.KnowledgeNewsName = request.KnowledgeNewsName;
            if (request.Thumnail != null)
            {
                knowledgeNews.Thumnail = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Thumnail);
            }            
            knowledgeNews.Description = !string.IsNullOrWhiteSpace(request.Description) ? request.Description : "";
            knowledgeNews.Active = request.Active;
            knowledgeNews.DateModified = DateTime.Now;
            knowledgeNews.KnowledgeNewCatagory = knowledgeNewCategory;
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<KnowledgeNewsVm>>> ViewKnowledgeNews(ViewKnowledgeNewsRequest request)
        {
            var listKnowledgeNews = await _context.KnowledgeNews.ToListAsync();
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                listKnowledgeNews = listKnowledgeNews.Where(x => x.KnowledgeNewsName.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                || x.DateCreated.ToString().Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                || x.Writer.Fullname.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();

            }
            listKnowledgeNews = listKnowledgeNews.OrderByDescending(x => x.DateModified).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listKnowledgeNews.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listKnowledgeNewslVm = new List<KnowledgeNewsVm>();
            foreach (var x in listPaging)
            {
                var user = await _userManager.FindByIdAsync(x.WriterId.ToString());
                var writer = new StaffVm()
                {
                    StaffId = user.Id,
                    FullName = user.Fullname,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Dob = (DateTime)(user.Dob ?? DateTime.MinValue),
                    Status = user.Status,
                    CitizenIDCard = user.CitizenIDCard,
                    Address = user.Address,
                    Image = user.Image,
                };
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count > 0)
                {
                    writer.ListRoleName = new List<string>();
                    foreach (var role in roles)
                    {
                        writer.ListRoleName.Add(role);
                    }
                }
                var knowledgeCategory = await _context.KnowledgeNewCatagories.FindAsync(x.KnowledgeNewCatagoryId);
                var knowledgeNewsCategoryVm = new KnowledgeNewsCategoryVm
                {
                    Description = knowledgeCategory.Description,
                    KnowledgeNewCatagoriesName = knowledgeCategory.KnowledgeNewCatagoriesName,
                    KnowledgeNewCatagoryId = knowledgeCategory.KnowledgeNewCatagoryId
                };

                var knowledgeNewsVm = new KnowledgeNewsVm()
                {
                    Active = x.Active,
                    DateCreated = x.DateCreated,
                    DateModified = x.DateCreated,
                    Description = x.Description,
                    KnowledgeNewsId = x.KnowledgeNewsId,
                    KnowledgeNewsName = x.KnowledgeNewsName,
                    Thumnail = x.Thumnail,
                    KnowledgeNewCatagoryVm = knowledgeNewsCategoryVm,
                    Writer = writer
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

