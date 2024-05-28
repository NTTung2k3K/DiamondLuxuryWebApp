using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models;
using DiamondLuxurySolution.ViewModel.Models.Category;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Material;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                if (request.ProcessingPrice <= 0 )
                {
                    errorList.Add("Sản phẩm phải có giá gia công hợp lí");
                }
                if (request.CategoryId == null)
                {
                    errorList.Add("Sản phẩm cần phải có loại");
                }
                if (request.Quantity == null || request.Quantity <= 0)
                {
                    errorList.Add("Sản phẩm cần có số lượng ít nhất là 1");
                }
                if (errorList.Any())
                {
                    return new ApiErrorResult<bool>("Lỗi thông tin", errorList);
                }
                Random rd = new Random();
                string ProductId = "P" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);




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
                        totalPriceGem += (decimal)item.Price;
                    }
                }
                // Process SubGemPrice
                decimal totalSubGemPrice = 0;
                if (request.ListSubGems != null && request.ListSubGems.Count > 0)
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
                // Process Image Of Product
                if (request.Images.Any() && request.Images.Count > 0)
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
                        _context.Images.Add(Image);
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
                    SellingCount = 0,
                    Quantity = request.Quantity,
                    PercentSale = request.PercentSale,
                    CategoryId = request.CategoryId,
                    Status = request.Status,
                    GemId = gem.GemId,
                    ProductPriceProcessing = request.ProcessingPrice
                };

                //Process Frame
                decimal totalFramePrice = 0;
                if (request.FrameId != null)
                {
                    var frame = await _context.Frames.FindAsync(request.FrameId);
                    if (frame == null)
                    {
                        return new ApiErrorResult<bool>("Không tìm thấy khung của sản phẩm");
                    }
                    product.FrameId = frame.FrameId;
                    var frameWeight = frame.Weight;
                    var material = await _context.Materials.FindAsync(frame.MaterialId);
                    if (material.EffectDate.Date.Equals(DateTime.Now.Date))
                    {
                        totalFramePrice = (decimal)material.Price * (decimal)frameWeight;
                    }
                    else
                    {
                        return new ApiErrorResult<bool>("Không tìm thấy giá cho nguyên liệu "+material.MaterialName);
                    }
                   
                }
                // Process totalPrice
                decimal OriginalPrice = (decimal)(totalPriceGem + totalSubGemPrice) + request.ProcessingPrice+ totalFramePrice; // + them cai processing price cua product
                double percent = (double)request.PercentSale / 100;
                decimal SellingPrice = OriginalPrice - (OriginalPrice * (decimal)percent);
                product.OriginalPrice = OriginalPrice;
                product.SellingPrice = SellingPrice;

                _context.Products.Add(product);

                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>(true, "Success");
            }
            catch (Exception e)
            {
                return new ApiErrorResult<bool>(e.Message);
            }
        }

        public async Task<ApiResult<bool>> DeleteProduct(DeleteProductRequest request)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy sản phẩm");
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<ProductVm>> GetProductById(string ProductId)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Images)
                    .Include(p => p.SubGemDetails)
                        .ThenInclude(sg => sg.SubGem)
                    .Include(p => p.Category)
                    .Include(p => p.Gem)
                    .ThenInclude(x => x.InspectionCertificate)
                    .Include(x => x.Frame)
                    .FirstOrDefaultAsync(p => p.ProductId == ProductId);

                if (product == null)
                {
                    return new ApiErrorResult<ProductVm>("Không tìm thấy sản phẩm");
                }


                var productVms = new ProductVm
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Description = product.Description,
                    ProductThumbnail = product.ProductThumbnail,
                    IsHome = product.IsHome,
                    IsSale = product.IsSale,
                    PercentSale = product.PercentSale,
                    Status = product.Status,
                    CategoryId = product.CategoryId,
                    CategoryVm = new CategoryVm
                    {
                        CategoryId = product.Category.CategoryId,
                        CategoryName = product.Category.CategoryName,
                        CategoryType = product.Category.CategoryType,
                        CategoryImage = product.Category.CategoryImage,
                        Status = product.Category.Status
                    },
                    Quantity = product.Quantity,
                    GemVm = new GemVm
                    {
                        GemId = product.Gem.GemId,
                        GemName = product.Gem.GemName,
                        GemImage = product.Gem.GemImage,
                        AcquisitionDate = product.Gem.AcquisitionDate,
                        IsOrigin = product.Gem.IsOrigin,
                        Active = product.Gem.Active,
                        Fluoresence = product.Gem.Fluoresence,
                        Polish = product.Gem.Polish,
                        ProportionImage = product.Gem.ProportionImage,
                        Symetry = product.Gem.Symetry,
                        InspectionCertificateVm = product.Gem.InspectionCertificate != null ? new InspectionCertificateVm()
                        {
                            InspectionCertificateId = product.Gem.InspectionCertificate.InspectionCertificateId,
                            DateGrading = product.Gem.InspectionCertificate.DateGrading,
                            InspectionCertificateName = product.Gem.InspectionCertificate.InspectionCertificateName,
                            Logo = product.Gem.InspectionCertificate.Logo,
                            Status = product.Gem.InspectionCertificate.Status
                        } : null,
                    },
                    ProcessingPrice = product.ProductPriceProcessing
                };
                if (product.Images != null)
                {
                    var imagePaths = product.Images.Select(x => x.ImagePath).ToList();
                    productVms.Images = imagePaths;
                }
                if (product.SubGemDetails != null)
                {
                    productVms.ListSubGems = product.SubGemDetails.Select(x => new SubGemSupportDTO
                    {
                        SubGemId = x.SubGemId,
                        Quantity = x.Quantity
                    }).ToList();
                }
                if (product.FrameId != null)
                {
              
                    productVms.FrameVm = new ViewModel.Models.Frame.FrameVm()
                    {
                        FrameId = product.FrameId,
                        NameFrame = product.Frame.FrameName,
                        Size = product.Frame.Size,
                        Weight = product.Frame.Weight,
                    };
                    var material = await _context.Materials.FindAsync(product.Frame.MaterialId);

                    productVms.MaterialVm = new MaterialVm()
                    {
                        MaterialId = material.MaterialId,
                        Color = material.Color,
                        Description = material.Description,
                        MaterialImage = material.MaterialImage,
                        MaterialName = material.MaterialName,
                        Status = material.Status,
                        Weight = material.Weight,

                    };
                   
                }
                return new ApiSuccessResult<ProductVm>(productVms);
            }
            catch (Exception e)
            {
                return new ApiErrorResult<ProductVm>(e.Message);
            }
        }


        public async Task<ApiResult<bool>> UpdateProduct(UpdateProductRequest request)
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

                if (errorList.Any())
                {
                    return new ApiErrorResult<bool>("Lỗi thông tin", errorList);
                }

                var product = await _context.Products.FindAsync(request.ProductId);
                if (product == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy sản phẩm");
                }

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
                        totalPriceGem += (decimal)item.Price;
                    }
                }
                product.GemId = gem.GemId;
                if (errorList.Any())
                {
                    return new ApiErrorResult<bool>("Lỗi tìm kiếm", errorList);
                }

                // Process SubGemPrice
                decimal totalSubGemPrice = 0;
                if (request.ListSubGems != null)
                {
                    var existingSubGems = _context.SubGemDetail.Where(x => x.ProductId == request.ProductId).ToList();

                    // Remove existing sub-gems if they are not in the new list
                    foreach (var existingSubGem in existingSubGems)
                    {
                        if (!request.ListSubGems.Any(x => x.SubGemId == existingSubGem.SubGemId))
                        {
                            _context.SubGemDetail.Remove(existingSubGem);
                        }
                    }

                    foreach (var subGemSupport in request.ListSubGems)
                    {
                        var subGem = await _context.SubGems.FindAsync(subGemSupport.SubGemId);
                        if (subGem == null)
                        {
                            return new ApiErrorResult<bool>("Không tìm thấy kim cương phụ");
                        }

                        var subGemDetail = existingSubGems.FirstOrDefault(x => x.SubGemId == subGemSupport.SubGemId);

                        if (subGemDetail == null)
                        {
                            subGemDetail = new SubGemDetail
                            {
                                ProductId = request.ProductId,
                                SubGemId = subGemSupport.SubGemId,
                                Quantity = subGemSupport.Quantity
                            };
                            _context.SubGemDetail.Add(subGemDetail);
                        }
                        else
                        {
                            subGemDetail.Quantity = subGemSupport.Quantity;
                        }

                        totalSubGemPrice += subGem.SubGemPrice * subGemSupport.Quantity;
                    }
                }
                else
                {
                    // If ListSubGems is null or empty, remove all existing sub-gems for the product
                    var existingSubGems = _context.SubGemDetail.Where(x => x.ProductId == request.ProductId).ToList();
                    _context.SubGemDetail.RemoveRange(existingSubGems);
                }

            
                // Process Image Of Product
                if (request.Images != null && request.Images.Any())
                {
                    var existingImages = _context.Images.Where(x => x.ProductId == request.ProductId).ToList();
                    _context.Images.RemoveRange(existingImages);

                    foreach (var item in request.Images)
                    {
                        string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(item);
                        var image = new Image
                        {
                            ImagePath = firebaseUrl,
                            Description = $"Hình ảnh của sản phẩm {request.ProductName}",
                            ProductId = request.ProductId
                        };
                        _context.Images.Add(image);
                    }
                }
                else
                {
                    var existingImages = _context.Images.Where(x => x.ProductId == request.ProductId).ToList();
                    _context.Images.RemoveRange(existingImages);
                }
           

                // Process Category
                var category = await _context.Categories.FindAsync(request.CategoryId);
                if (category == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy loại của sản phẩm");
                }
                product.CategoryId = category.CategoryId;
                //Process Frame
                decimal totalFramePrice = 0;
                if (request.FrameId != null)
                {
                    var frame = await _context.Frames.FindAsync(request.FrameId);
                    if (frame == null)
                    {
                        return new ApiErrorResult<bool>("Không tìm thấy khung của sản phẩm");
                    }
                    product.FrameId = frame.FrameId;
                    var frameWeight = frame.Weight;
                    var material = await _context.Materials.FindAsync(frame.MaterialId);
                    if (material.EffectDate.Date.Equals(DateTime.Now.Date))
                    {
                        totalFramePrice = (decimal)material.Price * (decimal)frameWeight;
                    }
                    else
                    {
                        return new ApiErrorResult<bool>("Không tìm thấy giá cho nguyên liệu " + material.MaterialName);
                    }

                }
                else
                {
                    product.FrameId = null;
                }
                // Process totalPrice
                decimal OriginalPrice = (decimal)(totalPriceGem + totalSubGemPrice) + request.ProcessingPrice + totalFramePrice; // + them cai processing price cua product
                double percent = (double)request.PercentSale / 100;
                decimal SellingPrice = OriginalPrice - (OriginalPrice * (decimal)percent);
                product.OriginalPrice = OriginalPrice;
                product.SellingPrice = SellingPrice;


                // Process Thumbnail
                string thumbnailUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.ProductThumbnail);
                product.ProductThumbnail = thumbnailUrl;
                // Update Product
                product.ProductName = request.ProductName;
                product.Description = string.IsNullOrEmpty(request.Description) ? string.Empty : request.Description;
                product.IsHome = request.IsHome;
                product.IsSale = request.IsSale;
                product.DateModified = DateTime.Now;
                product.OriginalPrice = OriginalPrice;
                product.SellingPrice = SellingPrice;
                product.PercentSale = request.PercentSale;
                product.Status = request.Status;
                product.ProductPriceProcessing = request.ProcessingPrice;

                _context.Products.Update(product);

                await _context.SaveChangesAsync();
                return new ApiSuccessResult<bool>(true, "Cập nhật sản phẩm thành công");
            }
            catch (Exception e)
            {
                return new ApiErrorResult<bool>(e.Message);
            }
        }

        public async Task<ApiResult<PageResult<ProductVm>>> ViewProduct(ViewProductRequest request)
        {

            var listProduct = await _context.Products
                .Include(p => p.Images)
                .Include(p => p.SubGemDetails)
                    .ThenInclude(sg => sg.SubGem)
                .Include(p => p.Category)
                .Include(p => p.Gem)
                .ThenInclude(x => x.InspectionCertificate)
                .Include(x => x.Frame)
                                        .ToListAsync();



            if (!string.IsNullOrEmpty(request.Keyword))
            {
                listProduct = listProduct.Where(x => x.ProductName.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                                   || x.Description.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                                   || x.Gem.GemName.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                                   || x.Category.CategoryName.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
                                                   || x.SubGemDetails.Any(sg => sg.SubGem.SubGemName.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase))
                                                   || x.SubGemDetails.Any(sg => sg.SubGem.Description.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase))).ToList();
            }

            var totalRecord = listProduct.Count();
            listProduct = listProduct.OrderBy(x => x.ProductName).ToList();

            int pageIndex = request.pageIndex ?? 1;
            int pageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE;
            var pagedProducts = listProduct.ToPagedList(pageIndex, pageSize).ToList();

            List<ProductVm> listResultVm = new List<ProductVm>();

            foreach (var product in pagedProducts)
            {
                var gem = product.Gem;
                var listSubGem = product.SubGemDetails.ToList();
                List<SubGemSupportDTO> listSubGemVm = listSubGem.Select(sg => new SubGemSupportDTO
                {
                    SubGemId = sg.SubGemId,
                    Quantity = sg.Quantity
                }).ToList();



                var productVms = new ProductVm
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Description = product.Description,
                    ProductThumbnail = product.ProductThumbnail,
                    IsHome = product.IsHome,
                    IsSale = product.IsSale,
                    PercentSale = product.PercentSale,
                    Status = product.Status,
                    CategoryId = product.CategoryId,
                    CategoryVm = new CategoryVm
                    {
                        CategoryId = product.Category.CategoryId,
                        CategoryName = product.Category.CategoryName,
                        CategoryType = product.Category.CategoryType,
                        CategoryImage = product.Category.CategoryImage,
                        Status = product.Category.Status
                    },
                    Quantity = product.Quantity,
                    GemVm = new GemVm
                    {
                        GemId = product.Gem.GemId,
                        GemName = product.Gem.GemName,
                        GemImage = product.Gem.GemImage,
                        AcquisitionDate = product.Gem.AcquisitionDate,
                        IsOrigin = product.Gem.IsOrigin,
                        Active = product.Gem.Active,
                        Fluoresence = product.Gem.Fluoresence,
                        Polish = product.Gem.Polish,
                        ProportionImage = product.Gem.ProportionImage,
                        Symetry = product.Gem.Symetry,
                        InspectionCertificateVm = product.Gem.InspectionCertificate != null ? new InspectionCertificateVm()
                        {
                            InspectionCertificateId = product.Gem.InspectionCertificate.InspectionCertificateId,
                            DateGrading = product.Gem.InspectionCertificate.DateGrading,
                            InspectionCertificateName = product.Gem.InspectionCertificate.InspectionCertificateName,
                            Logo = product.Gem.InspectionCertificate.Logo,
                            Status = product.Gem.InspectionCertificate.Status
                        } : null,
                    },
                    ProcessingPrice = product.ProductPriceProcessing
                };
                if (product.Images != null)
                {
                    var imagePaths = product.Images.Select(x => x.ImagePath).ToList();
                    productVms.Images = imagePaths;
                }
                if (product.SubGemDetails != null)
                {
                    productVms.ListSubGems = product.SubGemDetails.Select(x => new SubGemSupportDTO
                    {
                        SubGemId = x.SubGemId,
                        Quantity = x.Quantity
                    }).ToList();
                }
                if (product.FrameId != null)
                {

                    productVms.FrameVm = new ViewModel.Models.Frame.FrameVm()
                    {
                        FrameId = product.FrameId,
                        NameFrame = product.Frame.FrameName,
                        Size = product.Frame.Size,
                        Weight = product.Frame.Weight,
                    };
                    var material = await _context.Materials.FindAsync(product.Frame.MaterialId);

                    productVms.MaterialVm = new MaterialVm()
                    {
                        MaterialId = material.MaterialId,
                        Color = material.Color,
                        Description = material.Description,
                        MaterialImage = material.MaterialImage,
                        MaterialName = material.MaterialName,
                        Status = material.Status,
                        Weight = material.Weight,

                    };

                }

                listResultVm.Add(productVms);
            }

            var listResult = new PageResult<ProductVm>
            {
                Items = listResultVm.Distinct().ToList(),
                PageSize = pageSize,
                TotalRecords = totalRecord,
                PageIndex = pageIndex
            };

            return new ApiSuccessResult<PageResult<ProductVm>>(listResult, "Success");
        }



    }
}
