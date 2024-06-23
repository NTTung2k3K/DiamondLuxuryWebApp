using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models;
using DiamondLuxurySolution.ViewModel.Models.Category;
using DiamondLuxurySolution.ViewModel.Models.Frame;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.GemPriceList;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Material;
using DiamondLuxurySolution.ViewModel.Models.Order;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
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
                if (request.ProcessingPrice <= 0)
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
                if (request.ListSubGems != null)
                {
                    if (HasDuplicates(request.ListSubGems))
                    {
                        return new ApiErrorResult<bool>("Kim cương phụ bị trùng, vui lòng chọn lại");
                    }
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

                var GemPriceList = await _context.GemPriceLists.FindAsync(gem.GemPriceListId);
                if (GemPriceList == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy kim cương trong bảng giá kim cương");
                }
                if (GemPriceList.effectDate.Date <= DateTime.Now.Date && DateTime.Now.Date <= GemPriceList.effectDate.Date.AddDays(7))
                {
                    totalPriceGem += (decimal)GemPriceList.Price;
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
                        if (subGemSupport.Quantity <= 0)
                        {
                            return new ApiErrorResult<bool>($"Kim cương phụ cần có số lượng");
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
                        product.Images.Add(Image);
                    }
                }
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
                    if (material.EffectDate.Date <= DateTime.Now.Date && DateTime.Now.Date <= material.EffectDate.Date.AddDays(3))
                    {
                        totalFramePrice = (decimal)material.Price * (decimal)frameWeight;
                    }
                    else
                    {
                        return new ApiErrorResult<bool>("Không tìm thấy giá cho nguyên liệu " + material.MaterialName);
                    }

                }
                // Process totalPrice
                decimal OriginalPrice = (decimal)(totalPriceGem + totalSubGemPrice) + request.ProcessingPrice + totalFramePrice; // + them cai processing price cua product
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
					ProcessingPrice = product.ProductPriceProcessing,
					OriginalPrice = product.OriginalPrice,
					SellingPrice = product.SellingPrice,
					DateModify = product.DateModified,
					QuantitySold = product.SellingCount,
					
				};
				if (product.Images != null)
				{
					var imagePaths = product.Images.Select(x => x.ImagePath).ToList();
					productVms.Images = imagePaths;
				}
				var gemPriceList = await _context.GemPriceLists.FindAsync(product.Gem.GemPriceListId);

				if (gemPriceList != null)
				{
					productVms.GemPriceLists = new ViewModel.Models.GemPriceList.GemPriceListVm()
					{
						Active = gemPriceList.Active,
						CaratWeight = gemPriceList.CaratWeight,
						Clarity = gemPriceList.Clarity,
						Color = gemPriceList.Color,
						Cut = gemPriceList.Cut,
						effectDate = gemPriceList.effectDate,
						GemPriceListId = gemPriceList.GemPriceListId,
						Price = (decimal)gemPriceList.Price,
					};
				}
				if (product.SubGemDetails != null)
				{
					productVms.ListSubGems = product.SubGemDetails.Select(x => new SubGemSupportDTO
					{
						SubGemId = x.SubGemId,
						SubGemName = x.SubGem.SubGemName,
						Quantity = x.Quantity
					}).ToList();

				}
				if (product.FrameId != null)
				{

					productVms.FrameVm = new ViewModel.Models.Frame.FrameVm()
					{
						FrameId = product.FrameId,
						NameFrame = product.Frame.FrameName,
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

					};

				}

				return new ApiSuccessResult<ProductVm>(productVms);
			}
			catch (Exception e)
			{
				return new ApiErrorResult<ProductVm>(e.Message);
			}
        }

        private bool HasDuplicates(ICollection<SubGemSupportDTO> list)
        {
            return list.GroupBy(x => x).Any(g => g.Count() > 1);
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


				if (errorList.Any())
				{
					return new ApiErrorResult<bool>("Lỗi thông tin", errorList);
				}
				if (request.ListSubGems != null)
				{
					if (HasDuplicates(request.ListSubGems))
					{
						return new ApiErrorResult<bool>("Kim cương phụ bị trùng, vui lòng chọn lại");
					}
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

                var GemPriceList = await _context.GemPriceLists.FindAsync(gem.GemPriceListId);
                if (GemPriceList == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy kim cương trong bảng giá kim cương");
                }
                if (GemPriceList.effectDate.Date <= DateTime.Now.Date && DateTime.Now.Date <= GemPriceList.effectDate.Date.AddDays(7))
                {
                    totalPriceGem += (decimal)GemPriceList.Price;
                }
                product.GemId = gem.GemId;
				if (errorList.Any())
				{
					return new ApiErrorResult<bool>("Lỗi tìm kiếm", errorList);
				}



				// Process Image Of Product
				var imagesOfProduct = await _context.Images.Where(x => x.ProductId == product.ProductId).ToListAsync();
				var existingHandleImages = imagesOfProduct.Select(img => img.ImagePath).ToList();
				var imagesToRemove = existingHandleImages.Except(request.ExistingImages).ToList();

				foreach (var image in imagesToRemove)
				{
					var imageToRemove = _context.Images.FirstOrDefault(img => img.ImagePath == image);
					if (imageToRemove != null)
					{
						_context.Images.Remove(imageToRemove);
					}
				}


				if (request.Images != null && request.Images.Any())
				{
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


				// Process Thumbnail
				if (request.ProductThumbnail != null)
				{
					string thumbnailUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.ProductThumbnail);
					product.ProductThumbnail = thumbnailUrl;
				}


				// Process SubGem

				var exitingSubgemDetail = await _context.SubGemDetail.Where(x => x.ProductId == product.ProductId)
					.Select(x => new SubGemSupportDTO()
					{
						Quantity = x.Quantity,
						SubGemId = x.SubGemId,
					})
					.ToListAsync();

				var difference = exitingSubgemDetail
					.Except(request.ExistingListSubGems, new SubGemSupportDTOComparer())
					.ToList();
				List<SubGemDetail> listRemove = new List<SubGemDetail>();
				foreach (var item in difference)
				{
					var removeObject = _context.SubGemDetail.Where(x => x.SubGemId == item.SubGemId && x.ProductId == product.ProductId);
					listRemove.AddRange(removeObject);
				}
				_context.SubGemDetail.RemoveRange(listRemove);
				if (difference.Count == 0)
				{
					foreach (var item in request.ExistingListSubGems)
					{
						var existingSubgem = _context.SubGemDetail.Where(x => x.SubGemId == item.SubGemId && x.ProductId == product.ProductId);
						existingSubgem.First().Quantity = item.Quantity;
					}
				}

				if (request.ListSubGems != null)
				{
					foreach (var subGemSupport in request.ListSubGems)
					{
						var subGem = await _context.SubGems.FindAsync(subGemSupport.SubGemId);
						if (subGem == null)
						{
							return new ApiErrorResult<bool>("Không tìm thấy kim cương phụ");
						}
						if (subGemSupport.Quantity <= 0)
						{
							return new ApiErrorResult<bool>($"Kim cương phụ cần có số lượng");
						}
						var subGemDetail = new SubGemDetail
						{
							ProductId = request.ProductId,
							SubGemId = subGemSupport.SubGemId,
							Quantity = subGemSupport.Quantity
						};
						await _context.SubGemDetail.AddAsync(subGemDetail);
					}
				}
				_context.Products.Update(product);
				await _context.SaveChangesAsync();
				// Process SubGemPrice ..........................Fix percent sale
				decimal totalSubGemPrice = 0;
				var subgemList = _context.SubGemDetail.Where(x => x.ProductId == product.ProductId).ToList();
				if (subgemList != null && subgemList.Count > 0)
				{
					foreach (var subgem in subgemList)
					{
						var subgemEntity = await _context.SubGems.FindAsync(subgem.SubGemId);
						if (subgemEntity == null) continue;
						totalSubGemPrice += ((decimal)subgemEntity.SubGemPrice * subgem.Quantity);
					}
				}
				// Process totalPrice
				decimal OriginalPrice = (decimal)(totalPriceGem + totalSubGemPrice) + request.ProcessingPrice + totalFramePrice; // + them cai processing price cua product
				double percent = (double)request.PercentSale / 100;
				decimal SellingPrice = OriginalPrice - (OriginalPrice * (decimal)percent);
				product.OriginalPrice = OriginalPrice;
				product.SellingPrice = SellingPrice;


				// Update Product
				product.ProductName = request.ProductName;
				product.Description = string.IsNullOrEmpty(request.Description) ? string.Empty : request.Description;
				product.IsHome = request.IsHome;
				product.IsSale = request.IsSale;
				product.DateModified = DateTime.Now;
				product.OriginalPrice = OriginalPrice;
				product.SellingPrice = SellingPrice;
				product.PercentSale = request.PercentSale;
				product.Status = request.Status.ToString();
				product.ProductPriceProcessing = request.ProcessingPrice;
				product.Quantity = request.Quantity;



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
                request.Keyword = request.Keyword.Trim();
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
                    ProcessingPrice = product.ProductPriceProcessing,
                    OriginalPrice = product.OriginalPrice,
                    SellingPrice = product.SellingPrice,
                    DateModify = product.DateModified,
                    QuantitySold = product.SellingCount,


                };
                if (product.Images != null)
                {
                    var imagePaths = product.Images.Select(x => x.ImagePath).ToList();
                    productVms.Images = imagePaths;
                }
                var gemPriceList = await _context.GemPriceLists.FindAsync(product.Gem.GemPriceListId);

                if (gemPriceList != null)
                {
                    productVms.GemPriceLists = new ViewModel.Models.GemPriceList.GemPriceListVm()
                    {
                        Active = gemPriceList.Active,
                        CaratWeight = gemPriceList.CaratWeight,
                        Clarity = gemPriceList.Clarity,
                        Color = gemPriceList.Color,
                        Cut = gemPriceList.Cut,
                        effectDate = gemPriceList.effectDate,
                        GemPriceListId = gemPriceList.GemPriceListId,
                        Price = (decimal)gemPriceList.Price,
                    };
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


        public class SubGemSupportDTOComparer : IEqualityComparer<SubGemSupportDTO>
        {
            public bool Equals(SubGemSupportDTO x, SubGemSupportDTO y)
            {
                return x.SubGemId == y.SubGemId;
            }

            public int GetHashCode(SubGemSupportDTO obj)
            {
                return obj.SubGemId.GetHashCode();
            }
        }


        public async Task<ApiResult<List<ProductVm>>> GetAll()
<<<<<<< HEAD
		{
			var listProduct = await _context.Products
				.Include(p => p.Images)
				.Include(p => p.SubGemDetails)
				.ThenInclude(sg => sg.SubGem)
				.Include(p => p.Category)
				.Include(p => p.Gem)
				.ThenInclude(x => x.InspectionCertificate)
				.Include(x => x.Frame)
				.ThenInclude(f => f.Material) // Include material within Frame
				.ToListAsync();
=======
        {
            var listProduct = await _context.Products
                .Include(p => p.Images)
                .Include(p => p.SubGemDetails)
                .ThenInclude(sg => sg.SubGem)
                .Include(p => p.Category)
                .Include(p => p.Gem)
                .ThenInclude(x => x.InspectionCertificate)
                .Include(x => x.Frame)
                .ThenInclude(f => f.Material).Include(x => x.Gem) // Include material within Frame
                .ToListAsync();
>>>>>>> a6b97dec240dd8d1ad75855d6cce22972458b03f

            List<ProductVm> listResultVm = new List<ProductVm>();

            foreach (var product in listProduct)
            {
                var gemPriceList = await _context.GemPriceLists.FindAsync(product.Gem.GemPriceListId);

                var productVm = new ProductVm
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Description = product.Description,
                    ProductThumbnail = product.ProductThumbnail,
                    IsHome = product.IsHome,
                    IsSale = product.IsSale,
                    PercentSale = product.PercentSale,
                    Status = product.Status,
                    Quantity = product.Quantity,
                    ProcessingPrice = product.ProductPriceProcessing,
                    OriginalPrice = product.OriginalPrice,
                    SellingPrice = product.SellingPrice,
                    DateModify = product.DateModified,
                    QuantitySold = product.SellingCount,
                    CategoryVm = new CategoryVm
                    {
                        CategoryId = product.Category.CategoryId,
                        CategoryName = product.Category.CategoryName,
                        CategoryType = product.Category.CategoryType,
                        CategoryImage = product.Category.CategoryImage,
                        Status = product.Category.Status
                    },
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
                        InspectionCertificateVm = product.Gem.InspectionCertificate != null ? new InspectionCertificateVm
                        {
                            InspectionCertificateId = product.Gem.InspectionCertificate.InspectionCertificateId,
                            DateGrading = product.Gem.InspectionCertificate.DateGrading,
                            InspectionCertificateName = product.Gem.InspectionCertificate.InspectionCertificateName,
                            Logo = product.Gem.InspectionCertificate.Logo,
                            Status = product.Gem.InspectionCertificate.Status
                        } : null,
                    },
                    ListSubGems = product.SubGemDetails.Select(sg => new SubGemSupportDTO
                    {
                        SubGemId = sg.SubGemId,
                        Quantity = sg.Quantity
                    }).ToList(),
                    Images = product.Images.Select(i => i.ImagePath).ToList(),

                    FrameVm = product.Frame != null ? new FrameVm
                    {
                        FrameId = product.Frame.FrameId,
                        NameFrame = product.Frame.FrameName,
                        Weight = product.Frame.Weight,
                        MaterialVm = product.Frame.Material != null ? new MaterialVm
                        {
                            MaterialId = product.Frame.Material.MaterialId,
                            Color = product.Frame.Material.Color,
                            Description = product.Frame.Material.Description,
                            MaterialImage = product.Frame.Material.MaterialImage,
                            MaterialName = product.Frame.Material.MaterialName,
                            Status = product.Frame.Material.Status
                        } : null
                    } : null,
                    GemPriceLists = new ViewModel.Models.GemPriceList.GemPriceListVm()
                    {
                        Active = gemPriceList.Active,
                        CaratWeight = gemPriceList.CaratWeight,
                        Clarity = gemPriceList.Clarity,
                        Color = gemPriceList.Color,
                        Cut = gemPriceList.Cut,
                        effectDate = gemPriceList.effectDate,
                        Price = (decimal)gemPriceList.Price,
                    },
                };

                listResultVm.Add(productVm);
            }

            return new ApiSuccessResult<List<ProductVm>>(listResultVm);
        }

        public async Task<ApiResult<List<ProductCategorySale>>> ViewProductCategorySale()
        {
            var result = await _context.Categories
        .GroupJoin(
            _context.Products,
            category => category.CategoryId,
            product => product.CategoryId,
            (category, products) => new
            {
                CategoryName = category.CategoryName,
                CategoryType = category.CategoryType,
                SoldProducts = products.Where(p => p.SellingCount > 0)
            }
        )
        .Select(g => new ProductCategorySale
        {
            CategoryName = g.CategoryName,
            CategoryType = g.CategoryType,
            Quantity = g.SoldProducts.Sum(p => p.SellingCount)
        })
        .ToListAsync();

            return new ApiSuccessResult<List<ProductCategorySale>>(result,"Success");
        }

        public async Task<ApiResult<List<ProductSaleChart>>> ViewProductSale12Days()
        {
            try
            {
                DateTime today = DateTime.Now.Date;
                DateTime twelveDaysAgo = today.AddDays(-11);

                var salesData = await _context.Orders
                    .Where(order => order.OrderDate >= twelveDaysAgo && order.OrderDate < today.AddDays(1) && order.isShip)
                    .SelectMany(order => order.OrderDetails)
                    .GroupBy(orderDetail => new
                    {
                        DateSale = orderDetail.Order.OrderDate.Date,
                        orderDetail.Product.ProductName
                    })
                    .Select(group => new
                    {
                        group.Key.DateSale,
                        group.Key.ProductName,
                        Quantity = group.Sum(orderDetail => orderDetail.Quantity)
                    })
                    .ToListAsync();

                var allDates = Enumerable.Range(0, 12)
                    .Select(offset => twelveDaysAgo.AddDays(offset))
                    .ToList();

                var productSaleCharts = allDates
                    .Select(date => new ProductSaleChart
                    {
                        DateSale = date,
                        ListProduct = salesData
                            .Where(data => data.DateSale == date)
                            .Select(data => new ProductInfo
                            {
                                ProductName = data.ProductName,
                                Quantity = data.Quantity
                            })
                            .ToList()
                    })
                    .ToList();
                productSaleCharts = productSaleCharts.OrderBy(data => data.DateSale).ToList();

                return new ApiSuccessResult<List<ProductSaleChart>>(productSaleCharts, "Data retrieved successfully");
            }
            catch (Exception ex)
            {
                return new ApiSuccessResult<List<ProductSaleChart>>(null, $"An error occurred: {ex.Message}");
            }
        }







    }
}
