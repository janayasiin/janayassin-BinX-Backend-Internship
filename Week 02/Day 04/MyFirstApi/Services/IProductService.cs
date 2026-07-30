using MyFirstApi.Models;

namespace MyFirstApi.Services
{
    public interface IProductService
    {
        List<Product> GetProducts();
        Product? GetProductById(int Id);

    }
}
