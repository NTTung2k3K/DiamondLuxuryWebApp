using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.About;
using DiamondLuxurySolution.ViewModel.Models.GemPriceList;
using DiamondLuxurySolution.ViewModel.Models.Material;
using DiamondLuxurySolution.ViewModel.Models.MaterialPriceList;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.MaterialPriceList
{
    public class MaterialPriceListRepo : IMaterialPriceListRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public MaterialPriceListRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateMaterialPriceList(CreateMaterialPriceListRequest request)
        {
            var errorList = new List<string>();
            if (request.BuyPrice <= 0)
            {
                errorList.Add("Vui lòng nhập giá mua phải lớn hơn 0");
            }
            if (request.SellPrice <= request.BuyPrice)
            {
                errorList.Add("Vui lòng nhập giá bán phải lớn hơn giá mua");
            }
            if (request.effectDate < DateTime.Today.AddDays(-3) || request.effectDate > DateTime.Today)
            {
                errorList.Add("Bảng giá nguyên liệu phải được cập nhật trong khoảng thời gian gần đây.");
            }


            var material = await _context.Materials.FindAsync(request.MaterialId);
            if (material == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy nguyên liệu");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            var materialPriceList = new DiamondLuxurySolution.Data.Entities.MaterialPriceList
            {
                MaterialId = request.MaterialId,
                BuyPrice = request.BuyPrice,
                SellPrice = request.SellPrice,
                effectDate = request.effectDate,
                Active = request.Active,
                Material = material
            };
            _context.MaterialPriceLists.Add(materialPriceList);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> DeleteMaterialPriceList(DeleteMaterialPriceListRequest request)
        {
            var materialPriceList = await _context.MaterialPriceLists.FindAsync(request.MaterialPriceListId);
            if (materialPriceList == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy nguyên liệu");
            }
            _context.MaterialPriceLists.Remove(materialPriceList);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<MaterialPriceListVm>> GetMaterialPriceListById(Guid MaterialId)
        {
            var materialPriceList = await _context.MaterialPriceLists.FindAsync(MaterialId);
            if (materialPriceList == null)
            {
                return new ApiErrorResult<MaterialPriceListVm>("Không tìm thấy nguyên liệu");
            }
            var material = await _context.Materials.FindAsync(materialPriceList.MaterialId.ToString());

            var materialPriceListVm = new MaterialPriceListVm()
            {
                MaterialId = MaterialId,
                BuyPrice = materialPriceList.BuyPrice,
                SellPrice = materialPriceList.SellPrice,
                Active = materialPriceList.Active,
                effectDate = materialPriceList.effectDate,
                MaterialVm = material
            };
            return new ApiSuccessResult<MaterialPriceListVm>(materialPriceListVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateMaterialPriceList(UpdateMaterialPriceListRequest request)
        {
            var materialPL = await _context.MaterialPriceLists.FindAsync(request.MaterialPriceListId);

            if (materialPL == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy nguyên liệu");
            }
            var errorList = new List<string>();
            if (request.BuyPrice <= 0)
            {
                errorList.Add("Vui lòng nhập giá mua phải lớn hơn 0");
            }
            if (request.SellPrice <= materialPL.BuyPrice)
            {
                errorList.Add("Vui lòng nhập giá bán phải lớn hơn hoặc bằng giá mua");
            }
            if (request.effectDate < DateTime.Today.AddDays(-3) || request.effectDate > DateTime.Today)
            {
                errorList.Add("Bảng giá nguyên liệu phải được cập nhật trong khoảng thời gian gần đây.");
            }

            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            materialPL.BuyPrice = request.BuyPrice;
            materialPL.SellPrice = request.SellPrice;
            materialPL.effectDate = request.effectDate;
            materialPL.Active = request.Active;
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<MaterialPriceListVm>>> ViewMaterialPriceListInCustomer(ViewMaterialPriceListRequest request)
        {
            var listMaterialPL = await _context.MaterialPriceLists.ToListAsync();

            if (request.Keyword != null)
            {
                listMaterialPL = listMaterialPL.Where(x => x.effectDate.ToString().Contains(request.Keyword)).ToList();
            }
            listMaterialPL = listMaterialPL.Where(x => x.Active == true).OrderByDescending(x => x.effectDate.ToString()).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listMaterialPL.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();
            var listMaterialVm = new List<MaterialPriceListVm>();
            foreach (var x in listPaging)
            {
                var material = await _context.Materials.FindAsync(x.MaterialId);
                var materialPriceListVm = new MaterialPriceListVm()
                {
                    MaterialId = x.MaterialId,
                    BuyPrice = x.BuyPrice,
                    SellPrice = x.SellPrice,
                    effectDate = x.effectDate,
                    Active = x.Active,
                    MaterialVm = material
                };
                listMaterialVm.Add(materialPriceListVm);
            }
            var listResult = new PageResult<MaterialPriceListVm>()
            {
                Items = listMaterialVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listMaterialVm.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<MaterialPriceListVm>>(listResult, "Success");
        }

        public async Task<ApiResult<PageResult<MaterialPriceListVm>>> ViewMaterialPriceListInManager(ViewMaterialPriceListRequest request)
        {
            var listMaterialPL = await _context.MaterialPriceLists.ToListAsync();
            if (request.Keyword != null)
            {
                listMaterialPL = listMaterialPL.Where(x => x.effectDate.ToString().Contains(request.Keyword)).ToList();

            }
            listMaterialPL = listMaterialPL.OrderByDescending(x => x.effectDate).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listMaterialPL.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listMaterialVm = new List<MaterialPriceListVm>();
            foreach (var x in listPaging)
            {
                var material = await _context.Materials.FindAsync(x.MaterialId);
                var materialPriceListVm = new MaterialPriceListVm()
                {
                    MaterialId = x.MaterialId,
                    BuyPrice = x.BuyPrice,
                    SellPrice = x.SellPrice,
                    effectDate = x.effectDate,
                    Active = x.Active,
                    MaterialVm = material
                };
                listMaterialVm.Add(materialPriceListVm);
            }
            var listResult = new PageResult<MaterialPriceListVm>()
            {
                Items = listMaterialVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listMaterialPL.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<MaterialPriceListVm>>(listResult, "Success");
        }
    }
}
