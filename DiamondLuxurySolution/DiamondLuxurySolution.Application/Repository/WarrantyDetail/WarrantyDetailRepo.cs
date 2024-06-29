using DiamondLuxurySolution.Data.EF;
using DiamondLuxurySolution.Data.Entities;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Category;
using DiamondLuxurySolution.ViewModel.Models.Frame;
using DiamondLuxurySolution.ViewModel.Models.Gem;
using DiamondLuxurySolution.ViewModel.Models.GemPriceList;
using DiamondLuxurySolution.ViewModel.Models.InspectionCertificate;
using DiamondLuxurySolution.ViewModel.Models.Material;
using DiamondLuxurySolution.ViewModel.Models;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Product;
using DiamondLuxurySolution.ViewModel.Models.WarrantyDetail;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiamondLuxurySolution.ViewModel.Models.User.Customer;
using DiamondLuxurySolution.ViewModel.Models.Warranty;

namespace DiamondLuxurySolution.Application.Repository.WarrantyDetail
{
    public class WarrantyDetailRepo : IWarrantyDetailRepo
    {

        private readonly LuxuryDiamondShopContext _context;
        public WarrantyDetailRepo(LuxuryDiamondShopContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<string>> CheckValidWarrantyId(string WarrantyId)
        {
            var Warrantys = await _context.Warrantys.FindAsync(WarrantyId);
            if (Warrantys == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy chi tiết thông tin bảo hành");
            }
            var today = DateTime.Now;
            if (Warrantys.DateActive <= today && today <= Warrantys.DateExpired)
            {
                return new ApiSuccessResult<string>("Hợp lệ: (Ngày bắt đầu:" + Warrantys.DateActive + " | " + "Ngày kết thúc:" + Warrantys.DateExpired + ")", "Hợp lệ: (Ngày bắt đầu:" + Warrantys.DateActive + " | " + "Ngày kết thúc:" + Warrantys.DateExpired + ")");
            }
            return new ApiSuccessResult<string>("Không hợp lệ: (Ngày bắt đầu:" + Warrantys.DateActive + " | " + "Ngày kết thúc:" + Warrantys.DateExpired + ")", "Không hợp lệ: (Ngày bắt đầu:" + Warrantys.DateActive + " | " + "Ngày kết thúc:" + Warrantys.DateExpired + ")");

        }

        public async Task<ApiResult<bool>> CreateWarrantyDetail(CreateWarrantyDetailRequest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.WarrantyDetailName))
            {
                errorList.Add("Vui lòng nhập tên bảo hành");
            }
            if (string.IsNullOrEmpty(request.WarrantyId))
            {
                errorList.Add("Vui lòng nhập mã bảo hành");
            }
            if (request.ReturnProductDate != null)
            {
                if (request.ReturnProductDate < request.ReceiveProductDate)
                {
                    errorList.Add("Ngày trả sản phẩm ko hợp lệ");
                }
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }
            var Warrantys = await _context.Warrantys.FindAsync(request.WarrantyId);
            if (Warrantys == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy mã bảo hành");
            }
            var today = DateTime.Now;
            if (!(Warrantys.DateActive <= today && today <= Warrantys.DateExpired))
            {
                return new ApiErrorResult<bool>("Không hợp lệ: (Ngày bắt đầu:" + Warrantys.DateActive + " | " + "Ngày kết thúc:" + Warrantys.DateExpired + ")");
            }


            var warrantyDetail = new DiamondLuxurySolution.Data.Entities.WarrantyDetail
            {
                WarrantyId = request.WarrantyId,
                WarrantyDetailName = request.WarrantyDetailName,
                Description = request.Description,
                ReceiveProductDate = (DateTime)(request.ReceiveProductDate),
                ReturnProductDate = request.ReturnProductDate == null ? null : request.ReturnProductDate,
                Status = request.Status,
                WarrantyType = request.WarrantyType,
            };
            if (request.Image != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Image);
                warrantyDetail.Image = firebaseUrl;
            }

