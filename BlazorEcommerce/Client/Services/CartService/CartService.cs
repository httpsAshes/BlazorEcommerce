using BlazorEcommerce.Client.Pages;
using BlazorEcommerce.Shared;
using Blazored.LocalStorage;

namespace BlazorEcommerce.Client.Services.CartService
{
    public class CartService : ICartService
    {

        private readonly ILocalStorageService _locaStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        private readonly HttpClient _http;
        public CartService(ILocalStorageService localStorage, HttpClient http, AuthenticationStateProvider authStateProvider) 
        {
            
            _locaStorage = localStorage;
            _http = http;
            _authStateProvider = authStateProvider;

        }

        public event Action OnChange;

        public async Task AddToCart(CartItem cartItem)
        {
            if (await IsUserAuthenticated())
            {
                Console.WriteLine("User Athenticated");
            }
            else
            {
                Console.WriteLine("User Not Authenticated");
            }

            var cart = await _locaStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            var sameItem = cart.Find(x => x.ProductId == cartItem.ProductId &&
                            x.ProductTypeId == cartItem.ProductTypeId);
            if (sameItem == null)
            {
                cart.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }

            await _locaStorage.SetItemAsync("cart", cart);
            await GetCartItemsCount();
        }

        
        public async Task<List<CartItem>> GetCartItems()
        {
            await GetCartItemsCount();
            var cart = await _locaStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            

            return cart;
        }

        public async Task GetCartItemsCount()
        {
            if(await IsUserAuthenticated())
            {
                var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = result.Data;
                await _locaStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cart = await _locaStorage.GetItemAsync<List<CartItem>>("cart");
                await _locaStorage.SetItemAsync<int>("cartItemsCount", cart != null ? cart.Count : 0);
            }


            OnChange.Invoke();
        }

        public async Task<List<CartProductResponse>> GetCartProducts()
        {
            var cartItems = await _locaStorage.GetItemAsync<List<CartItem>>("cart");
            var response = await _http.PostAsJsonAsync("api/cart/products", cartItems);
            var cartProducts = 
                await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();

            return cartProducts.Data;

        }

        public async Task RemoveProductFromCart(int ProductId, int productTypeId)
        {
            var cart = await _locaStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                return;
            }

            var cartItem=cart.Find(x => x.ProductId == ProductId && x.ProductTypeId==productTypeId);
            if(cartItem!=null)
            {
                cart.Remove(cartItem);
                await _locaStorage.SetItemAsync("cart", cart);
                await GetCartItemsCount();
            }
            
        }

        public async Task StoreCartItems(bool emptyLocalCart = false)
        {
            var localCart = await _locaStorage.GetItemAsync<List<CartItem>>("cart");
            if (localCart == null)
            {
                return;
            }

            await _http.PostAsJsonAsync("api/cart", localCart);
            if(emptyLocalCart)
            {
                await _locaStorage.RemoveItemAsync("cart");
            }
        }

        public async Task UpdateQuantity(CartProductResponse product)
        {
            var cart = await _locaStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                return;
            }

            var cartItem = cart.Find(x => x.ProductId == product.ProductId 
            && x.ProductTypeId == product.ProductTypeId);
            if (cartItem != null)
            {
                cartItem.Quantity = product.Quantity;
                await _locaStorage.SetItemAsync("cart", cart);
                
            }
        }
        private async Task<bool> IsUserAuthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

    }

}
