using Indpro.API.Data.Models;
using Indpro.API.DTO.Interface;
using Indpro.API.Repository.Interface;

namespace Indpro.API.DTO.Service;
public class ProductDto : IProductDto
{
    private readonly IProductService _product;
    public ProductDto(IProductService productService)
    {
        _product = productService;
    }
    public Task<OperationResult> CreateProduct(ProductModel model) => _product.CreateProduct(model);

    public Task<OperationResult> DeleteProduct(int id, int userId) => _product.DeleteProduct(id, userId);

    public Task<OperationResult<List<ProductModel>>> GetProducts() => _product.GetProducts();

    public Task<OperationResult> UpdateProduct(ProductModel model, int id) => _product.UpdateProduct(model, id);
}