using AutoMapper;
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

        public async Task<IEnumerable<OrderResponseModel>> GetAllOrder()
        {
            var orders = await _unitOfWork.Repository<Order>()
                .GetQueryable()
                .ToListAsync();
            return _mapper.Map<IEnumerable<OrderResponseModel>>(orders);
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

        public async Task<OrderResponseModel> CreateOrder(OrderRequestModel request)
        {
            if (request == null)
                throw new ArgumentException("Invalid request data.");
            if (request.UserID <= 0)
                throw new ArgumentException("UserID must be greater than 0.");

            // Kiểm tra xem User đã có giỏ hàng chưa
            var unpaidOrder = await _unitOfWork.Repository<Order>()
                .FindAsync(c => c.UserID == request.UserID && c.StatusId == 1);

            if (unpaidOrder != null)
                throw new InvalidOperationException("User already has an unpaid order.");

            var order = new Order
            {
                UserID = request.UserID,
                Address = request.Address,
                Quantity = 0,
                TotalAmount = 0,
                ShipAmount = 16000,
                FinalAmount = 0,
                OrderDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                StatusId = 3,
                Details = new List<OrderDetail>()
            };

            await _unitOfWork.Repository<Order>().InsertAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderResponseModel>(order);
        }

        public async Task<bool> RemoveOrder(int orderId)
        {
            var order = await _unitOfWork.Repository<Order>()
                .FindAsync(c => c.Id == orderId);
            if (order == null)
            {
                return false;
            }

            var orderDetails = _unitOfWork.Repository<OrderDetail>()
                .GetQueryable()
                .Where(cp => cp.OrderId == orderId);
            if (await orderDetails.AnyAsync())
            {
                //_unitOfWork.Repository<OrderDetail>().DeleteRange(orderDetails);
                throw new ArgumentException("There are order detail isn't delete.");
            }

            _unitOfWork.Repository<Order>().Delete(order);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<OrderResponseModel> UpdateOrder(int orderId, int status)
        {
            var order = await _unitOfWork.Repository<Order>()
                .GetQueryable()
                .Where(cp => cp.Id == orderId)
                .FirstOrDefaultAsync();

            if (order == null)
                throw new Exception("Order not found");

            order.StatusId = status;
            order.UpdateDate = DateTime.Now;
            await _unitOfWork.Repository<Order>().UpdateAsync(order);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<OrderResponseModel>(order);
        }

        public async Task<OrderResponseModel> AddVoucherToOrder(int orderId, int voucherId)
        {
            var order = await _unitOfWork.Repository<Order>()
                .GetQueryable()
                .Where(cp => cp.Id == orderId)
                .FirstOrDefaultAsync();
            if (order == null)
                throw new Exception("Order not found");

            var storage = await _unitOfWork.Repository<Storage>()
                .GetQueryable()
                .Where(cp => cp.VoucherId == voucherId)
                .FirstOrDefaultAsync();
            if (storage == null) throw new Exception("User not have this voucher");

            order.VoucherID = voucherId;
            if (order.VoucherID > 0)
            {
                var voucher = await _unitOfWork.Repository<Voucher>()
                    .FindAsync(cp => cp.Id == order.VoucherID);
                if (order.TotalAmount >= (decimal)voucher.MinimumPurchase)
                {
                    order.FinalAmount -= (order.TotalAmount * (decimal)voucher.DiscountPercentage);
                }
            }
            order.UpdateDate = DateTime.Now;
            await _unitOfWork.Repository<Order>().UpdateAsync(order);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<OrderResponseModel>(order);
        }
    }
}
