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
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Get all carts for a user
        public async Task<IEnumerable<CartResponseModel>> GetAllCartsAsync(int userId)
        {
            var carts = await _unitOfWork.Repository<Cart>()
                .GetAll()
                .Where(c => c.UserID == userId)
                .Include(c => c.Products) 
                .ToListAsync();

            return _mapper.Map<IEnumerable<CartResponseModel>>(carts);
        }


        // Get cart by ID
        public async Task<CartResponseModel?> GetCartByIdAsync(int cartId)
        {
            var cart = await _unitOfWork.Repository<Cart>()
                .GetAll()
                .Include(c => c.Products) 
                .FirstOrDefaultAsync(c => c.Id == cartId);

            return cart == null ? null : _mapper.Map<CartResponseModel>(cart);
        }


        // Create a new cart item
        public async Task<CartResponseModel> CreateCartAsync(CartRequestModel request)
        {
            var cart = _mapper.Map<Cart>(request);
            await _unitOfWork.Repository<Cart>().InsertAsync(cart);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0) throw new Exception("Failed to create cart.");

            return _mapper.Map<CartResponseModel>(cart);
        }

        // Update quantity in the cart
        public async Task<CartResponseModel?> UpdateCartAsync(int cartId, CartRequestModel request)
        {
            var existingCart = await _unitOfWork.Repository<Cart>().GetById(cartId);
            if (existingCart == null) return null;

            existingCart.Quantity = request.Quantity;
            await _unitOfWork.Repository<Cart>().Update(existingCart, cartId);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CartResponseModel>(existingCart);
        }

        // Remove a product from the cart
        public async Task<bool> RemoveProductFromCartAsync(int cartId)
        {
            var cart = await _unitOfWork.Repository<Cart>().GetById(cartId);
            if (cart == null) return false;

            _unitOfWork.Repository<Cart>().Delete(cart);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
