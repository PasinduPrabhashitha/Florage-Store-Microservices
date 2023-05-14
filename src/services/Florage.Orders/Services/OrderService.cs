﻿using AutoMapper;
using Florage.Orders.Contracts;
using Florage.Orders.Dtos;
using Florage.Orders.Models;
using Florage.Orders.Utils;
using Florage.Shared.Contracts;

namespace Florage.Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _repository;
        private readonly IRepository<Product> _productsRepository;
        private readonly IMapper _mapper;
        private readonly IOrderPublishingService _orderPublishingService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IOrderCommisionService _orderCommisionService;

        public OrderService(IRepository<Order> repository, 
            IRepository<Product> productsRepository, 
            IMapper mapper,
            IOrderPublishingService orderPublishingService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IOrderCommisionService orderCommisionService
        ){
            _repository = repository;
            _productsRepository = productsRepository;
            _repository.SetCollectionName(Constants.OrdersCollectionName);
            _productsRepository.SetCollectionName(Constants.ProductsCollectionName);
            _mapper = mapper;
            _orderPublishingService = orderPublishingService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _orderCommisionService = orderCommisionService;
        }
        public async Task<GetCreatedOrderDto> CreateAsync(CreateOrderDto orderDto)
        { 
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            float totalPrice = 0;


            string userId = _httpContextAccessor.HttpContext.User.Identity.Name;

            User user = await _userService.GetUserById(userId);

            foreach (var orderProductDto in orderDto.Products)
            {
                Product product = await _productsRepository.GetByIdAsync(orderProductDto.Product);
                OrderProduct orderProduct = new OrderProduct
                {
                    Product = product,
                    Quantity = orderProductDto.Quantity
                };

                totalPrice += product.Price * orderProductDto.Quantity;
                orderProducts.Add(orderProduct);
            }

            Order order = new Order
            {
                Products = orderProducts,
                TotalPrice = totalPrice,
                User = user,
            };

            Order createdOrder = await _repository.CreateAsync(order);
            _orderPublishingService.PublishCreatedOrder(createdOrder);
            
            GetCreatedOrderDto createdOrderDto = new GetCreatedOrderDto { Id=createdOrder.Id };
            return createdOrderDto;
        }

        public async Task<IReadOnlyCollection<GetOrderDto>> GetAllOrdersAsync()
        {
            IReadOnlyCollection<Order> orders = await _repository.GetAllAsync();
            IReadOnlyCollection<GetOrderDto> getOrders = _mapper.Map<IReadOnlyCollection<GetOrderDto>>(orders);
            return getOrders;
        }

        public async Task SetOrderAsApprovedAsync(string orderId)
        {
            Order order = await _repository.GetByIdAsync(orderId);

            if (order == null)
                throw new KeyNotFoundException();

            order.Status = nameof(OrderStatus.Approved);
            await _repository.UpdateAsync(orderId, order);
        }

        public async Task SetOrderAsPaidAsync(string orderId)
        {
            Order order = await _repository.GetByIdAsync(orderId);

            if (order == null)
                throw new KeyNotFoundException();
            
            order.Status = nameof(OrderStatus.Paid);
            await _repository.UpdateAsync(orderId, order);
        }

        public async Task CalculateAndSaveCommisionAsync(string orderId)
        {
            Order order = await _repository.GetByIdAsync(orderId);
            double commision = CalculateCommision(order);

            OrderCommisions orderCommisions = new OrderCommisions
            {
                OrderId = orderId,
                Commision = commision,
                UserId = order.User.Id
            };

            await _orderCommisionService.CreateAsync(orderCommisions);
        }

        private double CalculateCommision(Order order)
        {
            double commision = 0;
            foreach (var orderProduct in order.Products)
            {
                var commisionFromProduct = orderProduct.Product.Price - orderProduct.Product.BuyPrice;
                commision += commisionFromProduct;
            }
            return commision;
        }

        public async Task<IReadOnlyCollection<GetOrderDto>> GetCurrentUsersOrdersAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.Name;
            IReadOnlyCollection<GetOrderDto> orders =  _mapper.Map<IReadOnlyCollection<GetOrderDto>>(await _repository.FilterAsync(order => order.User.Id == userId));
            return orders;
        }
    }
}
