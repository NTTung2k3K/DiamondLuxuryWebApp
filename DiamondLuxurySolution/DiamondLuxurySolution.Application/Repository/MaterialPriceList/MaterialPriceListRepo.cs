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
            var material = await _context.Materials.FindAsync(request.MaterialId);
            if (material == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy nguyên liệu");
            }

            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.BuyPrice) || string.IsNullOrEmpty(request.SellPrice))
            {
                errorList.Add("Vui lòng nhập giá mua và bán nguyên liệu");
            }

            decimal priceBuy = 0;
            decimal priceSell = 0;
            try
            {
                priceBuy = Convert.ToDecimal(request.BuyPrice);
                priceSell = Convert.ToDecimal(request.SellPrice);

                if (priceBuy <= 0 || priceSell <=0)
                {
                    errorList.Add("Giá mua và bán nguyên liệu > 0");
                }
            }
            catch (FormatException)
            {
                errorList.Add("Giá nguyên liệu không hợp lệ");
            }
            if (priceSell <= priceBuy)
            {
                errorList.Add("Vui lòng nhập giá bán phải lớn hơn giá mua");
            }
            if (request.effectDate < DateTime.Today.AddDays(-3) || request.effectDate > DateTime.Today)
            {
                errorList.Add("Bảng giá nguyên liệu phải được cập nhật trong khoảng thời gian gần đây.");
            }
            
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            var materialPriceList = new DiamondLuxurySolution.Data.Entities.MaterialPriceList
            {
                MaterialId = request.MaterialId,
                BuyPrice = priceBuy,
                SellPrice = priceSell,
                effectDate = request.effectDate,
                Active = request.Active,
                Material = material,
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

        public async Task<ApiResult<MaterialPriceListVm>> GetMaterialPriceListById(int MaterialPriceListId)
        {
            var materialPriceList = await _context.MaterialPriceLists.FindAsync(MaterialPriceListId);
            if (materialPriceList == null)
            {
                return new ApiErrorResult<MaterialPriceListVm>("Không tìm thấy nguyên liệu");
            }
            var material = await _context.Materials.FindAsync(materialPriceList.MaterialId);

            var materialPriceListVm = new MaterialPriceListVm()
            {
                MaterialPriceListId = MaterialPriceListId,
                BuyPrice = (decimal)materialPriceList.BuyPrice,
                SellPrice = (decimal)materialPriceList.SellPrice,
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
            if (string.IsNullOrEmpty(request.BuyPrice) || string.IsNullOrEmpty(request.SellPrice))
            {
                errorList.Add("Vui lòng nhập giá mua và bán nguyên liệu");
            }

            decimal priceBuy = 0;
            decimal priceSell = 0;
            try
            {
                priceBuy = Convert.ToDecimal(request.BuyPrice);
                priceSell = Convert.ToDecimal(request.SellPrice);

                if (priceBuy <= 0 || priceSell <= 0)
                {
                    errorList.Add("Giá mua và bán nguyên liệu > 0");
                }
            }
            catch (FormatException)
            {
                errorList.Add("Giá nguyên liệu không hợp lệ");
            }
            if (priceSell <= priceBuy)
            {
                errorList.Add("Vui lòng nhập giá bán phải lớn hơn giá mua");
            }
            if (request.effectDate < DateTime.Today.AddDays(-3) || request.effectDate > DateTime.Today)
            {
                errorList.Add("Bảng giá nguyên liệu phải được cập nhật trong khoảng thời gian gần đây.");
            }

            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            materialPL.BuyPrice = priceBuy;
            materialPL.SellPrice = priceSell;
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
            var listMaterialPriceList = new List<MaterialPriceListVm>();
            foreach (var x in listPaging)
            {
                var material = await _context.Materials.FindAsync(x.MaterialId);
                var materialPriceListVm = new MaterialPriceListVm()
                {
                    MaterialPriceListId = x.MaterialPriceListId,
                    BuyPrice = (decimal)x.BuyPrice,
                    SellPrice = (decimal)x.SellPrice,
                    effectDate = x.effectDate,
                    Active = x.Active,
                    MaterialVm = material,
                };
                listMaterialPriceList.Add(materialPriceListVm);
            }
            var listResult = new PageResult<MaterialPriceListVm>()
            {
                Items = listMaterialPriceList,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listMaterialPL.Count,
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
            listMaterialPL = listMaterialPL.OrderByDescending(x => x.effectDate.ToString()).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listMaterialPL.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();
            var listMaterialPriceList = new List<MaterialPriceListVm>();
            foreach (var x in listPaging)
            {
                var material = await _context.Materials.FindAsync(x.MaterialId);
                var materialPriceListVm = new MaterialPriceListVm()
                {
                    MaterialPriceListId = x.MaterialPriceListId,
                    BuyPrice = (decimal)x.BuyPrice,
                    SellPrice = (decimal)x.SellPrice,
                    effectDate = x.effectDate,
                    Active = x.Active,
                    MaterialVm = material,
                };
                listMaterialPriceList.Add(materialPriceListVm);
            }
            var listResult = new PageResult<MaterialPriceListVm>()
            {
                Items = listMaterialPriceList,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listMaterialPL.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<MaterialPriceListVm>>(listResult, "Success");
        }
    }
}
