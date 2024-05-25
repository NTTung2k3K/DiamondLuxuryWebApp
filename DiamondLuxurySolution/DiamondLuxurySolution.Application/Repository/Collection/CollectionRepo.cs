using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.Collection;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Collection
{
    public class CollectionRepo : ICollectionRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public CollectionRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateCollection(CreateCollectionRequest request)
        {
            if (string.IsNullOrEmpty(request.CollectionName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên bộ sưu tập");
            }

            var collection = new DiamondLuxurySolution.Data.Entities.Collection
            {
                CollectionId = await GenerateUniqueCollectionIdAsync(),
                CollectionName = request.CollectionName,
                Description = request.Description != null ? request.Description : "",
                Status = request.Status,
            };
            if (request.Thumbnail != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Thumbnail);
                collection.Thumbnail = firebaseUrl;
            }
            else
            {
                collection.Thumbnail = "";
            }

            _context.Collections.Add(collection);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<string> GenerateUniqueCollectionIdAsync()
        {
            string newId;
            bool exists;
            Random random = new Random();

            do
            {
                newId = "CL" + random.Next(1000, 10000).ToString(); 
                exists = await _context.Collections.AnyAsync(ic => ic.CollectionId == newId);
            } while (exists);

            return newId;
        }

        public async Task<ApiResult<bool>> DeleteCollection(DeleteCollectionRequest request)
        {
            var collection = await _context.Collections.FindAsync(request.CollectionId);
            if (collection == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy bộ sưu tập");
            }

            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<CollectionVm>> GetCollectionById(string CollectionId)
        {
            var collection = await _context.Collections.FindAsync(CollectionId);
            if (collection == null)
            {
                return new ApiErrorResult<CollectionVm>("Không tìm thấy bộ sưu tập");
            }
            var collectionVm = new CollectionVm()
            {
                CollectionId = CollectionId,
                CollectionName = collection.CollectionName,
                Description = collection.Description,
                Thumbnail = collection.Thumbnail,
                Status = collection.Status,
            };
            return new ApiSuccessResult<CollectionVm>(collectionVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateCollection(UpdateCollectionRequest request)
        {
            if (string.IsNullOrEmpty(request.CollectionName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên bộ sưu tập");
            }

            var collection = await _context.Collections.FindAsync(request.CollectionId);
            if (collection == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy bộ sưu tập");
            }
            if (request.Thumbnail != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Thumbnail);
                collection.Thumbnail = firebaseUrl;
            }
            else
            {
                collection.Thumbnail = "";
            }
            collection.CollectionName = request.CollectionName;
            collection.Description = request.Description != null ? request.Description : "";
            collection.Status = request.Status;

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<CollectionVm>>> ViewCollectionInCustomer(ViewCollectionRequest request)
        {
            var listCollection = await _context.Collections.ToListAsync();
            if (request.Keyword != null)
            {
                listCollection = listCollection.Where(x => x.CollectionName.Contains(request.Keyword)).ToList();

            }
            listCollection = listCollection.Where(x => x.Status).OrderByDescending(x => x.CollectionName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listCollection.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listCollectionVm = listPaging.Select(x => new CollectionVm()
            {
                CollectionId = x.CollectionId,
                CollectionName = x.CollectionName,
                Description = x.Description,
                Thumbnail = x.Thumbnail,
                Status = x.Status,
            }).ToList();
            var listResult = new PageResult<CollectionVm>()
            {
                Items = listCollectionVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listCollection.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<CollectionVm>>(listResult, "Success");
        }

        public async Task<ApiResult<PageResult<CollectionVm>>> ViewCollectionInManager(ViewCollectionRequest request)
        {
            var listCollection = await _context.Collections.ToListAsync();
            if (request.Keyword != null)
            {
                listCollection = listCollection.Where(x => x.CollectionName.Contains(request.Keyword)).ToList();

            }
            listCollection = listCollection.OrderByDescending(x => x.CollectionName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listCollection.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listCollectionVm = listPaging.Select(x => new CollectionVm()
            {
                CollectionId = x.CollectionId,
                CollectionName = x.CollectionName,
                Description = x.Description,
                Thumbnail = x.Thumbnail,
                Status = x.Status,
            }).ToList();
            var listResult = new PageResult<CollectionVm>()
            {
                Items = listCollectionVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listCollection.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<CollectionVm>>(listResult, "Success");
        }
    }
}
