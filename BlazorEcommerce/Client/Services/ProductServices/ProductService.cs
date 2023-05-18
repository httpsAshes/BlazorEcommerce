

namespace BlazorEcommerce.Client.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient http)
        {
            _http = http;
        }
        public List<Product> Products { get; set; } = new List<Product>();

        public async Task GetProducts()
        {
            var result = 
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/producta");
            if(result != null && result.Data !=null)
                Products = result.Data;

        }
    }
}
