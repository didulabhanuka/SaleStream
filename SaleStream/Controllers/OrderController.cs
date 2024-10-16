// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using SaleStream.Models;
// using SaleStream.Services;
// using System.Security.Claims;
// using System.Threading.Tasks;
// using System.Collections.Generic;
// using System.Linq;

// namespace SaleStream.Controllers
// {
//     [ApiController]
//     [Route("api/order")]
//     public class OrderController : ControllerBase
//     {
//         private readonly OrderService _orderService;
//         private readonly NotificationService _notificationService;

//         public OrderController(OrderService orderService, NotificationService notificationService)
//         {
//             _orderService = orderService;
//             _notificationService = notificationService;
//         }

//         // Create a new order
//         [Authorize]
//         [HttpPost("create")]
//         public async Task<IActionResult> CreateOrder([FromBody] OrderModel orderRequest)
//         {
//             var userId = User.FindFirst("UserId")?.Value;  // Get userId from token
//             var email = User.FindFirst(ClaimTypes.Email)?.Value;  // Extract Email from token

//             // Map OrderModel to the actual Order entity
//             var order = new Order
//             {
//                 UserId = userId,  // Set UserId from the token
//                 Email = email,           // Store Email in the order
//                 DeliveryAddress = orderRequest.DeliveryAddress,
//                 Note = orderRequest.Note,
//                 PaymentMethod = orderRequest.PaymentMethod,
//                 OrderItems = new List<OrderItem>(),
//                 OrderStatus = 0  // Default to Pending
//             };

//             // Calculate the total for the order and map items
//             foreach (var item in orderRequest.OrderItems)
//             {
//                 var orderItem = new OrderItem
//                 {
//                     ProductId = item.ProductId,
//                     ProductName = item.ProductName,
//                     Quantity = item.Quantity,
//                     UnitPrice = item.UnitPrice,
//                     TotalPrice = item.Quantity * item.UnitPrice,
//                     VendorId = item.VendorId,
//                     VendorEmail = item.VendorEmail,
//                     OrderItemStatus = 0  // Default to Pending
//                 };

//                 order.OrderItems.Add(orderItem);
//                 order.OrderTotal += orderItem.TotalPrice;
//             }

//             await _orderService.CreateOrderAsync(order);
//             return Ok("Order created successfully.");
//         }

//         // Update an order (before dispatched)
//         [Authorize]
//         [HttpPut("update/{orderId}")]
//         public async Task<IActionResult> UpdateOrder(string orderId, [FromBody] OrderModel orderUpdate)
//         {
//             var existingOrder = await _orderService.GetOrderByIdAsync(orderId);
//             if (existingOrder == null)
//             {
//                 return NotFound("Order not found.");
//             }

//             var userId = User.FindFirst("UserId")?.Value;
//             if (existingOrder.UserId != userId)
//             {
//                 return Unauthorized("You can only update your own orders.");
//             }

//             // Only allow update if the order is not yet dispatched
//             if (existingOrder.OrderStatus != 0)
//             {
//                 return BadRequest("Cannot update an order that is already dispatched.");
//             }

//             existingOrder.OrderItems = new List<OrderItem>();
//             existingOrder.Note = orderUpdate.Note;
//             existingOrder.DeliveryAddress = orderUpdate.DeliveryAddress;
//             existingOrder.PaymentMethod = orderUpdate.PaymentMethod;

//             // Recalculate the total and update items
//             existingOrder.OrderTotal = 0;
//             foreach (var item in orderUpdate.OrderItems)
//             {
//                 var orderItem = new OrderItem
//                 {
//                     ProductId = item.ProductId,
//                     ProductName = item.ProductName,
//                     Quantity = item.Quantity,
//                     UnitPrice = item.UnitPrice,
//                     TotalPrice = item.Quantity * item.UnitPrice,
//                     VendorId = item.VendorId,
//                     VendorEmail = item.VendorEmail,
//                     OrderItemStatus = 0  // Reset to Pending
//                 };

//                 existingOrder.OrderItems.Add(orderItem);
//                 existingOrder.OrderTotal += orderItem.TotalPrice;
//             }

//             await _orderService.UpdateOrderAsync(existingOrder);
//             return Ok("Order updated successfully.");
//         }

//         // Get all orders by a user
//         [Authorize]
//         [HttpGet("user-orders")]
//         public async Task<IActionResult> GetUserOrders()
//         {
//             var userId = User.FindFirst("UserId")?.Value;
//             var orders = await _orderService.GetOrdersByUserIdAsync(userId);
//             return Ok(orders);
//         }

//         // CSR/Admin cancel an order
//         [Authorize(Roles = "Customer Service Representative, Admin")]
//         [HttpPost("cancel-order/{orderId}")]
//         public async Task<IActionResult> CancelOrder(string orderId, [FromBody] CancelOrderRequest request)
//         {
//             try
//             {
//                 await _orderService.CancelOrderAsync(orderId, request.Note);  // Use the Note from the JSON object
//                 return Ok("Order canceled successfully.");
//             }
//             catch (InvalidOperationException ex)
//             {
//                 return BadRequest(ex.Message);  // Return 400 Bad Request with the error message
//             }
//         }

