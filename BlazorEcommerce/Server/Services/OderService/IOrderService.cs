namespace BlazorEcommerce.Server.Services.OderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder();

    }
}
