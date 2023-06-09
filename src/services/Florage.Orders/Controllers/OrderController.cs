﻿using Florage.Orders.Contracts;
using Florage.Orders.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Florage.Orders.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("current-user")]
        [Authorize]
        public async Task<ActionResult<IReadOnlyCollection<GetOrderDto>>> GetCurrentUsersOrders()
        {
            IReadOnlyCollection<GetOrderDto> orders = await _orderService.GetCurrentUsersOrdersAsync();
            return Ok(orders);
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IReadOnlyCollection<GetOrderDto>>> GetAllAsync()
        {
            IReadOnlyCollection<GetOrderDto> orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        } 

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GetCreatedOrderDto>> CreateAsync(CreateOrderDto orderDto)
        {
            GetCreatedOrderDto getCreatedOrderDto = await _orderService.CreateAsync(orderDto);
            return new ObjectResult(getCreatedOrderDto) { StatusCode = 201 };
        }
        
        [HttpPost("approved")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetCreatedOrderDto>> ApproveOrderAsync(string orderId)
        {
            await _orderService.SetOrderAsApprovedAsync(orderId);
            return Ok();
        }
    }
}
