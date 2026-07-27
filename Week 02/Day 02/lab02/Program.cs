using System;
namespace lab02
{

       public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Order
        {
            public int Id { get; set; }
            public int CustomerId { get; set; }
            public decimal Amount { get; set; }
            public List<string> Items { get; set; } 
        }

        class Program
        {
            static void Main(string[] args)
            {

                var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Jana" },
                new Customer { Id = 2, Name = "Noor" },
                new Customer { Id = 3, Name = "Sara" },
                new Customer { Id = 4, Name = "Omar" },
                new Customer { Id = 5, Name = "Mohammed" },
                new Customer { Id = 6, Name = "Lana" }
            };

                var orders = new List<Order>
            {
                new Order { Id = 101, CustomerId = 1, Amount = 150m, Items = new() { "Laptop", "Mouse" } },
                new Order { Id = 102, CustomerId = 1, Amount = 80m, Items = new() { "Keyboard" } },
                new Order { Id = 103, CustomerId = 2, Amount = 300m, Items = new() { "Monitor", "HDMI Cable" } },
                new Order { Id = 104, CustomerId = 3, Amount = 45m, Items = new() { "Headset" } },
                new Order { Id = 105, CustomerId = 2, Amount = 120m, Items = new() { "USB Hub", "Webcam" } },
                new Order { Id = 106, CustomerId = 4, Amount = 500m, Items = new() { "Chair", "Desk Mat" } }
            };

          
                Console.WriteLine("--- 1. GroupBy: Total amount per customer ---");
                var ordersByCustomer = orders
                    .GroupBy(o => o.CustomerId)
                    .Select(g => new { CustomerId = g.Key, TotalAmount = g.Sum(o => o.Amount) });

                foreach (var group in ordersByCustomer)
                {
                    Console.WriteLine($"Customer ID: {group.CustomerId} | Total Spent: ${group.TotalAmount}");
                }

               
                Console.WriteLine("\n--- 2. Join: Combining Customer Names with Order Amounts ---");
                var customerOrders = customers
                    .Join(orders,
                          c => c.Id,
                          o => o.CustomerId,
                          (c, o) => new { c.Name, o.Amount });

                foreach (var co in customerOrders)
                {
                    Console.WriteLine($"Customer: {co.Name} | Order Amount: ${co.Amount}");
                }

             
                Console.WriteLine("\n--- 3. SelectMany: Flattening all order items into a single sequence ---");
                var allItems = orders.SelectMany(o => o.Items);

                foreach (var item in allItems)
                {
                    Console.WriteLine($"- Item: {item}");
                }

                
                Console.WriteLine("\n--- 4. Demonstrating Deferred Execution ---");

                var expensiveOrders = orders.Where(o => o.Amount > 100);
                Console.WriteLine("Query defined. Now adding a new expensive order to the source list...");

                orders.Add(new Order { Id = 107, CustomerId = 5, Amount = 950m, Items = new() { "Phone" } });

                Console.WriteLine("Executing query now:");
                foreach (var order in expensiveOrders)
                {
                    Console.WriteLine($"Order ID: {order.Id} | Amount: ${order.Amount} (Detected dynamically!)");
                }
            }
        }
    }

