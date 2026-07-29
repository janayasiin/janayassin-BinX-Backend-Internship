using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Models;

namespace MyFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
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

        [HttpGet]
        public ActionResult<List<Product>> GetProducts() {
            return Ok(products);
        }
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id) {
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }



    }
}
