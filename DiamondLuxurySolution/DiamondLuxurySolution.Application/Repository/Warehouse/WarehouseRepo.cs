using Azure.Core;
using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Warehouse;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Warehouse
{
    public class WarehouseRepo : IWarehouseRepo
    {
        private readonly LuxuryDiamondShopContext _context;
        public WarehouseRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<bool>> CreateWarehouse(CreateWarehouseRequest request)
        {

            if (string.IsNullOrEmpty(request.WareHouseName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên kho");
            }
            var warehouse = new DiamondLuxurySolution.Data.Entities.WareHouse
            {
                WareHouseName = request.WareHouseName,
                Location = request.Location != null ? request.Location : "",
                Description = request.Description != null ? request.Description : ""
            };
            _context.WareHouses.Add(warehouse);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> DeleteWarehouse(DeleteWarehouseRequest request)
        {
            var warehouse = await _context.WareHouses.FindAsync(request.WareHouseId);
            if (warehouse == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy kho");
            }

            _context.WareHouses.Remove(warehouse);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<WarehouseVm>> GetWarehouseById(int WarehouseId)
        {
            var warehouse = await _context.WareHouses.FindAsync(WarehouseId);
            if (warehouse == null)
            {
                return new ApiErrorResult<WarehouseVm>("Không tìm thấy kho");
            }

            var warehouseVm = new WarehouseVm()
            {
                WarehouseId = warehouse.WareHouseId,
                WareHouseName = warehouse.WareHouseName,
                Description = warehouse.Description,
                Location = warehouse.Location
            };
            return new ApiSuccessResult<WarehouseVm>(warehouseVm, "Success");
        }

        public async Task<ApiResult<bool>> UpdateWarehouse(UpdateWarehouseRequest request)
        {
            if (string.IsNullOrEmpty(request.WareHouseName))
            {
                return new ApiErrorResult<bool>("Vui lòng nhập tên kho");
            }
           
            var warehouse = await _context.WareHouses.FindAsync(request.WareHouseId);
            if (warehouse == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy kho");
            }
            
            warehouse.WareHouseName = request.WareHouseName;
            warehouse.Description = request.Description!=null ? request.Description : "";
            warehouse.Location = request.Location != null ? request.Location : "";

            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<PageResult<WarehouseVm>>> ViewWarehouse(ViewWarehouseRequest request)
        {
            var listPlatform = await _context.WareHouses.ToListAsync();
            if (request.Keyword != null)
            {
                listPlatform = listPlatform.Where(x => x.WareHouseName.Contains(request.Keyword) || x.Location.Contains(request.Keyword) || x.Location.Contains(request.Keyword)).ToList();
            }
            listPlatform = listPlatform.OrderBy(x => x.WareHouseName).ToList();

            int pageIndex = request.pageIndex ?? 1;

            var listPaging = listPlatform.ToPagedList(pageIndex, DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE).ToList();

            var listWarehouseVm = listPaging.Select(x => new WarehouseVm()
            {
                WarehouseId = x.WareHouseId,
                WareHouseName = x.WareHouseName,
                Description = x.Description,
                Location = x.Location
            }).ToList();
            var listResult = new PageResult<WarehouseVm>()
            {
                Items = listWarehouseVm,
                PageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE,
                TotalRecords = listPlatform.Count,
                PageIndex = pageIndex
            };
            return new ApiSuccessResult<PageResult<WarehouseVm>>(listResult, "Success");
        }
    }
}
