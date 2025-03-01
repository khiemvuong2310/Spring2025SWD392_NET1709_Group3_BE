using AutoMapper;
using Azure.Core;
using DianaShop.Data.Entities;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianaShop.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderResponseModel> CreateOrder(OrderRequestModel request)
        {
            if (request == null)
                throw new ArgumentException("Invalid request data.");
            if (request.UserID <= 0)
                throw new ArgumentException("UserID must be greater than 0.");

            // Kiểm tra xem User đã có giỏ hàng chưa
            var unpaidOrder = await _unitOfWork.Repository<Order>()
                .FindAsync(c => c.UserID == request.UserID && c.Status.Equals("Unpaid"));

            if (unpaidOrder != null)
                throw new InvalidOperationException("User already has an unpaid order.");

            var order = new Order
            {
                UserID = request.UserID,
                Quantity = 0,
                OrderDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Details = new List<OrderDetail>()
            };

            await _unitOfWork.Repository<Order>().InsertAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderResponseModel>(order);
        }

        public async Task<IEnumerable<OrderResponseModel>> GetAllOrderByUser(int userId)
        {
            var orders = await _unitOfWork.Repository<Order>()
                .GetQueryable()
                .Where(cp => cp.UserID == userId)
                .Include(cp => cp.Details)
                .ToListAsync();
            return _mapper.Map<IEnumerable<OrderResponseModel>>(orders);
        }

        public async Task<OrderResponseModel> GetOrderById(int orderId)
        {
            var order = await _unitOfWork.Repository<Order>()
                .GetQueryable()
                .Where(cp => cp.Id == orderId)
                .Include(cp => cp.Details)
                .FirstOrDefaultAsync();
            return _mapper.Map<OrderResponseModel>(order);
        }

        public async Task<IEnumerable<OrderDetailResponseModel>> GetOrderDetails(int orderId)
        {
            var orderDetails = await _unitOfWork.Repository<OrderDetail>()
                .GetQueryable()
                .Where(cp => cp.OrderId == orderId)
                .Include(cp => cp.Product)
                .ToListAsync();
            return _mapper.Map<IEnumerable<OrderDetailResponseModel>>(orderDetails);
        }

        public async Task<OrderDetailResponseModel> AddOrderDetail(OrderDetailRequestModel request)
        {
            if (request == null || request.OrderID <= 0 || request.ProductId <= 0 || request.Quantity <= 0)
                throw new ArgumentException("Invalid request data. Id and Quantity is greater than 0");

            // Kiểm tra order có tồn tại không
            var order = await _unitOfWork.Repository<Order>().FindAsync(c => c.Id == request.OrderID);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {request.OrderID} not found.");

            // Kiểm tra Product có tồn tại không
            var product = await _unitOfWork.Repository<Product>().FindAsync(p => p.Id == request.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {request.ProductId} not found.");

            // Kiểm tra sản phẩm đã có chưa
            var existingProduct = await _unitOfWork.Repository<OrderDetail>()
                .FindAsync(cp => cp.OrderId == request.OrderID && cp.ProductId == request.ProductId);

            if (existingProduct != null)
            {
                existingProduct.Quantity += request.Quantity;
                _unitOfWork.Repository<OrderDetail>().UpdateAsync(existingProduct);
            }
            else
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = request.OrderID,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };
                await _unitOfWork.Repository<OrderDetail>().InsertAsync(orderDetail);
            }

            //cập nhật order sau khi thêm product
            var updateOrder = order;
            updateOrder.Quantity += request.Quantity;
            updateOrder.TotalAmount += (product.Price * request.Quantity);
            updateOrder.UpdateDate = DateTime.Now;
            await _unitOfWork.Repository<Order>().UpdateAsync(updateOrder);

            await _unitOfWork.SaveChangesAsync();

            var AddedResult = await _unitOfWork.Repository<OrderDetail>()
                .FindAsync(x => x.OrderId == request.OrderID && x.ProductId == request.ProductId);

            return _mapper.Map<OrderDetailResponseModel>(AddedResult);
        }

        public async Task<OrderResponseModel> UpdateOrder(int orderId, string status)
        {
            var order = await _unitOfWork.Repository<Order>()
                .GetQueryable()
                .Where(cp => cp.UserID == orderId)
                .FirstOrDefaultAsync();

            if (order == null)
                throw new Exception("Order not found");

            order.Status = status;
            order.UpdateDate = DateTime.Now;
            _unitOfWork.Repository<Order> ().UpdateAsync(order);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<OrderResponseModel>(order);
        }

        public async Task<OrderDetailResponseModel> UpdateOrderDetail(OrderDetailRequestModel request)
        {
            var orderDetail = await _unitOfWork.Repository<OrderDetail>()
                .GetQueryable()
                .Where(cp => cp.OrderId == request.OrderID && cp.ProductId == request.ProductId)
                .Include(cp => cp.Product)
                .FirstOrDefaultAsync();

            if (orderDetail == null)
                throw new Exception("Order detail not found");


            var order = await _unitOfWork.Repository<Order>().FindAsync(cp => cp.Id == request.OrderID);

            if (order == null)
                throw new Exception("Order not found");

            order.Quantity -= orderDetail.Quantity;
            orderDetail.Quantity = request.Quantity;
            order.Quantity += request.Quantity;
            order.UpdateDate = DateTime.Now;

            await _unitOfWork.Repository<OrderDetail>().UpdateAsync(orderDetail);
            await _unitOfWork.Repository<Order>().UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDetailResponseModel>(orderDetail);
        }

        public async Task<bool> RemoveOrderDetail(int orderId, int productId)
        {
            var orderDetail = await _unitOfWork.Repository<OrderDetail>()
            .FindAsync(cp => cp.OrderId == orderId && cp.ProductId == productId);
            if (orderDetail == null)
                return false;

            var order = await _unitOfWork.Repository<Order>().FindAsync(cp => cp.Id == orderId);
            if (order == null)
                return false;

            order.Quantity -= orderDetail.Quantity;
            order.TotalAmount -= (orderDetail.Quantity * orderDetail.UnitPrice);
            order.UpdateDate = DateTime.Now;

            _unitOfWork.Repository<OrderDetail>().Delete(orderDetail);
            _unitOfWork.Repository<Order>().UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearOrder(int orderId)
        {
            var orderDetails = _unitOfWork.Repository<OrderDetail>()
                .GetQueryable()
                .Where(cp => cp.OrderId == orderId);
            if (!await orderDetails.AnyAsync())
                return false;

            var order = await _unitOfWork.Repository<Order>()
                .FindAsync(c => c.Id == orderId);
            if (order == null)
                return false;

            order.Quantity = 0;
            order.TotalAmount = 0;
            order.UpdateDate = DateTime.Now;

            _unitOfWork.Repository<OrderDetail>().DeleteRange(orderDetails);
            _unitOfWork.Repository<Order>().UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveOrder(int orderId)
        {
            var order = await _unitOfWork.Repository<Order>()
                .FindAsync(c => c.Id == orderId);
            if (order == null)
                return false;

            var orderDetails = _unitOfWork.Repository<OrderDetail>()
                .GetQueryable()
                .Where(cp => cp.OrderId == orderId);
            if (!await orderDetails.AnyAsync())
                return false;

            _unitOfWork.Repository<OrderDetail>().DeleteRange(orderDetails);
            _unitOfWork.Repository<Order>().Delete(order);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
