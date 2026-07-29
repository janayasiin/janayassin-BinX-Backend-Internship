
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Models;
using System.Reflection;
namespace MyFirstApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
          
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            var products = new List<Product>
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

            app.MapGet("/products", () => products);

            app.MapGet("/products/{id}", (int id) =>
            {
                var product = products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                    return Results.NotFound();

                return Results.Ok(product);
            });


            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();
            try
            {
                app.MapControllers();
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (var error in ex.LoaderExceptions)
                {
                    Console.WriteLine(error.Message);
                }

                throw;
            }
            app.Run();
        } } }