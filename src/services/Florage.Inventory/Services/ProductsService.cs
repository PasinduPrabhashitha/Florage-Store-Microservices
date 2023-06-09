﻿using AutoMapper;
using Florage.Inventory.Contracts;
using Florage.Inventory.Dtos;
using Florage.Inventory.Models;
using Florage.Shared.Contracts;

namespace Florage.Inventory.Services
{
    public class ProductsService : IProductService
    {
        private readonly IRepository<Product> _repository;
        private readonly IMapper _mapper;
        private readonly IProductsMessagingService _productsMessagingService;

        public ProductsService(IRepository<Product> genericRepository, IMapper mapper, IProductsMessagingService productsMessagingService)
        {
            _repository = genericRepository;
            _mapper = mapper;
            _productsMessagingService = productsMessagingService;
        }

        public async Task<GetProductDto> CreateAsync(CreateProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto); 
            Product insertedProduct = await _repository.CreateAsync(product);
            GetProductDto insertedMappedProduct = _mapper.Map<GetProductDto>(insertedProduct);

            //Publish created product
            _productsMessagingService.PublishCreatedProduct(insertedProduct);
            return insertedMappedProduct;
        }

        public async Task DeleteAsync(string productId)
        {
            await _repository.DeleteAsync(productId);
            _productsMessagingService.PublishDeleteProduct(productId);
        }

        public async Task<IReadOnlyCollection<GetProductDto>> GetAllAsync()
        {
            IReadOnlyCollection<Product> products = await _repository.GetAllAsync();
            IReadOnlyCollection<GetProductDto> mappedProducts = _mapper.Map<IReadOnlyCollection<GetProductDto>>(products);
            return mappedProducts;
        }

        public async Task<GetProductDto> GetByIdAsync(string id)
        {
            // Get product by custom product id
            Product product = await _repository.GetByIdAsync(id);
            GetProductDto mappedProduct = _mapper.Map<GetProductDto>(product);
            return mappedProduct;
        }

        public async Task UpdateAsync(string productId,UpdateProductDto updateProductDto)
        {
            Product productToUpdate = _mapper.Map<Product>(updateProductDto); 
            
            productToUpdate.Id = productId; 
            await _repository.UpdateAsync(productId, productToUpdate);
            _productsMessagingService.PublishUpdateProduct(productToUpdate);
        }
    }
}
