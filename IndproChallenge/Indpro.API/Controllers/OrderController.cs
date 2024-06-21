using Indpro.API.Data.Models;
using Indpro.API.DTO.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Indpro.API.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderDto _order;

        public OrderController(IOrderDto orderDto) => _order = orderDto;

        [HttpPost]
        [Route("orders")]
        public async Task<OperationResult> CreateOrder(OrderModel model) => await _order.CreateOrder(model);

        [HttpGet]
        [Route("orders")]
        public async Task<OperationResult<List<OrderModel>>> GetOrders(int userId) => await _order.GetOrders(userId);

        [HttpGet]
        [Route("orders/{id}")]
        public async Task<OperationResult<List<OrderItemModel>>> GetOrderDetail(int id) => await _order.GetOrderDetail(id);
    }

}
