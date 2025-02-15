﻿using DiamondLuxurySolution.ViewModel.Common;
using DiamondLuxurySolution.ViewModel.Models.Order;
using DiamondLuxurySolution.ViewModel.Models.Platform;
using DiamondLuxurySolution.ViewModel.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Application.Repository.Product
{
    public interface IProductRepo
    {
        public Task<ApiResult<List<ProductVm>>> GetAll();
		public Task<ApiResult<bool>> CreateProduct(CreateProductRequest request);
        public Task<ApiResult<bool>> UpdateProduct(UpdateProductRequest request);
        public Task<ApiResult<bool>> DeleteProduct(DeleteProductRequest request);
        public Task<ApiResult<ProductVm>> GetProductById(string ProductId);
        public Task<ApiResult<PageResult<ProductVm>>> ViewProduct(ViewProductRequest request);
        public Task<ApiResult<List<ProductCategorySale>>> ViewProductCategorySale();
        public Task<ApiResult<List<ProductSaleChart>>> ViewProductSale12Days();


    }
}
