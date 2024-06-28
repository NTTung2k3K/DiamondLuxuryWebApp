using DiamondLuxurySolution.Application.Repository.Gem;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNews;
using DiamondLuxurySolution.ViewModel.Models.KnowledgeNewsCategory;
using DiamondLuxurySolution.ViewModel.Models.News;
using DiamondLuxurySolution.ViewModel.Models.Slide;
using DiamondLuxurySolution.ViewModel.Models.User.Staff;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.News
{
    public class NewsRepo : INewsRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        private readonly UserManager<AppUser> _userManager;
        public NewsRepo(LuxuryDiamondShopContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ApiResult<bool>> CreateNews(CreateNewsRequest request)
        {
            var writer = await _userManager.FindByIdAsync(request.WriterId.ToString());
            if (writer == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy người viết");
            }
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.NewName))
            {
                errorList.Add("Vui lòng nhập tên tin tức");
            }

            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }

            var news = new DiamondLuxurySolution.Data.Entities.News
            {
                NewName = request.NewName,
                Title = request.Title != null ? request.Title : "",
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Description = request.Description != null ? request.Description : "",
                Status = request.Status,
                Id = (Guid)writer.Id,
                Writer = writer,
            };
            if (request.Image != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Image);
                news.Image = firebaseUrl;
            }
            else
            {
                news.Image = "";
            }

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> UpdateNews(UpdateNewsRequest request)
        {
            var news = await _context.News.FindAsync(request.NewsId);
            if (news == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy tin tức");
            }
            var writer = await _userManager.FindByIdAsync(request.WriterId.ToString());
            if (writer == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy người viết");
            }
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.NewName))
            {
                errorList.Add("Vui lòng nhập tên tin tức");
            }

            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }

            news.Title = request.Title;
            news.DateModified = DateTime.Now;
            if(request.Image != null && request.Image.Length > 0)
            {
                var firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Image);
                news.Image = firebaseUrl;
            }

            news.Description = !string.IsNullOrWhiteSpace(request.Description) ? request.Description : "";
            news.Status = request.Status;
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> DeleteNews(DeleteNewsRequest request)
        {
            var news = await _context.News.FindAsync(request.NewsId);
            if (news == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy tin tức");
            }
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<NewsVm>> GetNewsById(int NewsId)
        {
            var news = await _context.News.FindAsync(NewsId);
            if (news == null)
            {
                return new ApiErrorResult<NewsVm>("Không tìm thấy tin tức");
            }
            var user = await _userManager.FindByIdAsync(news.Id.ToString());
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
            var newsVm = new NewsVm()
            {
                NewsId = NewsId,
                NewName = news.NewName,
                DateCreated = news.DateCreated,
                DateModified = news.DateModified,
                Description = news.Description,
                Image = news.Image,
                Title = news.Title,
                Status = news.Status,
                Writer = writer
            };
            return new ApiSuccessResult<NewsVm>(newsVm, "Success");
        }


        public async Task<ApiResult<PageResult<NewsVm>>> ViewNews(ViewNewsRequest request)
        {

            var listNews = _context.News.AsQueryable();
            if (request.Keyword != null)
            {
                listNews = listNews.Where(x => x.NewName.Contains(request.Keyword) || x.Title.Contains(request.Keyword));
            }
            listNews = listNews.OrderByDescending(x => x.NewName);


            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listNews.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listNewsVm = new List<NewsVm>();
            foreach (var item in listPaging)
            {
                var user = await _userManager.FindByIdAsync(item.Id.ToString());
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
                var newsVm = new NewsVm()
                {
                    NewsId = item.NewsId,
                    NewName = item.NewName,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified,
                    Description = item.Description,
                    Title = item.Title,
                    Image = item.Image,
                    Status = item.Status,
                    Writer = writer
                };
                listNewsVm.Add(newsVm);
            }
            listNewsVm = listNewsVm.OrderByDescending(x => x.DateModified).ToList();
            var listResult = new PageResult<NewsVm>()
            {
                Items = listNewsVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listNews.Count(),
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<NewsVm>>(listResult, "Success");
        }

        public async Task<ApiResult<int>> CountAllNews()
        {
            var allNews = _context.News.Count();
            var allKnowledgeNews = _context.KnowledgeNews.Count();
            return new ApiSuccessResult<int>(allNews + allKnowledgeNews, "Success");
        }


        //getAlll
        public async Task<ApiResult<List<NewsVm>>> GetAll()
        {
            var listKNewslVm = new List<NewsVm>();
            var News = await _context.News.ToListAsync();
            foreach (var x in News)
            {
                var user = await _userManager.FindByIdAsync(x.Id.ToString());
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

                var NewsVm = new NewsVm()
                {
                   NewName=x.NewName,
                   Description=x.Description,
                   DateModified=x.DateModified,
                   DateCreated=x.DateCreated,
                   Image=x.Image,
                   NewsId=x.NewsId,
                   Status=x.Status,
                   Title=x.Title,
                    Writer = writer
                };
                listKNewslVm.Add(NewsVm);
            }
            return new ApiSuccessResult<List<NewsVm>>(listKNewslVm);
        }
    }
}

