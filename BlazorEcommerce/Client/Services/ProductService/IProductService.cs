﻿

namespace BlazorEcommerce.Client.Services.ProductServices
{
    public interface IProductService
    {
        event Action ProductChanged;
        List<Product> Products { get; set; }
        Task GetProducts(string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProduct(int prodyctId);
    }
}
