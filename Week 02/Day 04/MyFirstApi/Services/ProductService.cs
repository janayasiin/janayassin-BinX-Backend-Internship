using DocumentFormat.OpenXml.Office2010.Excel;
using MyFirstApi.Models;

namespace MyFirstApi.Services
{
    public class ProductService : IProductService
    {
        private static List<Product> products = new List<Product>
{
    new Product
    {
        Id = 1,
        ProductName = "Laptop",
        Price = 2000
    },
    new Product
    {
        Id = 2,
        ProductName = "Keyboard",
        Price = 120
    },
    new Product
    {
        Id = 3,
        ProductName = "Mouse",
        Price = 50
    }
};
        public Product? GetProductById(int Id)
        {
            return products.FirstOrDefault(p => p.Id == Id);
        }

        public List<Product> GetProducts()
        {
            return products;        }
    }
}
