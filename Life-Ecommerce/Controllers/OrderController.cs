﻿using Application.Services.Order;
using Domain.DTOs.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Life_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(int? userId, string cartIdentifier, OrderDto orderDto)
        {
            if (orderDto == null)
                return BadRequest("Order data is required.");

            var result = await _orderService.CreateOrder(userId, cartIdentifier, orderDto);

            if (result)
                return Ok("Order created successfully.");
            else
                return BadRequest("Failed to create order.");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var orders = await _orderService.GetOrdersByUserId(userId);
            return Ok(orders);
        }

      
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string newStatus)
        {
            var result = await _orderService.UpdateOrderStatus(id, newStatus);
            if (result)
                return Ok();

            return BadRequest("Failed to update order status");
        }
    }
}