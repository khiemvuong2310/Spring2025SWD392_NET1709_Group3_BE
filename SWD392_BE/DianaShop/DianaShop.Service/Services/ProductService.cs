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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductResponseModel>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Repository<Product>()
                .GetAll()
                .Where(p => !p.IsDelete)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Packaging)
                .Include(p => p.FormulationType)
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductSkinTypes).ThenInclude(ps => ps.SkinType)
                .Include(p => p.ProductIngredients).ThenInclude(pi => pi.Ingredient)
                .Include(p => p.ProductImages).ThenInclude(pi => pi.Image)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductResponseModel>>(products);
        }

        private async Task<Product> HandleValidation(ProductRequestModel model)
        {
            // Validate required foreign keys
            var brand = await _unitOfWork.Repository<Brand>().GetById(model.BrandId);
            if (brand == null) throw new Exception("Invalid BrandId");

            var category = await _unitOfWork.Repository<Category>().GetById(model.CategoryId);
            if (category == null) throw new Exception("Invalid CategoryId");

            var packaging = await _unitOfWork.Repository<Packaging>().GetById(model.PackagingId);
            if (packaging == null) throw new Exception("Invalid PackagingId");

            var formulationType = await _unitOfWork.Repository<FormulationType>().GetById(model.FormulationTypeId);
            if (formulationType == null) throw new Exception("Invalid FormulationTypeId");

            // Map Request Model to Product Entity
            var product = _mapper.Map<Product>(model);

            // Assign related entities
            product.Brand = brand;
            product.Category = category;
            product.Packaging = packaging;
            product.FormulationType = formulationType;

            // Handle Collections (e.g., Tags, Images)
            // Handle Many-to-Many relationships
            if (model.ProductTags?.Any() == true)
            {
                foreach (var tagId in model.ProductTags)
                {
                    await _unitOfWork.Repository<ProductTag>().InsertAsync(new ProductTag { ProductId = product.Id, TagId = tagId });
                }
            }
            if (model.ProductSkinTypes?.Any() == true)
            {
                foreach (var skinTypeId in model.ProductSkinTypes)
                {
                    await _unitOfWork.Repository<ProductSkinType>().InsertAsync(new ProductSkinType { ProductId = product.Id, SkinTypeId = skinTypeId });
                }
            }
            if (model.ProductIngredients?.Any() == true)
            {
                foreach (var ingredientId in model.ProductIngredients)
                {
                    await _unitOfWork.Repository<ProductIngredient>().InsertAsync(new ProductIngredient { ProductId = product.Id, IngredientId = ingredientId });
                }
            }
            if (model.ProductImages?.Any() == true)
            {
                foreach (var imageUrl in model.ProductImages)
                {
                    await _unitOfWork.Repository<ProductImage>().InsertAsync(new ProductImage { ProductId = product.Id, ImageId = imageUrl });
                }
            }
            return product;
        }
        public async Task<ProductResponseModel> AddProductAsync(ProductRequestModel model)
        {
            var product = await HandleValidation(model);
            // Add to DB
            await _unitOfWork.Repository<Product>().InsertAsync(product);
            await _unitOfWork.CommitAsync();

            // Return response
            return _mapper.Map<ProductResponseModel>(product);
        }

        public async Task<ProductResponseModel?> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>()
                .GetAll()
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Packaging)
                .Include(p => p.FormulationType)
                .Include(p => p.ProductBatches)
                .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag)
                .Include(p => p.ProductSkinTypes).ThenInclude(ps => ps.SkinType)
                .Include(p => p.ProductIngredients).ThenInclude(pi => pi.Ingredient)
                .Include(p => p.ProductImages).ThenInclude(pi => pi.Image)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product != null) return _mapper.Map<ProductResponseModel>(product);
             else return null;
        }

        public async Task UpdateProductAsync(int id, ProductRequestModel model)
        {
            var product = await _unitOfWork.Repository<Product>().GetById(id);
            if (product == null) throw new ("Product not found");
            _mapper.Map(model, product);
            // Update to DB
            await _unitOfWork.Repository<Product>().Update(product, id);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetById(id);
            if (product == null) throw new Exception("Product not found");
            product.IsDelete = true;
            await _unitOfWork.Repository<Product>().Update(product, id);
            await _unitOfWork.CommitAsync();
        }
    }
}

