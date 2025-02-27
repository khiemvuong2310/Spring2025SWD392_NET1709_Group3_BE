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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ProductResponseModel>> GetAllProductsAsync(ProductQueryRequest request)
        {
            var productsQuery = _unitOfWork.Repository<Product>()
                .GetAll()
                .Where(p => p.IsDelete == request.IsDeleted)
                .Where(p => request.Name == null || p.Name.Contains(request.Name))
                .Where(p => request.MinPrice == null || p.Price >= request.MinPrice)
                .Where(p => request.MaxPrice == null || p.Price <= request.MaxPrice)
                .Where(p => request.BrandId == null || p.BrandId == request.BrandId)
                .Where(p => request.CategoryId == null || p.CategoryId == request.CategoryId)
                .Where(p => request.PackagingId == null || p.PackagingId == request.PackagingId)
                .Where(p => request.FormulationTypeId == null || p.FormulationTypeId == request.FormulationTypeId)
                .Where(p => !request.TagIds.Any() || p.ProductTags.Any(pt => request.TagIds.Contains(pt.TagId)))
                .Where(p => !request.SkinTypeIds.Any() || p.ProductSkinTypes.Any(ps => request.SkinTypeIds.Contains(ps.SkinTypeId)))
                .Where(p => !request.IngredientIds.Any() || p.ProductIngredients.Any(pi => request.IngredientIds.Contains(pi.IngredientId)))
                .ProjectTo<ProductResponseModel>(_mapper.ConfigurationProvider);
            switch (request.SortBy)
            {
                case ProductSort.Default:
                    productsQuery = request.Ascending ? productsQuery.OrderBy(p => p.Id) : productsQuery.OrderByDescending(p => p.Id);
                    break;
                case ProductSort.Name:
                    productsQuery = request.Ascending ? productsQuery.OrderBy(p => p.Name) : productsQuery.OrderByDescending(p => p.Name);
                    break;
                case ProductSort.Price:
                    productsQuery = request.Ascending ? productsQuery.OrderBy(p => p.Price) : productsQuery.OrderByDescending(p => p.Price);
                    break;
                case ProductSort.StockQuantity:
                    productsQuery = request.Ascending ? productsQuery.OrderBy(p => p.StockQuantity) : productsQuery.OrderByDescending(p => p.StockQuantity);
                    break;
                default:
                    break;
            }

            return await PaginatedList<ProductResponseModel>.CreateAsync(productsQuery, request.PageNumber, request.PageSize);
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
            if (model.ProductTags?.Count > 0)
            {
                foreach (var tagId in model.ProductTags)
                {
                    await _unitOfWork.Repository<ProductTag>().InsertAsync(new ProductTag { ProductId = product.Id, TagId = tagId });
                }
            }
            if (model.ProductSkinTypes?.Count > 0)
            {
                foreach (var skinTypeId in model.ProductSkinTypes)
                {
                    await _unitOfWork.Repository<ProductSkinType>().InsertAsync(new ProductSkinType { ProductId = product.Id, SkinTypeId = skinTypeId });
                }
            }
            if (model.ProductIngredients?.Count > 0)
            {
                foreach (var ingredientId in model.ProductIngredients)
                {
                    await _unitOfWork.Repository<ProductIngredient>().InsertAsync(new ProductIngredient { ProductId = product.Id, IngredientId = ingredientId });
                }
            }
            if (model.ProductImages?.Count > 0)
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
            return await _unitOfWork.Repository<Product>().AsQueryable()
                .Where(i => i.Id == id)
                .ProjectTo<ProductResponseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
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