//         public class CancelOrderRequest
//         {
//             public string Note { get; set; }
//         }

//         // Get All Orders
//         [Authorize(Roles = "Customer Service Representative, Admin")]
//         [HttpGet("all-orders")]
//         public async Task<IActionResult> GetAllOrders()
//         {
//             var orders = await _orderService.GetAllOrdersAsync();
//             return Ok(orders);
//         }
//     }
// }


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleStream.Models;
using SaleStream.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace SaleStream.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly NotificationService _notificationService;

        public OrderController(OrderService orderService, NotificationService notificationService)
        {
            _orderService = orderService;
            _notificationService = notificationService;
        }

        // Create a new order
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel orderRequest)
        {
            var userId = User.FindFirst("UserId")?.Value;  // Get userId from token
            var email = User.FindFirst(ClaimTypes.Email)?.Value;  // Extract Email from token

            // Map OrderModel to the actual Order entity
            var order = new Order
            {
                UserId = userId,  // Set UserId from the token
                Email = email,  // Store Email in the order
                DeliveryAddress = orderRequest.DeliveryAddress,
                Note = orderRequest.Note,
                PaymentMethod = orderRequest.PaymentMethod,
                OrderItems = new List<OrderItem>(),
                OrderStatus = 0  // Default to Pending
            };

            // Calculate the total for the order and map items
            foreach (var item in orderRequest.OrderItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.Quantity * item.UnitPrice,
                    VendorId = item.VendorId,
                    VendorEmail = item.VendorEmail,
                    OrderItemStatus = 0  // Default to Pending
                };

                order.OrderItems.Add(orderItem);
                order.OrderTotal += orderItem.TotalPrice;
            }

            await _orderService.CreateOrderAsync(order);
            return Ok("Order created successfully.");
        }

        // Update an order (before dispatched)
        [Authorize]
        [HttpPut("update/{orderId}")]
        public async Task<IActionResult> UpdateOrder(string orderId, [FromBody] OrderModel orderUpdate)
        {
            var existingOrder = await _orderService.GetOrderByIdAsync(orderId);
            if (existingOrder == null)
            {
                return NotFound("Order not found.");
            }

            var userId = User.FindFirst("UserId")?.Value;
            if (existingOrder.UserId != userId)
            {
                return Unauthorized("You can only update your own orders.");
            }

            // Only allow update if the order is not yet dispatched
            if (existingOrder.OrderStatus != 0)
            {
                return BadRequest("Cannot update an order that is already dispatched.");
            }

            existingOrder.OrderItems.Clear();
            existingOrder.Note = orderUpdate.Note;
            existingOrder.DeliveryAddress = orderUpdate.DeliveryAddress;
            existingOrder.PaymentMethod = orderUpdate.PaymentMethod;

            // Recalculate the total and update items
            existingOrder.OrderTotal = 0;
            foreach (var item in orderUpdate.OrderItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.Quantity * item.UnitPrice,
                    VendorId = item.VendorId,
                    VendorEmail = item.VendorEmail,
                    OrderItemStatus = 0  // Reset to Pending
                };

                existingOrder.OrderItems.Add(orderItem);
                existingOrder.OrderTotal += orderItem.TotalPrice;
            }

            await _orderService.UpdateOrderAsync(existingOrder);
            return Ok("Order updated successfully.");
        }

        // Mark an order as delivered
        [Authorize(Roles = "Customer Service Representative, Admin")]
        [HttpPut("mark-delivered/{orderId}")]
        public async Task<IActionResult> MarkOrderAsDelivered(string orderId)
        {
            var existingOrder = await _orderService.GetOrderByIdAsync(orderId);
            if (existingOrder == null)
            {
                return NotFound("Order not found.");
            }

            if (existingOrder.OrderStatus == 3 && existingOrder.Delivered) // If already delivered
            {
                return BadRequest("Order is already marked as delivered.");
            }

            existingOrder.OrderStatus = 3;  // Set status to Delivered
            existingOrder.Delivered = true; // Set Delivered field to true

            await _orderService.UpdateOrderAsync(existingOrder);
            return Ok("Order marked as delivered successfully.");
        }

        // Get all orders by a user
        [Authorize]
        [HttpGet("user-orders")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        // CSR/Admin cancel an order
        [Authorize(Roles = "Customer Service Representative, Admin")]
        [HttpPost("cancel-order/{orderId}")]
        public async Task<IActionResult> CancelOrder(string orderId, [FromBody] CancelOrderRequest request)
        {
            try
            {
                await _orderService.CancelOrderAsync(orderId, request.Note);  // Use the Note from the JSON object
                return Ok("Order canceled successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);  // Return 400 Bad Request with the error message
            }
        }

        public class CancelOrderRequest
        {
            public string Note { get; set; }
        }

        // Get All Orders
        [Authorize(Roles = "Customer Service Representative, Admin")]
        [HttpGet("all-orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
    }
}
