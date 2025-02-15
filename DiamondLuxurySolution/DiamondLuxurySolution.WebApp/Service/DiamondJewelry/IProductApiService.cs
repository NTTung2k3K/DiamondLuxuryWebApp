﻿using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Product;

namespace DiamondLuxurySolution.WebApp.Service.Product
{
    public interface IProductApiService
    {
        public Task<ApiResult<bool>> CreateProduct(CreateProductRequest request);
        public Task<ApiResult<bool>> UpdateProduct(UpdateProductRequest request);
        public Task<ApiResult<bool>> DeleteProduct(DeleteProductRequest request);
        public Task<ApiResult<ProductVm>> GetProductById(string ProductId);
        public Task<ApiResult<PageResult<ProductVm>>> ViewProduct(ViewProductRequest request);
        public Task<ApiResult<List<ProductVm>>> GetAll();

	}
}
