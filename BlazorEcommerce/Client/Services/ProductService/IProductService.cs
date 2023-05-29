namespace BlazorEcommerce.Client.Services.ProductServices
{
    public interface IProductService
    {
        event Action ProductChanged;
        string Message { get; set; }
        List<Product> Products { get; set; }
        Task GetProducts(string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProduct(int prodyctId);
        Task SearchProducs(string searchText);
        Task<List<string>>GetProductSearchSuggestions(string searchText);
    }
}
