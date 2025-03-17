using AutoMapper;
using AutoMapper.QueryableExtensions;
using DianaShop.Data.Entities;
using DianaShop.Repository.UnitOfWork;
using DianaShop.Service.Interfaces;
using DianaShop.Service.ReponseModel;
using DianaShop.Service.RequestModel;
using DianaShop.Service.RequestModel.QueryRequest;
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
        private readonly IImageService _imageService;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<PaginatedResponse<ProductResponseModel>> GetAllProductsAsync(ProductQueryRequest request)
        {
            var productsQuery = _unitOfWork.Repository<Product>()
                .GetAll()
                .Where(p => request.IsDeleted == null || p.IsDelete == request.IsDeleted)
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

            return await PaginatedResponse<ProductResponseModel>.CreateAsync(productsQuery, request.PageNumber, request.PageSize);
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

            // Handle Many-to-Many relationships
            if (model.ProductTagIds?.Count > 0)
            {
                product.ProductTags = new List<ProductTag>();
                foreach (var tagId in model.ProductTagIds)
                {

                    var tag = await _unitOfWork.Repository<Tag>().GetById(tagId);
                    if (tag != null)
                    {
                        product.ProductTags.Add(new ProductTag { Tag = tag });
                    }
                }
            }
            if (model.ProductSkinTypeIds?.Count > 0)
            {
                product.ProductSkinTypes = new List<ProductSkinType>();
                foreach (var skinTypeId in model.ProductSkinTypeIds)
                {
                    var skinType = await _unitOfWork.Repository<SkinType>().GetById(skinTypeId);
                    if (skinType != null)
                    {
                        product.ProductSkinTypes.Add(new ProductSkinType { SkinType = skinType });
                    }
                }
            }
            if (model.ProductIngredientIds?.Count > 0)
            {
                product.ProductIngredients = new List<ProductIngredient>();
                foreach (var ingredientId in model.ProductIngredientIds)
                {
                    var ingredient = await _unitOfWork.Repository<Ingredient>().GetById(ingredientId);
                    if (ingredient != null)
                    {
                        product.ProductIngredients.Add(new ProductIngredient { Ingredient = ingredient });
                    }
                }
            }
            if (model.ProductImageFiles?.Count > 0)
            {
                product.ProductImages = new List<ProductImage>();               
                foreach (var imageUrl in model.ProductImageFiles)
                {
                    var image = await _imageService.ConvertToImageAsync(imageUrl);
                    if (image != null)
                    {
                        product.ProductImages.Add(new ProductImage { Image = image });
                    }                 
                }
            }
            return product;
        }
        public async Task<ProductResponseModel?> AddProductAsync(ProductRequestModel model)
        {
            var product = await HandleValidation(model);
            // Add to DB
            await _unitOfWork.Repository<Product>().InsertAsync(product);
            await _unitOfWork.CommitAsync();
            var response = await GetProductByIdAsync(product.Id);
            // Return response
            return response;
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
            var existingProduct = await _unitOfWork.Repository<Product>().AsQueryable().Where(p => p.Id == id).Include(p => p.ProductImages).FirstOrDefaultAsync();
            var updatedProduct = await HandleValidation(model);

            // Preserve the existing images if none are provided in the model
            if (model.ProductImageFiles == null || !model.ProductImageFiles.Any())
            {
                updatedProduct.ProductImages = existingProduct.ProductImages; // Preserve the old images
            }

            // Update the product with the preserved images if necessary
            _mapper.Map(model, existingProduct); // Map the new values to the existing product entity
            existingProduct.ProductImages = updatedProduct.ProductImages;

            await _unitOfWork.Repository<Product>().Update(existingProduct, id);
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

