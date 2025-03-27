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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
                await _unitOfWork.Repository<OrderDetail>().UpdateAsync(existingProduct);
            }
            else
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = request.OrderID,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    UnitPrice = product.Price
                };
                await _unitOfWork.Repository<OrderDetail>().InsertAsync(orderDetail);
            }

            //cập nhật order sau khi thêm product

            order.Quantity += request.Quantity;
            order.TotalAmount += (product.Price * request.Quantity);
            order.FinalAmount = (order.ShipAmount + order.TotalAmount);
            order.UpdateDate = DateTime.Now;
            await _unitOfWork.Repository<Order>().UpdateAsync(order);

            //cập nhật product sau khi mua
            //var inStock = product.ProductBatches.Where(pb => pb.Status == true && !pb.IsDelete).Sum(pb => pb.Quantity) ?? 0;
            var batchs = await _unitOfWork.Repository<ProductBatch>().GetAll().Where(p => p.ProductID == product.Id && p.Status == true && !p.IsDelete && p.ExpiryDate > DateTime.Now).ToListAsync();
            int inStock = 0;
            foreach (var batch in batchs)
            {
                inStock += (batch.Quantity ?? 0);
            }

            if (inStock < request.Quantity)
            {
                throw new KeyNotFoundException($"Product with Id {request.ProductId} are insufficient unit in stock.");
            }

            var batchList = new List<ProductBatch>();
            var tmpQuantity = request.Quantity;
            while (tmpQuantity != 0)
            {
                var batch = await _unitOfWork.Repository<ProductBatch>().AsQueryable()
                .Where(b => b.ProductID == request.ProductId && b.Quantity != 0 && !b.IsDelete && b.Status == true && b.ExpiryDate > DateTime.Now)
                .OrderBy(pb => pb.ImportDate)
                .FirstOrDefaultAsync();
                
                if (batch == null)
                {
                    throw new KeyNotFoundException($"Product with Batch {request.ProductId} not found.");
                }

                if (batch.Quantity < request.Quantity)
                {
                    tmpQuantity -= (batch.Quantity ?? 0);
                    batch.Quantity = 0;
                } else
                {
                    batch.Quantity -= tmpQuantity;
                    tmpQuantity = 0;
                }
                batchList.Add(batch);
            }
            await _unitOfWork.Repository<ProductBatch>().UpdateRangeAsync(batchList);

            await _unitOfWork.SaveChangesAsync();

            var AddedResult = await _unitOfWork.Repository<OrderDetail>()
                .FindAsync(x => x.OrderId == request.OrderID && x.ProductId == request.ProductId);

            return _mapper.Map<OrderDetailResponseModel>(AddedResult);
        }

        public async Task<bool> ClearOrderDetail(int orderId)
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

            //cập nhật order sau khi làm sạch
            order.Quantity = 0;
            order.TotalAmount = 0;
            order.FinalAmount = 0;
            order.UpdateDate = DateTime.Now;

            //cập nhật product sau khi xóa
            var orderDetailList = await orderDetails.ToListAsync(); // ✅ Nạp dữ liệu trước
            var batchList = new List<ProductBatch>();
            var updateDetailList = new List<OrderDetail>();

            foreach (var item in orderDetailList)
            {
                var batch = await _unitOfWork.Repository<ProductBatch>().AsQueryable()
                    .Where(b => b.ProductID == item.ProductId && !b.IsDelete && b.Status == true && b.ExpiryDate > DateTime.Now)
                    .OrderBy(pb => pb.ImportDate)
                    .FirstOrDefaultAsync();

                if (batch == null)
                {
                    throw new KeyNotFoundException($"Product with Batch {item.ProductId} not found.");
                }
                batch.Quantity += item.Quantity;
                batchList.Add(batch);
                item.IsDelete = true;
                updateDetailList.Add(item);
            }

            //_unitOfWork.Repository<OrderDetail>().DeleteRange(orderDetails);
            await _unitOfWork.Repository<OrderDetail>().UpdateRangeAsync(updateDetailList);
            await _unitOfWork.Repository<Order>().UpdateAsync(order);
            await _unitOfWork.Repository<ProductBatch>().UpdateRangeAsync(batchList);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderDetailResponseModel>> GetAllOrderDetailsByOrderId(int orderId)
        {
            var orderDetails = await _unitOfWork.Repository<OrderDetail>()
                .GetQueryable()
                .Where(cp => cp.OrderId == orderId && !cp.IsDelete)
                .Include(cp => cp.Product)
                .ToListAsync();
            return _mapper.Map<IEnumerable<OrderDetailResponseModel>>(orderDetails);
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

            //cập nhật order sau khi xóa
            order.Quantity -= orderDetail.Quantity;
            order.TotalAmount -= (orderDetail.Quantity * orderDetail.UnitPrice);
            order.FinalAmount += (order.TotalAmount + order.ShipAmount);
            order.UpdateDate = DateTime.Now;

            //cập nhật product sau khi xóa
            var batch = await _unitOfWork.Repository<ProductBatch>().AsQueryable()
                .Where(b => b.ProductID == productId && b.ExpiryDate > DateTime.Now && b.IsDelete == false && b.Status == true)
                .OrderBy(pb => pb.ImportDate)
                .FirstOrDefaultAsync();
            if (batch == null)
            {
                throw new KeyNotFoundException($"Product with Batch {productId} not found.");
            }
            batch.Quantity += orderDetail.Quantity;

            orderDetail.IsDelete = true;
            await _unitOfWork.Repository<OrderDetail>().UpdateAsync(orderDetail);
            await _unitOfWork.Repository<Order>().UpdateAsync(order);
            await _unitOfWork.Repository<ProductBatch>().UpdateAsync(batch);
            await _unitOfWork.SaveChangesAsync();
            return true;
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

            //cập nhật product sau khi cập nhật
            var batch = await _unitOfWork.Repository<ProductBatch>().AsQueryable()
                .Where(b => b.ProductID == request.ProductId && b.Status == true && b.ExpiryDate > DateTime.Now)
                .OrderBy(pb => pb.ImportDate)
                .FirstOrDefaultAsync();
            if (batch == null)
            {
                throw new KeyNotFoundException($"Product with Batch are insufficient unit in stock.");
            }
            if (request.Quantity > orderDetail.Quantity)
            {
                var minus = request.Quantity - orderDetail.Quantity;
                if (batch.Quantity < minus) throw new KeyNotFoundException($"Product with Batch are insufficient unit in stock.");
                batch.Quantity -= minus;
            }
            else
            {
                batch.Quantity += (orderDetail.Quantity - request.Quantity);
            }

            order.Quantity -= orderDetail.Quantity;
            order.TotalAmount -= (orderDetail.Quantity * orderDetail.UnitPrice);
            orderDetail.Quantity = request.Quantity;
            order.Quantity += request.Quantity;
            order.TotalAmount += (request.Quantity * orderDetail.UnitPrice);
            order.UpdateDate = DateTime.Now;

            await _unitOfWork.Repository<OrderDetail>().UpdateAsync(orderDetail);
            await _unitOfWork.Repository<Order>().UpdateAsync(order);
            await _unitOfWork.Repository<ProductBatch>().UpdateAsync(batch);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderDetailResponseModel>(orderDetail);
        }
    }
}
