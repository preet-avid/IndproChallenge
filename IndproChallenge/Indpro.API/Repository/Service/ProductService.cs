using Indpro.API.Data.Entity;
using Indpro.API.Data.Entity.DbSet;
using Indpro.API.Data.Models;
using Indpro.API.Repository.Interface;

namespace Indpro.API.Repository.Service;

public class ProductService : IProductService
{
    private readonly IndproChallengeDbContext _db;

    public ProductService(IndproChallengeDbContext dbContext) => _db = dbContext;

    public async Task<OperationResult<List<ProductModel>>> GetProducts()
    {
        var result = new OperationResult<List<ProductModel>>();
        try
        {
            var products = _db.Products.Where(x=> x.IsDeleted == false).ToList();

            result.IsSuccess = true;
            result.StatusCode = StatusCodes.Status200OK;
            result.Message = "Products fetched successfully.";
            result.Data = new List<ProductModel>();
            foreach (var product in products)
            {

                result.Data.Add(new ProductModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Stock = product.Stock,
                    Description = product.Description,
                    Price = product.Price,
                    CreatedAt = product.CreatedAt
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

    public async Task<OperationResult> CreateProduct(ProductModel model)
    {
        try
        {
            var user = _db.Users.Where(x => x.Id == model.UserId).FirstOrDefault();
            if (user is not null)
            {
                if (user.IsAdmin)
                {
                    var product = new Product()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Price = model.Price,
                        Stock = model.Stock,
                        CreatedAt = model.CreatedAt
                    };
                    await _db.Products.AddAsync(product);
                    await _db.SaveChangesAsync();

                    return new OperationResult(true, "Product created successfully.", StatusCodes.Status201Created);
                }
                else
                {
                    return new OperationResult(false, "You don't have rights to create product.", StatusCodes.Status406NotAcceptable);
                }
            }
            else
            {
                return new OperationResult(false, "Invalid user details.", StatusCodes.Status404NotFound);
            }
        }
        catch (Exception ex)
        {
            return new OperationResult(false, "Something went wrong. Please try again after some time.", StatusCodes.Status500InternalServerError);
        }
    }
    public async Task<OperationResult> UpdateProduct(ProductModel model, int id)
    {
        try
        {
            var user = _db.Users.Where(x => x.Id == model.UserId).FirstOrDefault();
            if (user is not null)
            {
                if (user.IsAdmin)
                {
                    var product = _db.Products.Where(x => x.Id == id).FirstOrDefault();
                    if (product is not null)
                    {
                        product.Name = model.Name;
                        product.Description = model.Description;
                        product.Price = model.Price;
                        product.Stock = model.Stock;

                        _db.Update(product);
                        _db.SaveChanges();

                        return new OperationResult(true, "Product updated successfully.", StatusCodes.Status200OK);
                    }
                    else
                    {
                        return new OperationResult(true, "Product not found.", StatusCodes.Status404NotFound);
                    }
                }
                else
                {
                    return new OperationResult(false, "You don't have rights to update product.", StatusCodes.Status406NotAcceptable);
                }
            }
            else
            {
                return new OperationResult(false, "Only admin can update products.", StatusCodes.Status404NotFound);
            }


        }
        catch (Exception ex)
        {
            return new OperationResult(false, "Something went wrong. Please try again after some time.", StatusCodes.Status500InternalServerError);
        }
    }

    public async Task<OperationResult> DeleteProduct(int id, int userId)
    {
        try
        {
            var user = _db.Users.Where(x => x.Id == userId).FirstOrDefault();
            if (user is not null)
            {
                if (user.IsAdmin)
                {
                    var product = _db.Products.Where(x => x.Id == id).FirstOrDefault();

                    if (product is not null)
                    {
                        product.IsDeleted = true;
                        _db.Products.Update(product);
                        _db.SaveChanges();

                        return new OperationResult(true, "Product deleted successfully.", StatusCodes.Status200OK);
                    }
                    else
                    {
                        return new OperationResult(true, "Product not found.", StatusCodes.Status404NotFound);
                    }
                }
                else
                {
                    return new OperationResult(false, "You don't have rights to update product.", StatusCodes.Status406NotAcceptable);
                }
            }
            else
            {
                return new OperationResult(false, "Invalid user details.", StatusCodes.Status404NotFound);
            }
        }
        catch (Exception ex)
        {
            return new OperationResult(false, "Something went wrong. Please try again after some time.", StatusCodes.Status500InternalServerError);
        }
    }

}
