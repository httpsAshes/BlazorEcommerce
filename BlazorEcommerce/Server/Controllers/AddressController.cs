using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addrressService;

        public AddressController(IAddressService addressService)
        {
            _addrressService = addressService;
        }
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<Address>>> GetAdddress()
        {
            return await _addrressService.GetAddress();
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Address>>> AddOrUpdateAdddress(Address address)
        {
            return await _addrressService.AddOrUpdateAddress(address);
        }
    }
}
