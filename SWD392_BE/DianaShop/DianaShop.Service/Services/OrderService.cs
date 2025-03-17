using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            return await _unitOfWork.Repository<Order>()
                .GetQueryable()
                .ProjectTo<OrderResponseModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }


        public async Task<IEnumerable<OrderResponseModel>> GetAllOrderByUser(int userId)
        {
            return await _unitOfWork.Repository<Order>()
                .AsQueryable()
                .Where(cp => cp.UserID == userId)
                .ProjectTo<OrderResponseModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<OrderResponseModel?> GetOrderById(int orderId)
        {
            return await _unitOfWork.Repository<Order>()
                .GetQueryable()
                .Where(cp => cp.Id == orderId)
                .ProjectTo<OrderResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
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
                ShipAmount = request.Shipprice ?? 100,
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

            if (order.StatusId == 5)
            {
                throw new Exception("Order is complete, it can't be changed!");
            }
            var user = await _unitOfWork.Repository<User>()
                .GetQueryable()
                .Where(cp => cp.Id == order.UserID)
                .FirstOrDefaultAsync();

            order.StatusId = status;
            order.UpdateDate = DateTime.Now;
            await _unitOfWork.Repository<Order>().UpdateAsync(order);

            user.wallet += (order.FinalAmount * 0.05m);
            await _unitOfWork.Repository<User>().UpdateAsync(user);

            if (status == 6)
            {
                if (order.VoucherID > 0 && order.VoucherID != null)
                {
                    var userVoucher = await _unitOfWork.Repository<VoucherStorage>()
                    .GetQueryable()
                    .Where(cp => cp.UserID == order.UserID)
                    .FirstOrDefaultAsync();
                    if (userVoucher == null) throw new Exception("User not have this voucher storage");


                    var storage = await _unitOfWork.Repository<Storage>()
                    .GetQueryable()
                    .Where(cp => cp.VoucherId == order.VoucherID && cp.VoucherStorageId == userVoucher.Id)
                    .FirstOrDefaultAsync();
                    if (storage == null) throw new Exception("User not have this voucher");

                    storage.Quantity++;
                    await _unitOfWork.Repository<Storage>().UpdateAsync(storage);
                }
            }

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

            if (order.VoucherID > 0 && order.VoucherID != null)
            {
                throw new Exception("There is a voucher is applied before!");
            }

            var userVoucher = await _unitOfWork.Repository<VoucherStorage>()
            .GetQueryable()
            .Where(cp => cp.UserID == order.UserID)
            .FirstOrDefaultAsync();
            if (userVoucher == null) throw new Exception("User not have this voucher storage");

            var storage = await _unitOfWork.Repository<Storage>()
            .GetQueryable()
            .Where(cp => cp.VoucherId == voucherId && cp.VoucherStorageId == userVoucher.Id)
            .FirstOrDefaultAsync();
            if (storage == null || storage.Quantity == 0) throw new Exception("User not have this voucher");

            order.VoucherID = voucherId;

            var voucher = await _unitOfWork.Repository<Voucher>()
                    .FindAsync(cp => cp.Id == order.VoucherID);
            if (voucher == null)
            {
                throw new Exception("voucher not found");
            }

            var reducedAmount = order.TotalAmount;
            if (order.TotalAmount >= (decimal)voucher.MinimumPurchase)
            {
                reducedAmount = (order.TotalAmount * (decimal)(voucher.DiscountPercentage/100));
                storage.Quantity--;
            }
            order.FinalAmount = order.TotalAmount + order.ShipAmount - reducedAmount;
            order.UpdateDate = DateTime.Now;
            await _unitOfWork.Repository<Order>().UpdateAsync(order);
            await _unitOfWork.Repository<Storage>().UpdateAsync(storage);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<OrderResponseModel>(order);
        }
    }
}