            _context.WarrantyDetails.Add(warrantyDetail);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Success");
        }

        public async Task<ApiResult<bool>> DeleteWarrantyDetail(DeleteWarrantyDetailRequest request)
        {
            var warrantyDetail = await _context.WarrantyDetails.FindAsync(request.WarrantyDetailId);
            if (warrantyDetail == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy chi tiết thông tin bảo hành");
            }
            _context.WarrantyDetails.Remove(warrantyDetail);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(false, "Success");
        }

        public async Task<ApiResult<List<WarrantyDetailVm>>> GetAll()
        {
            var list = await _context.WarrantyDetails
                .Include(x => x.Warranty)
                .ThenInclude(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .Include(x => x.Warranty)
                .ThenInclude(x => x.OrderDetails)
                .ThenInclude(x => x.Order)
                .ThenInclude(x => x.Customer)
                .ToListAsync();

            var rs = new List<WarrantyDetailVm>();

            foreach (var item in list)
            {
                var productId = item.Warranty.OrderDetails.First().ProductId;
                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Gem).ThenInclude(g => g.InspectionCertificate)
                    .Include(p => p.SubGemDetails)
                    .Include(p => p.Images)
                    .Include(p => p.Frame).ThenInclude(f => f.Material)
                    .FirstOrDefaultAsync(p => p.ProductId == productId);

                if (product == null) continue;

                var customer = item.Warranty.OrderDetails.First().Order.Customer;

                var warrantyDetailVm = new WarrantyDetailVm()
                {
                    WarrantyDetailId = item.WarrantyDetailId,
                    Description = item.Description,
                    ReceiveProductDate = item.ReceiveProductDate ?? DateTime.MinValue,
                    ReturnProductDate = item.ReturnProductDate ?? DateTime.MinValue,
                    Image = item.Image,
                    WarrantyDetailName = item.WarrantyDetailName,
                    WarrantyType = item.WarrantyType,
                    ProductVm = new ProductVm()
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
                    },
                    CustomerVm = new CustomerVm()
                    {
                        CustomerId = customer.Id,
                        FullName = customer.Fullname ?? "Không có",
                        Email = customer.Email ?? "Không có",
                        PhoneNumber = customer.PhoneNumber ?? "Không có",
                        Address = customer.Address ?? "Không có"
                    },
                    Status = item.Status,
                    WarrantyVm = new ViewModel.Models.Warranty.WarrantyVm()
                    {
                        WarrantyId = item.WarrantyId,
                        DateActive = item.Warranty.DateActive,
                        DateExpired = item.Warranty.DateExpired,
                        Description = item.Warranty.Description,
                        Status = item.Warranty.Status,
                        WarrantyName = item.Warranty.WarrantyName
                    }
                };

                rs.Add(warrantyDetailVm);
            }

            return new ApiSuccessResult<List<WarrantyDetailVm>>(rs);
        }



        public async Task<ApiResult<WarrantyDetailVm>> GetWarrantyDetaiById(int WarrantyDetailId)
        {
            var warrantyDetail = await _context.WarrantyDetails
                .Include(x => x.Warranty)
                .ThenInclude(x => x.OrderDetails)
                .ThenInclude(x => x.Product).ThenInclude(X => X.Frame).ThenInclude(X => X.Material)
                .Include(x => x.Warranty)
                .ThenInclude(x => x.OrderDetails)
                .ThenInclude(x => x.Order)
                .ThenInclude(x => x.Customer)
                .Include(x => x.Warranty)
                .ThenInclude(x => x.OrderDetails)
                .ThenInclude(x => x.Product).ThenInclude(X =>X.Gem).ThenInclude(X => X.GemPriceList)
                .FirstOrDefaultAsync(x => x.WarrantyDetailId == WarrantyDetailId);

            if (warrantyDetail == null)
            {
                return new ApiErrorResult<WarrantyDetailVm>("Không tìm thấy chi tiết thông tin bảo hành");
            }

            var productId = warrantyDetail.Warranty.OrderDetails.First().ProductId;
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Gem).ThenInclude(g => g.InspectionCertificate)
                .Include(p => p.SubGemDetails).ThenInclude(x => x.SubGem)
                .Include(p => p.Images)
                .Include(p => p.Frame).ThenInclude(f => f.Material)
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return new ApiErrorResult<WarrantyDetailVm>("Không tìm thấy sản phẩm liên quan");
            }

            var customer = warrantyDetail.Warranty.OrderDetails.First().Order.Customer;
            var quantityBuy = warrantyDetail.Warranty.OrderDetails.First().Quantity;


            var warrantyDetailVm = new WarrantyDetailVm()
            {
                WarrantyDetailId = warrantyDetail.WarrantyDetailId,
                Description = warrantyDetail.Description,


                Image = warrantyDetail.Image,
                WarrantyDetailName = warrantyDetail.WarrantyDetailName,
                WarrantyType = warrantyDetail.WarrantyType,
                ProductVm = new ProductVm()
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Description = product.Description,
                    ProductThumbnail = product.ProductThumbnail,
                    IsHome = product.IsHome,
                    IsSale = product.IsSale,
                    PercentSale = product.PercentSale,
                    Status = product.Status,
                    Quantity = quantityBuy,
                    ProcessingPrice = product.ProductPriceProcessing,
                    OriginalPrice = product.OriginalPrice,
                    SellingPrice = product.SellingPrice,
                    DateModify = product.DateModified,
                    QuantitySold = product.SellingCount,
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
                    CategoryVm = new CategoryVm
                    {
                        CategoryName = product.Category.CategoryName
                    },
                    
                    ListSubGems = product.SubGemDetails.Select(x => new SubGemSupportDTO()
                    {
                        Quantity = x.Quantity,
                        SubGemName = x.SubGem.SubGemName,
                    }).ToList()
                },
                CustomerVm = new CustomerVm()
                {
                    CustomerId = customer.Id,
                    FullName = customer.Fullname ?? "Không có",
                    Email = customer.Email ?? "Không có",
                    PhoneNumber = customer.PhoneNumber ?? "Không có",
                    Address = customer.Address ?? "Không có",
                    Dob = customer.Dob ?? DateTime.MinValue,
                    Status = customer.Status
                },
                Status = warrantyDetail.Status,
                WarrantyVm = new ViewModel.Models.Warranty.WarrantyVm()
                {
                    WarrantyId = warrantyDetail.WarrantyId,
                    DateActive = warrantyDetail.Warranty.DateActive,
                    DateExpired = warrantyDetail.Warranty.DateExpired,
                    Description = warrantyDetail.Warranty.Description,
                    Status = warrantyDetail.Warranty.Status,
                    WarrantyName = warrantyDetail.Warranty.WarrantyName
                }
            };
            if(product.Frame != null)
            {
                warrantyDetailVm.ProductVm.FrameVm = new FrameVm
                {
                    NameFrame = product.Frame.FrameName,
                    Weight = product.Frame.Weight,
                };

                warrantyDetailVm.ProductVm.FrameVm.MaterialVm = new MaterialVm
                {
                    MaterialName = product.Frame.Material.MaterialName,
                    Color = product.Frame.Material.Color,
                };
            }

            if (warrantyDetail.ReturnProductDate != null)
            {
                warrantyDetailVm.ReturnProductDate = warrantyDetail.ReturnProductDate;
            }

            if (warrantyDetail.ReceiveProductDate != null)
            {
                warrantyDetailVm.ReceiveProductDate = (DateTime)warrantyDetail.ReceiveProductDate;
            }

            return new ApiSuccessResult<WarrantyDetailVm>(warrantyDetailVm);
        }


        public async Task<ApiResult<bool>> UpdateWarrantyDetail(UpdateWarrantyDetailRequest request)
        {
            var errorList = new List<string>();
            if (string.IsNullOrEmpty(request.WarrantyDetailName))
            {
                errorList.Add("Vui lòng nhập tên bảo hành");
            }
            if (string.IsNullOrEmpty(request.WarrantyId))
            {
                errorList.Add("Vui lòng nhập mã bảo hành");
            }
            if (request.ReturnProductDate != null && request.ReturnProductDate < request.ReceiveProductDate)
            {
                errorList.Add("Ngày trả sản phẩm không hợp lệ");
            }
            if (errorList.Any())
            {
                return new ApiErrorResult<bool>("Không hợp lệ", errorList);
            }


            var Warrantys = await _context.Warrantys.FindAsync(request.WarrantyId);
            if (Warrantys == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy chi tiết thông tin bảo hành");
            }
            var today = DateTime.Now;
            if (!(Warrantys.DateActive <= today && today <= Warrantys.DateExpired))
            {
                return new ApiErrorResult<bool>("Không hợp lệ: (Ngày bắt đầu:" + Warrantys.DateActive + " | " + "Ngày kết thúc:" + Warrantys.DateExpired + ")");
            }




            var warrantyDetail = await _context.WarrantyDetails.FindAsync(request.WarrantyDetailId);
            if (warrantyDetail == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy chi tiết bảo hành");
            }





            warrantyDetail.WarrantyId = request.WarrantyId;
            warrantyDetail.WarrantyDetailName = request.WarrantyDetailName;
            warrantyDetail.Description = request.Description;
            warrantyDetail.Status = request.Status;
            warrantyDetail.WarrantyType = request.WarrantyType;
            warrantyDetail.ReceiveProductDate = (DateTime)(request.ReceiveProductDate);
            warrantyDetail.ReturnProductDate = request.ReturnProductDate == null ? null : request.ReturnProductDate;
            if (request.Image != null)
            {
                string firebaseUrl = await DiamondLuxurySolution.Utilities.Helper.ImageHelper.Upload(request.Image);
                warrantyDetail.Image = firebaseUrl;
            }

            _context.WarrantyDetails.Update(warrantyDetail);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>(true, "Cập nhật thành công");
        }


        public async Task<ApiResult<PageResult<WarrantyDetailVm>>> ViewWarrantyDetai(ViewWarrantyDetailRequest request)
        {
            var listWarrantyDetails = _context.WarrantyDetails
                .Include(x => x.Warranty)
                .ThenInclude(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .Include(x => x.Warranty)
                .ThenInclude(x => x.OrderDetails)
                .ThenInclude(x => x.Order)
                .ThenInclude(x => x.Customer)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                listWarrantyDetails = listWarrantyDetails.Where(x => x.WarrantyDetailName.Contains(request.Keyword) || x.Warranty.Description.Contains(request.Keyword));
            }

            listWarrantyDetails = listWarrantyDetails.OrderByDescending(x => x.WarrantyDetailName);

            int pageIndex = request.pageIndex ?? 1;
            int pageSize = DiamondLuxurySolution.Utilities.Constants.Systemconstant.AppSettings.PAGE_SIZE;

            var listPaging = await listWarrantyDetails.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            var listWarrantyDetailVm = new List<WarrantyDetailVm>();
            foreach (var item in listPaging)
            {
                var productId = item.Warranty.OrderDetails.First().ProductId;
                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Gem).ThenInclude(g => g.InspectionCertificate)
                    .Include(p => p.SubGemDetails)
                    .Include(p => p.Images)
                    .Include(p => p.Frame).ThenInclude(f => f.Material)
                    .FirstOrDefaultAsync(p => p.ProductId == productId);

                if (product == null) continue;

                var customer = item.Warranty.OrderDetails.First().Order.Customer;

                var warrantyDetailVm = new WarrantyDetailVm()
                {
                    WarrantyDetailId = item.WarrantyDetailId,
                    Description = item.Description,


                    Image = item.Image,
                    WarrantyDetailName = item.WarrantyDetailName,
                    WarrantyType = item.WarrantyType,
                    ProductVm = new ProductVm()
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
                    },
                    CustomerVm = new CustomerVm()
                    {
                        CustomerId = customer.Id,
                        FullName = customer.Fullname ?? "Không có",
                        Email = customer.Email ?? "Không có",
                        PhoneNumber = customer.PhoneNumber ?? "Không có",
                        Address = customer.Address ?? "Không có"
                    },
                    Status = item.Status,
                    WarrantyVm = new ViewModel.Models.Warranty.WarrantyVm()
                    {
                        WarrantyId = item.WarrantyId,
                        DateActive = item.Warranty.DateActive,
                        DateExpired = item.Warranty.DateExpired,
                        Description = item.Warranty.Description,
                        Status = item.Warranty.Status,
                        WarrantyName = item.Warranty.WarrantyName
                    }
                };
                if (item.ReturnProductDate != null)
                {
                    warrantyDetailVm.ReturnProductDate = item.ReturnProductDate;
                }

                if (item.ReceiveProductDate != null)
                {
                    warrantyDetailVm.ReceiveProductDate = (DateTime)item.ReceiveProductDate;
                }
                listWarrantyDetailVm.Add(warrantyDetailVm);
            }

            var totalRecords = await listWarrantyDetails.CountAsync();
            listWarrantyDetailVm = listWarrantyDetailVm.OrderByDescending(x => x.ReceiveProductDate).ToList();
            var listResult = new PageResult<WarrantyDetailVm>()
            {
                Items = listWarrantyDetailVm,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                PageIndex = pageIndex
            };

            return new ApiSuccessResult<PageResult<WarrantyDetailVm>>(listResult, "Success");
        }


    }
}
