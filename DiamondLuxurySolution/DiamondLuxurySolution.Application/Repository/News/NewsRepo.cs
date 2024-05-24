using DiamondLuxurySolution.Application.Repository.Gem;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.News;
using DiamondLuxurySolution.ViewModel.Models.Slide;
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
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.NewName.Trim()))
            {
                errorList.Add("Vui lòng nhập tên tin tức");
            }
            if (string.IsNullOrEmpty(request.Title.Trim()))
            {
                errorList.Add("Vui lòng nhập tiêu đề");
            }
            if (request.Image == null)
            {
                errorList.Add("Vui lòng chèn hình ảnh của tin tức");
            }
            var writer = await _userManager.FindByIdAsync(request.WriterId.ToString());
            if (writer == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy người viết");
            }
            if(errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ");
            }
            await _context.SaveChangesAsync();

            string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Image);
            var news = new DiamondLuxurySolution.Data.Entities.News
            {
                NewName = request.NewName,
                Title = request.Title,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Image = firebaseUrl,
                Description = request.Description,
                IsOutstanding = request.IsOutstanding,
                Id = writer.Id,
                Writer = writer
            };
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
            if (!string.IsNullOrEmpty(request.NewName?.Trim()))
            {
                news.NewName = request.NewName;
            }
            if (!string.IsNullOrEmpty(request.Title?.Trim()))
            {
                news.Title = request.Title;
            }
            news.DateModified = DateTime.Now;
            if (request.Image != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Image);
                news.Image = firebaseUrl;
            }
            if (!string.IsNullOrEmpty(request.Description?.Trim()))
            {
                news.Description = request.Description;
            }

            news.IsOutstanding = request.IsOutstanding;
            if (news.Id != request.WriterId)
            {
                var writer = await _userManager.FindByIdAsync(request.WriterId.ToString());
                if (writer == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy người viết");
                }
                news.Id = writer.Id;
                news.Writer = writer;
            }
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
            var writer = await _userManager.FindByIdAsync(news.Id.ToString());

            var newsVm = new NewsVm()
            {
                NewsId = NewsId,
                NewName = news.NewName,
                DateCreated = news.DateCreated,
                DateModified = news.DateModified,
                Description = news.Description,
                Image = news.Image,
                Title = news.Title,
                IsOutstanding = news.IsOutstanding,
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
                var writer = await _userManager.FindByIdAsync(item.Id.ToString());
                var newsVm = new NewsVm()
                {
                    NewsId = item.NewsId,
                    NewName = item.NewName,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified,
                    Description = item.Description,
                    Title = item.Title,
                    Image = item.Image,
                    IsOutstanding = item.IsOutstanding,
                    Writer = writer
                };
                listNewsVm.Add(newsVm);
            }

            var listResult = new PageResult<NewsVm>()
            {
                Items = listNewsVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listNews.Count(),
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<NewsVm>>(listResult, "Success");
        }
    }
}

