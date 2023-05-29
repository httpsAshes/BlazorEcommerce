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

        public string Message { get; set; } = "Loading Products...";


        public event Action ProductChanged;
        

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = 
                await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
                return result;
        
        }

        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = categoryUrl == null ?
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product") :
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");
            if (result != null && result.Data !=null)
                Products = result.Data;

           ProductChanged.Invoke();
        }

        
        public async Task SearchProducs(string searchText)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/search/{searchText}");

            if(result != null && result.Data !=null)
                Products = result.Data;
            
            if(Products.Count == 0) Message = "No Products Found";
            ProductChanged?.Invoke();


        }

        

        public  async Task<List<string>> GetProductSearchSuggestions(string searchText)
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggestions/{searchText}");
            return result.Data;

        }
    }
}
