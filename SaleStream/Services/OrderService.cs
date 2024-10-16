using MongoDB.Driver;
using SaleStream.Models;
using SaleStream.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaleStream.Services
{
    public class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly ProductService _productService;

        public OrderService(OrderRepository orderRepository, ProductService productService)
        {
            _orderRepository = orderRepository;
            _productService = productService;
        }

        public async Task CreateOrderAsync(Order order)
        {
            foreach (var item in order.OrderItems)
            {
                // Decrease available quantity in the product
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.AvailableQuantity -= item.Quantity;
                    await _productService.UpdateProductAsync(product);
                }
            }
            await _orderRepository.CreateOrderAsync(order);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            var existingOrder = await GetOrderByIdAsync(order.Id);

            foreach (var existingItem in existingOrder.OrderItems)
            {
                var newItem = order.OrderItems.FirstOrDefault(i => i.ProductId == existingItem.ProductId);

                if (newItem != null)
                {
                    // Calculate the quantity difference
                    var quantityDifference = newItem.Quantity - existingItem.Quantity;

                    // Update product available quantity based on the difference
                    var product = await _productService.GetProductByIdAsync(existingItem.ProductId);
                    if (product != null)
                    {
                        product.AvailableQuantity -= quantityDifference;
                        await _productService.UpdateProductAsync(product);
                    }
                }
            }
            await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task DeleteOrderAsync(string orderId)
        {
            var order = await GetOrderByIdAsync(orderId);
            foreach (var item in order.OrderItems)
            {
                // Increase available quantity when the order is deleted
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.AvailableQuantity += item.Quantity;
                    await _productService.UpdateProductAsync(product);
                }
            }
            await _orderRepository.DeleteOrderAsync(orderId);
        }

        public async Task CancelOrderAsync(string orderId, string note)
        {
            var order = await GetOrderByIdAsync(orderId);

            if (order.OrderStatus == 0)
            {
                foreach (var item in order.OrderItems)
                {
                    var product = await _productService.GetProductByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.AvailableQuantity += item.Quantity;
                        await _productService.UpdateProductAsync(product);
                    }
                }

                order.OrderStatus = 3;
                order.Note = note;
                await _orderRepository.UpdateOrderAsync(order);
            }
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }
    }
}
