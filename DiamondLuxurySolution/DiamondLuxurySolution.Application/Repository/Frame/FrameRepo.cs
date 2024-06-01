using Azure.Core;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Frame;
using DiamondLuxurySolution.ViewModel.Models.GemPriceList;
using DiamondLuxurySolution.ViewModel.Models.Material;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Frame
{
    public class FrameRepo : IFrameRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public FrameRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateFrame(CreateFrameRequest request)
        {
            var material = await _context.Materials.FindAsync(request.MaterialId);
            if (material == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy vật liệu");
            }

            var errorList = new List<string>();
            if (string.IsNullOrWhiteSpace(request.NameFrame))
            {
                errorList.Add("Vui lòng nhập tên khung");
            }

            double weight = 0, size = 0;
            try
            {
                weight = Convert.ToDouble(request.Weight);

                if (weight <= 0)
                {
                    errorList.Add("Trọng lượng khung phải lớn > 0");
                }
            }
            catch (FormatException)
            {
                errorList.Add("Trọng lượng không hợp lệ");
            }

            try
            {
                size = Convert.ToDouble(request.Size);

                if (weight <= 0)
                {
                    errorList.Add("Kích thước khung phải lớn > 0");
                }
            }
            catch (FormatException)
            {
                errorList.Add("Kích thước khung không hợp lệ");
            }

            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            var frame = new Data.Entities.Frame
            {
                FrameId = await GenerateUniqueIdAsync(),
                FrameName = request.NameFrame.Trim(),
                Size = size,
                Weight = weight,
                Material = material,
            };

            _context.Frames.Add(frame);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }
        public async Task<string> GenerateUniqueIdAsync()
        {
            string newId;
            bool exists;
            Random rd = new Random();
            do
            {
                newId = "F" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) +
                    rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                exists = await _context.Frames.AnyAsync(x => x.FrameId == newId);
            } while (exists);
            return newId;
        }
        public string GenerateFrameIdRandom()
        {
            Random random = new Random();
            bool check = true;
            do
            {
                string result = "F" + random.Next(9) + random.Next(9) + random.Next(9) + random.Next(9) + random.Next(9) + random.Next(9) + random.Next(9) + random.Next(9) + random.Next(9);

            } while (check);
            return "";
        }

        public async Task<ApiResult<bool>> DeleteFrame(DeleteFrameRequest request)
        {
            var frame = await _context.Frames.FindAsync(request.FrameId);
            if (frame == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy khung");
            }
            _context.Frames.Remove(frame);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<FrameVm>> GetFrameById(string FrameId)
        {
            var frame = await _context.Frames.FindAsync(FrameId);
            if (frame == null)
            {
                return new ApiErrorResult<FrameVm>("Không tìm thấy khung");
            }
            var material = await _context.Materials.FindAsync(frame.MaterialId);
            var materialVm = new MaterialVm
            {
                Color = material.Color,
                Description = material.Description,
                MaterialId = material.MaterialId,
                MaterialImage = material.MaterialImage,
                MaterialName = material.MaterialName,
                Status = material.Status,
            };
            var frameVm = new FrameVm
            {
                Size = frame.Size,
                Weight = frame.Weight,
                NameFrame = frame.FrameName,
                MaterialVm = materialVm,
                FrameId = FrameId,
            };
            return new ApiSuccessResult<FrameVm>(frameVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateFrame(UpdateFrameRequest request)
        {
            var frame = await _context.Frames.FindAsync(request.FrameId);
            if (frame == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy khung");
            }

            var material = await _context.Materials.FindAsync(request.MaterialId);
            if (material == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy khung");
            }

            var errorList = new List<string>();
            if (string.IsNullOrWhiteSpace(request.Size))
            {
                errorList.Add("Vui lòng nhập kích thước");
            }
            if (string.IsNullOrWhiteSpace(request.Weight))
            {
                errorList.Add("Vui lòng nhập trọng lượng");
            }
            if (string.IsNullOrWhiteSpace(request.NameFrame))
            {
                errorList.Add("Vui lòng nhập tên khung");
            }
            double size = 0, weight = 0;
            try
            {
                size = Convert.ToDouble(request.Size);

                if (size <= 0)
                {
                    errorList.Add("Kích thước phải > 0");
                }
            }
            catch (FormatException)
            {
                errorList.Add("Kích thước không hợp lệ");
            }

            try
            {
                weight = Convert.ToDouble(request.Weight);

                if (weight <= 0)
                {
                    errorList.Add("Trọng lượng phải > 0");
                }
            }
            catch (FormatException)
            {
                errorList.Add("Trọng lượng không hợp lệ");
            }


            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }

            frame.Weight = weight;
            frame.Size = size;
            frame.FrameName = request.NameFrame.Trim();

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<FrameVm>>> ViewFrameInPaging(ViewFrameRequest request)
        {
            var listFrame = await _context.Frames.ToListAsync();
            if (request.Keyword != null)
            {
                listFrame = listFrame.Where(x => x.FrameName.Contains(request.Keyword)).ToList();

            }
            listFrame = listFrame.OrderByDescending(x => x.FrameName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listFrame.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();
            var listFrameVm = new List<FrameVm>();
            foreach (var item in listPaging)
            {
                var material = await _context.Materials.FindAsync(item.MaterialId);
                var materialVm = new MaterialVm
                {
                    Color = material.Color,
                    Description = material.Description,
                    MaterialId = material.MaterialId,
                    MaterialImage = material.MaterialImage,
                    MaterialName = material.MaterialName,
                    Status = material.Status,
                };
                var frameVm = new FrameVm()
                {
                   FrameId = item.FrameId,
                   NameFrame = item.FrameName,
                   Size = item.Size,
                   Weight = item.Weight,
                   MaterialVm = materialVm
                };
                listFrameVm.Add(frameVm);
            }
            var listResult = new PageResult<FrameVm>()
            {
                Items = listFrameVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listFrame.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<FrameVm>>(listResult, "Success");
        }
    }
}
