﻿namespace BlazorEcommerce.Server.Services.OderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder();
        Task<ServiceResponse<List<OrderOverviewResponse>>> GetOrders();
        Task <ServiceResponse<OrderDetailsResponse>> GetOrderDetails(int orderId);
    }
}
