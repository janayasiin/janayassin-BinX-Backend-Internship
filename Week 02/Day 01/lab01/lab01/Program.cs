namespace lab01
{
  
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }

        public class User
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
        }

        public class Repository<T> where T : class
        {
            private readonly List<T> _items = new();

            public void Add(T item)
            {
                if (item == null) throw new ArgumentNullException(nameof(item));
                _items.Add(item);
            }

            public IReadOnlyList<T> GetAll() => _items.AsReadOnly();

            public IEnumerable<T> Find(Func<T, bool> predicate)
            {
                return _items.Where(predicate);
            }
        }

        
        class Program
        {
            static void Main(string[] args)
            {

                var productRepo = new Repository<Product>();
                productRepo.Add(new Product { Id = 1, Name = "Laptop", Price = 1200m });
                productRepo.Add(new Product { Id = 2, Name = "Mouse", Price = 25m });
                productRepo.Add(new Product { Id = 3, Name = "Keyboard", Price = 75m });

                Console.WriteLine("--- Products List (IReadOnlyList) ---");
                IReadOnlyList<Product> products = productRepo.GetAll();
                foreach (var p in products)
                {
                    Console.WriteLine($"[ID: {p.Id}] {p.Name} - ${p.Price}");
                }

                var userRepo = new Repository<User>();
                userRepo.Add(new User { Id = 101, Username = "Jana", Email = "jana@gmail.com" });
                userRepo.Add(new User { Id = 102, Username = "Admin", Email = "admin@gmail.com" });

                Console.WriteLine("\n--- Users List (IReadOnlyList) ---");
                IReadOnlyList<User> users = userRepo.GetAll();
                foreach (var u in users)
                {
                    Console.WriteLine($"[ID: {u.Id}] {u.Username} ({u.Email})");
                }

                Console.WriteLine("\n--- Find Test: Products with Price > $50 ---");
                var expensiveProducts = productRepo.Find(p => p.Price > 50);
                foreach (var p in expensiveProducts)
                {
                    Console.WriteLine($"Found: {p.Name} (${p.Price})");
                }
            }
        }
    }


