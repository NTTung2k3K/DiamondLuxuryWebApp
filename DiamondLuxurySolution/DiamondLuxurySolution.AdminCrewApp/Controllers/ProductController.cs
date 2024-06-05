using DiamondLuxurySolution.AdminCrewApp.Service.Category;
using DiamondLuxurySolution.AdminCrewApp.Service.Frame;
using DiamondLuxurySolution.AdminCrewApp.Service.Gem;
using DiamondLuxurySolution.AdminCrewApp.Service.Product;
using DiamondLuxurySolution.AdminCrewApp.Service.Staff;
using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Product;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;
using static DiamondLuxurySolution.Utilities.Constants.Systemconstant;

namespace DiamondLuxurySolution.AdminCrewApp.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductApiService _ProductApiService;
        private readonly IStaffApiService _staffApiService;
        private readonly ICategoryApiService _categoryApiService;
        private readonly IFrameApiService _frameApiService;
        private readonly IGemApiService _gemApiService;



        public ProductController(IProductApiService ProductApiService, IStaffApiService staffApiService, ICategoryApiService categoryApiService, IFrameApiService frameApiService, IGemApiService gemApiService)
        {
            _categoryApiService = categoryApiService;
            _gemApiService = gemApiService;
            _frameApiService = frameApiService;
            _staffApiService = staffApiService;
            _ProductApiService = ProductApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(ViewProductRequest request)
        {
            try
            {

                ViewBag.txtLastSeachValue = request.Keyword;
                if (!ModelState.IsValid)
                {
                    return View();
                }
                if (TempData["FailMsg"] != null)
                {
                    ViewBag.FailMsg = TempData["FailMsg"];
                }
                if (TempData["SuccessMsg"] != null)
                {
                    ViewBag.SuccessMsg = TempData["SuccessMsg"];
                }

                var Product = await _ProductApiService.ViewProduct(request);
                return View(Product.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(string ProductId)
        {
            try
            {
                var status = await _ProductApiService.GetProductById(ProductId);
                if (status is ApiErrorResult<ProductVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (status.Message != null)
                    {
                        listError.Add(errorResult.Message);
                    }
                    else if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                    {
                        foreach (var error in listError)
                        {
                            listError.Add(error);
                        }
                    }
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(status.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string ProductId)
        {
            try
            {
                ViewBag.FullCategory = _categoryApiService.GetAll().Result.ResultObj.ToList();
                ViewBag.FullFrame = _frameApiService.GetAll().Result.ResultObj.ToList();
                ViewBag.FullSubgem = _ProductApiService.GetAll().Result.ResultObj.ToList();
                ViewBag.FullGem = _gemApiService.GetAll().Result.ResultObj.ToList();
                // Làm status từ viewBag enum
                var statuses = Enum.GetValues(typeof(ProductStatus)).Cast<ProductStatus>().ToList();
                ViewBag.ListStatus = statuses;
                var Product = await _ProductApiService.GetProductById(ProductId);
                if (Product is ApiErrorResult<ProductVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (Product.Message != null)
                    {
                        listError.Add(errorResult.Message);
                    }
                    else if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                    {
                        foreach (var error in listError)
                        {
                            listError.Add(error);
                        }
                    }
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(Product.ResultObj);
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProductRequest request)
        {
            try
            {
                ViewBag.FullCategory = _categoryApiService.GetAll().Result.ResultObj.ToList();
                ViewBag.FullFrame = _frameApiService.GetAll().Result.ResultObj.ToList();
                ViewBag.FullSubgem = _ProductApiService.GetAll().Result.ResultObj.ToList();
                ViewBag.FullGem = _gemApiService.GetAll().Result.ResultObj.ToList();
                // Làm status từ viewBag enum
                var statuses = Enum.GetValues(typeof(ProductStatus)).Cast<ProductStatus>().ToList();
                ViewBag.ListStatus = statuses;
                if (!ModelState.IsValid)
                {
                    var ProductVmCall = await _ProductApiService.GetProductById(request.ProductId);
                    ProductVm ProductVm = new ProductVm()
                    {
                        ProductId = request.ProductId,
                        Description = request.Description,
                        PercentSale = request.PercentSale,
                        IsSale = request.IsSale,
                        IsHome = request.IsHome,
                        Images = ProductVmCall.ResultObj.Images,
                        ListSubGems = request.ListSubGems,
                        ProductName = request.ProductName,
                        CategoryVm = ProductVmCall.ResultObj.CategoryVm,
                        FrameVm = ProductVmCall.ResultObj.FrameVm,
                        ProcessingPrice = request.ProcessingPrice,
                        GemVm = ProductVmCall.ResultObj.GemVm,
                        MaterialVm = ProductVmCall.ResultObj.MaterialVm,
                        ProductThumbnail = ProductVmCall.ResultObj.ProductThumbnail,
                        Quantity = request.Quantity,
                        Status = request.Status,
                    };
                    return View(ProductVm);
                }
                if (request.ListSubGems != null && request.ListSubGems.Count > 0)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true, // If needed
                        WriteIndented = true // If needed
                    };
                    string listSubGemsJson = System.Text.Json.JsonSerializer.Serialize(request.ListSubGems, options);
                    request.ListSubGemsJson = listSubGemsJson;
                }
                if (request.ExistingListSubGems != null && request.ExistingListSubGems.Count > 0)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true, // If needed
                        WriteIndented = true // If needed
                    };
                    string listExistingSubGemsJson = System.Text.Json.JsonSerializer.Serialize(request.ExistingListSubGems, options);
                    request.ListExistingSubGemsJson = listExistingSubGemsJson;
                }
                var status = await _ProductApiService.UpdateProduct(request);
                if (status is ApiErrorResult<bool> errorResult)
                {
                    List<string> listError = new List<string>();

                    if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                    {
                        foreach (var error in errorResult.ValidationErrors)
                        {
                            listError.Add(error);
                        }
                    }
                    else if (status.Message != null)
                    {
                        listError.Add(errorResult.Message);
                    }
                    ViewBag.Errors = listError;
                    return View(request);
                }

                return RedirectToAction("Index", "Product");
            }
            catch
            {
                return View(request);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Delete(string ProductId)
        {
            try
            {
                var Product = await _ProductApiService.GetProductById(ProductId);
                if (Product is ApiErrorResult<ProductVm> errorResult)
                {
                    List<string> listError = new List<string>();
                    if (Product.Message != null)
                    {
                        listError.Add(errorResult.Message);
                    }
                    else if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                    {
                        foreach (var error in listError)
                        {
                            listError.Add(error);
                        }
                    }
                    ViewBag.Errors = listError;
                    return View();

                }
                return View(Product.ResultObj);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteProductRequest request)
        {
            try
            {
                var status = await _ProductApiService.DeleteProduct(request);
                if (status is ApiErrorResult<bool> errorResult)
                {
                    List<string> listError = new List<string>();

                    if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                    {
                        foreach (var error in errorResult.ValidationErrors)
                        {
                            listError.Add(error);
                        }
                    }
                    else if (status.Message != null)
                    {
                        listError.Add(errorResult.Message);
                    }
                    ViewBag.Errors = listError;
                    return View();

                }

                return RedirectToAction("Index", "Product");

            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.FullCategory = _categoryApiService.GetAll().Result.ResultObj.ToList();
            ViewBag.FullFrame =  _frameApiService.GetAll().Result.ResultObj.ToList();
            ViewBag.FullSubgem =  _ProductApiService.GetAll().Result.ResultObj.ToList();
            ViewBag.FullGem =  _gemApiService.GetAll().Result.ResultObj.ToList();
            var statuses = Enum.GetValues(typeof(ProductStatus)).Cast<ProductStatus>().ToList();
            ViewBag.ListStatus = statuses;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {

            ViewBag.FullCategory = _categoryApiService.GetAll().Result.ResultObj.ToList();
            ViewBag.FullFrame = _frameApiService.GetAll().Result.ResultObj.ToList();
            ViewBag.FullSubgem = _ProductApiService.GetAll().Result.ResultObj.ToList();
            ViewBag.FullGem = _gemApiService.GetAll().Result.ResultObj.ToList();
            // Làm status từ viewBag enum
            var statuses = Enum.GetValues(typeof(ProductStatus)).Cast<ProductStatus>().ToList();
            ViewBag.ListStatus = statuses;


            if (!ModelState.IsValid)
            {
                return View(request);
            }
            if(request.ListSubGems!=null && request.ListSubGems.Count > 0)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // If needed
                    WriteIndented = true // If needed
                };
                string listSubGemsJson = System.Text.Json.JsonSerializer.Serialize(request.ListSubGems, options);
                request.ListSubGemsJson = listSubGemsJson;
            }
           



            var status = await _ProductApiService.CreateProduct(request);

            if (status is ApiErrorResult<bool> errorResult)
            {
                List<string> listError = new List<string>();

                if (errorResult.ValidationErrors != null && errorResult.ValidationErrors.Count > 0)
                {
                    foreach (var error in errorResult.ValidationErrors)
                    {
                        listError.Add(error);
                    }
                }
                else if (status.Message != null)
                {
                    listError.Add(errorResult.Message);
                }
                ViewBag.Errors = listError;
                return View();

            }

            TempData["SuccessMsg"] = "Tạo mới thành công cho " + request.ProductName;

            return RedirectToAction("Index", "Product");
        }
    }

}
