using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>> GetAllOrder();
        Task<IEnumerable<OrderResponseModel>> GetAllOrderByUser(int userId);
        Task<OrderResponseModel> GetOrderById(int orderId);

        //Task<IEnumerable<OrderDetailResponseModel>> GetOrderDetails(int orderId);
        //Task<OrderDetailResponseModel> AddOrderDetail(OrderDetailRequestModel request);
        //Task<OrderDetailResponseModel> UpdateOrderDetail(OrderDetailRequestModel request);
        Task<OrderResponseModel> CreateOrder(OrderRequestModel request);
        Task<OrderResponseModel> UpdateOrder(int orderId, int status);
        Task<OrderResponseModel> AddVoucherToOrder(int orderId, int voucherId);
        //Task<bool> RemoveOrderDetail(int orderId, int productId);
        //Task<bool> ClearOrder(int orderId);
        Task<bool> RemoveOrder(int orderId);

    }

}
