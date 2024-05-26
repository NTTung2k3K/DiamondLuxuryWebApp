using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.Material;
using DiamondLuxurySolution.ViewModel.Models.Product;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Product
{
    public class ProductRepo : IProductRepo
    {

        private readonly LuxuryDiamondShopContext _context;
        private readonly UserManager<AppUser> _userManager;
        public ProductRepo(LuxuryDiamondShopContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<ApiResult<bool>> CreateProduct(CreateProductRequest request)
        {
            try
            {

                if (string.IsNullOrEmpty(request.ProductName))
                {
                    return new ApiErrorResult<bool>("Vui lòng nhập tên sản phẩm");
                }
                if (request.GemId == null)
                {
                    return new ApiErrorResult<bool>("Sản phẩm cần phải có kim cương");
                }
                List<string> errorList = new List<string>();
                if (request.ProductThumbnail == null)
                {
                    errorList.Add("Sản phẩm phải có ảnh đại diện");
                }
                if (request.ProcessingPrice <= 0)
                {
                    errorList.Add("Sản phẩm phải có giá hợp lí");
                }

                if (request.WareHouseId == null)
                {
                    errorList.Add("Sản phẩm cần phải có kho lưu trữ");
                }
                
                if (errorList.Any())
                {
                    return new ApiErrorResult<bool>("Lỗi thông tin", errorList);
                }
                Random rd = new Random();
                string ProductId = "P" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                // Process Product gem
                decimal totalPriceGem = 0;

                var gem = await _context.Gems.FindAsync(request.GemId);
                if (gem == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy kim cương");
                }

                var GemPriceList = _context.GemPriceLists.Where(x => x.GemId == request.GemId);
                if (GemPriceList == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy kim cương trong bảng giá kim cương");
                }
                foreach (var item in GemPriceList)
                {
                    if (item.effectDate.Date == DateTime.Now.Date)
                    {
                        totalPriceGem += item.Price;
                    }
                }
                if (errorList.Any())
                {
                    return new ApiErrorResult<bool>("Lỗi tìm kiếm", errorList);
                }
                // Process Material
                decimal totalMaterialPrice = 0;
                if (request.MaterialId != null)
                {
                    var materialList = _context.MaterialPriceLists.Where(x => x.MaterialId == request.MaterialId);
                    if (materialList == null)
                    {
                        return new ApiErrorResult<bool>($"Không tìm nguyên liệu");
                    }
                    foreach (var item in materialList)
                    {
                        if (item.effectDate.Date == DateTime.Now.Date)
                        {
                            totalMaterialPrice += item.SellPrice;
                        }
                    }
                    if (errorList.Any())
                    {
                        return new ApiErrorResult<bool>("Lỗi tìm kiếm", errorList);
                    }
                }
                // Process SubGemPrice
                decimal totalSubGemPrice = 0;
                if (request.ListSubGems != null)
                {
                    foreach (var subGemSupport in request.ListSubGems)
                    {
                        var subGem = await _context.SubGems.FindAsync(subGemSupport.SubGemId);
                        if (subGem == null)
                        {
                            return new ApiErrorResult<bool>($"Không tìm thấy kim cương phụ");
                        }
                        var subGemDetail = new SubGemDetail()
                        {
                            ProductId = ProductId,
                            SubGemId = subGemSupport.SubGemId,
                            Quantity = subGemSupport.Quantity,
                        };
                        totalSubGemPrice += subGem.SubGemPrice * subGemSupport.Quantity;
                        _context.SubGemDetail.Add(subGemDetail);
                    }
                }


                // Process totalPrice
                decimal OriginalPrice = totalPriceGem + totalMaterialPrice + request.ProcessingPrice + totalSubGemPrice;
                decimal SellingPrice = OriginalPrice - (OriginalPrice * request.PercentSale);
                // Process Image Of Product
                if (request.Images.Any())
                {
                    foreach (var item in request.Images)
                    {
                        string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(item);
                        var Image = new Image()
                        {
                            ImagePath = firebaseUrl,
                            Description = $"Hình ảnh của sản phẩm {request.ProductName}",
                            ProductId = ProductId
                        };

                    }
                }
                // Process Category
                var category = await _context.Categories.FindAsync(request.CategoryId);
                if (category == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy loại của sản phẩm");
                }
                // Process Thumbnail
                string thumbnailUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.ProductThumbnail);


                // Save Product
                var product = new DiamondLuxurySolution.Data.Entities.Product()
                {
                    ProductId = ProductId,
                    ProductName = request.ProductName,
                    Description = string.IsNullOrEmpty(request.Description) ? string.Empty : request.Description,
                    ProductThumbnail = thumbnailUrl,
                    IsHome = request.IsHome,
                    IsSale = request.IsSale,
                    DateCreate = DateTime.Now,
                    DateModified = DateTime.Now,
                    OriginalPrice = OriginalPrice,
                    SellingPrice = SellingPrice,
                    SellingCount = 0,
                    PercentSale = request.PercentSale,
                    CategoryId = request.CategoryId,
                    InspectionCertificateId = string.IsNullOrEmpty(request.InspectionCertificateId) ? null : request.InspectionCertificateId,
                    MaterialId = request.MaterialId != null ? request.MaterialId : Guid.Empty,
                    Status = request.Status,
                    WarehouseId = request.WareHouseId
                };
                _context.Products.Add(product);

                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>(true, "Success");
            }catch(Exception e)
            {
                return new ApiErrorResult<bool>(e.Message);
            }
        }

        public Task<ApiResult<bool>> DeleteProduct(DeleteProductRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<ProductVm>> GetProductById(int ProductId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdateProduct(UpdateProductRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<PageResult<ProductVm>>> ViewProduct(ViewProductRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
