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
    public class CartService 
        //: ICartService
    {
        //private readonly IUnitOfWork _unitOfWork;
        //private readonly IMapper _mapper;

        //public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        //{
        //    _unitOfWork = unitOfWork;
        //    _mapper = mapper;
        //}

        //// Get all carts for a user
        //public async Task<IEnumerable<CartResponseModel>> GetAllCartsAsync(int userId)
        //{
        //    var carts = await _unitOfWork.Repository<Cart>()
        //        .GetAll()
        //        .Where(c => c.UserID == userId)
        //        .Include(c => c.Products)
        //        .ToListAsync();

        //    return _mapper.Map<IEnumerable<CartResponseModel>>(carts);
        //}

        //// Get cart by ID
        //public async Task<CartResponseModel?> GetCartByIdAsync(int cartId)
        //{
        //    var cart = await _unitOfWork.Repository<Cart>()
        //        .GetAll()
        //        .Include(c => c.Products)
        //        .FirstOrDefaultAsync(c => c.Id == cartId);

        //    return cart == null ? null : _mapper.Map<CartResponseModel>(cart);
        //}

        //// Create a new cart
        //public async Task<CartResponseModel> CreateCartAsync(CartRequestModel request)
        //{
        //    var cart = _mapper.Map<Cart>(request);

        //    cart.Products = await _unitOfWork.Repository<Product>()
        //        .GetAll()
        //        .Where(p => request.ProductIDs.Contains(p.Id))
        //        .ToListAsync();

        //    await _unitOfWork.Repository<Cart>().InsertAsync(cart);
        //    await _unitOfWork.SaveChangesAsync();

        //    return _mapper.Map<CartResponseModel>(cart);
        //}

        //// Update cart
        //public async Task<CartResponseModel?> UpdateCartAsync(int cartId, CartRequestModel request)
        //{
        //    var cart = await _unitOfWork.Repository<Cart>().GetById(cartId);
        //    if (cart == null) return null;

        //    cart.Quantity = request.Quantity;
        //    cart.Products = await _unitOfWork.Repository<Product>()
        //        .GetAll()
        //        .Where(p => request.ProductIDs.Contains(p.Id))
        //        .ToListAsync();

        //    await _unitOfWork.Repository<Cart>().UpdateAsync(cart);
        //    await _unitOfWork.SaveChangesAsync();

        //    return _mapper.Map<CartResponseModel>(cart);
        //}

        //// Delete cart
        //public async Task<bool> DeleteCartAsync(int cartId)
        //{
        //    var cart = await _unitOfWork.Repository<Cart>().GetById(cartId);
        //    if (cart == null) return false;

        //    await _unitOfWork.Repository<Cart>().DeleteAsync(cart);
        //    await _unitOfWork.SaveChangesAsync();

        //    return true;
        //}

        //// Add products to cart
        //public async Task<CartResponseModel?> AddProductsToCartAsync(int cartId, List<int> productIds)
        //{
        //    var cart = await _unitOfWork.Repository<Cart>()
        //        .GetAll()
        //        .Include(c => c.Products)
        //        .FirstOrDefaultAsync(c => c.Id == cartId);
        //    if (cart == null) return null;

        //    var products = await _unitOfWork.Repository<Product>()
        //        .GetAll()
        //        .Where(p => productIds.Contains(p.Id))
        //        .ToListAsync();

        //    if (cart.Products == null)
        //    {
        //        cart.Products = new List<Product>();
        //    }

        //    foreach (var product in products)
        //    {
        //        cart.Products.Add(product);
        //    }
        //    await _unitOfWork.Repository<Cart>().UpdateAsync(cart);
        //    await _unitOfWork.SaveChangesAsync();

        //    return _mapper.Map<CartResponseModel>(cart);
        //}

        //// Add a single product to cart
        //public async Task<CartResponseModel?> AddProductToCartAsync(int cartId, int productId)
        //{
        //    var cart = await _unitOfWork.Repository<Cart>()
        //        .GetAll()
        //        .Include(c => c.Products)
        //        .FirstOrDefaultAsync(c => c.Id == cartId);

        //    if (cart == null) return null;

        //    var product = await _unitOfWork.Repository<Product>().GetById(productId);
        //    if (product == null) return null;

        //    if (cart.Products == null)
        //        cart.Products = new List<Product>();

        //    cart.Products.Add(product);

        //    await _unitOfWork.Repository<Cart>().UpdateAsync(cart);
        //    await _unitOfWork.SaveChangesAsync();

        //    return _mapper.Map<CartResponseModel>(cart);
        //}
    }
}
