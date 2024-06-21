using Indpro.API.Data.Entity.DbSet;
using Indpro.API.Data.Entity;
using Indpro.API.Data.Models;
using Indpro.API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Indpro.API.Repository.Service;

public class OrderService : IOrderService
{
    private readonly IndproChallengeDbContext _db;

    public OrderService(IndproChallengeDbContext dbContext) => _db = dbContext;

    public async Task<OperationResult> CreateOrder(OrderModel model)
    {
        try
        {
            var checkProductStock = CheckStock(model.OrderItems);
            if (checkProductStock.IsSuccess)
            {

                var orderItems = new List<OrderItem>();
                var products = new List<Product>();
                var totalPrice = 0.0M;

                var productIds = model.OrderItems.Select(oi => oi.ProductId).ToList();
                var productsList = _db.Products.Where(p => productIds.Contains(p.Id)).ToList();

                foreach (var orderItem in model.OrderItems)
                {
                    var product = productsList.Where(p => p.Id == orderItem.ProductId).First();
                    product.Stock -= orderItem.Quantity;
                    products.Add(product);

                    orderItems.Add(new OrderItem()
                    {
                        Price = product.Price,
                        ProductId = orderItem.ProductId,
                        Quantity = orderItem.Quantity,
                    });

                    totalPrice += orderItem.Quantity * product.Price;
                }
                var order = new Order()
                {
                    UserId = model.UserId,
                    TotalPrice = totalPrice,
                    Status = model.Status,
                    CreatedAt = DateTime.UtcNow
                };
                await _db.Orders.AddAsync(order);
                await _db.SaveChangesAsync();

                orderItems.ForEach(x => x.OrderId = order.Id);

                _db.Products.UpdateRange(products);
                await _db.OrderItems.AddRangeAsync(orderItems);
                await _db.SaveChangesAsync();
                return new OperationResult(true, "Order created successfully.", StatusCodes.Status201Created);
            }
            else
            {
                return checkProductStock;
            }
        }
        catch (Exception ex)
        {
            return new OperationResult(false, "Something went wrong. Please try again after some time.", StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<OperationResult<List<OrderModel>>> GetOrders(int userId)
    {
        var result = new OperationResult<List<OrderModel>>();
        try
        {
            var orders = _db.Orders.Where(x => x.UserId == userId).ToList();

            result.IsSuccess = true;
            result.StatusCode = StatusCodes.Status200OK;
            result.Data = new List<OrderModel>();
            result.Message = "Orders fetched successfully.";
            foreach (var order in orders)
            {

                result.Data.Add(new OrderModel()
                {
                    Id = order.Id,
                    TotalPrice = order.TotalPrice,
                    UserId = order.UserId,
                    Status = order.Status,
                    CreatedAt = order.CreatedAt
                });
            }
        }
        catch (Exception ex)
        {
            result.IsSuccess = true;
            result.StatusCode = StatusCodes.Status500InternalServerError;
            result.Message = "Something went wrong. Please try again after some time.";
        }
        return result;
    }

    public async Task<OperationResult<List<OrderItemModel>>> GetOrderDetail(int id)
    {
        var result = new OperationResult<List<OrderItemModel>>();
        try
        {
            var orders = _db.OrderItems.Where(x => x.OrderId == id).ToList();
            var ids = orders.Select(x => x.ProductId).ToList();
            var products = _db.Products.Where(x => ids.Contains(x.Id)).ToList();
            result.IsSuccess = true;
            result.StatusCode = StatusCodes.Status200OK;
            result.Message = "Orders fetched Successfully.";
            result.Data = new List<OrderItemModel>();
            foreach (var order in orders)
            {
                var product = products.Where(x => x.Id == order.ProductId).FirstOrDefault();
                var orderItem = new OrderItemModel()
                {
                    Id = order.Id,
                    Price = order.Price,
                    ProductId = order.ProductId,
                    Quantity = order.Quantity,
                    OrderId = order.OrderId,
                    Product = new ProductModel()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Name,
                        Price = product.Price,
                        Stock = product.Stock
                    }
                };
                result.Data.Add(orderItem);
            }
        }
        catch (Exception)
        {
            result.IsSuccess = true;
            result.StatusCode = StatusCodes.Status500InternalServerError;
            result.Message = "Something went wrong. Please try again after some time.";
        }
        return result;
    }

    private OperationResult CheckStock(List<OrderItemModel> model)
    {
        var isOutofStock = false;
        var message = string.Empty;
        foreach (var order in model)
        {
            var product = _db.Products.Where(x => x.Id == order.ProductId).FirstOrDefault();
            var stock = product.Stock - order.Quantity;
            if (stock < 0)
            {
                isOutofStock = true;
                if (product.Stock is 0)
                {
                    message += product.Name + " is out of stock.";
                }
                else
                {
                    message += " Only " + product.Stock + " quantity of " + product.Name + " is available and ordered quantity is " + order.Quantity + ".";
                }

            }
        }

        return new OperationResult(!isOutofStock, message, !isOutofStock ? StatusCodes.Status406NotAcceptable : StatusCodes.Status200OK);
    }
}
