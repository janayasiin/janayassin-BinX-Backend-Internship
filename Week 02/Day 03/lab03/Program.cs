using System.Diagnostics;

namespace Lab03
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var sw = new Stopwatch();

            Console.WriteLine("===== Sequential Execution =====");

            sw.Start();

            await GetUsersAsync();
            await GetOrdersAsync();
            await GetProductsAsync();

            sw.Stop();
            Console.WriteLine($"\nSequential Time: {sw.ElapsedMilliseconds} ms");

       
            Console.WriteLine("\n===== Concurrent Execution =====");

            sw.Restart();

            var usersTask = GetUsersAsync();
            var ordersTask = GetOrdersAsync();
            var productsTask = GetProductsAsync();

            await Task.WhenAll(usersTask, ordersTask, productsTask);

            sw.Stop();
            Console.WriteLine($"\nConcurrent Time: {sw.ElapsedMilliseconds} ms");

         
            Console.WriteLine("\n===== Cancellation Demo =====");

            var cts = new CancellationTokenSource();

          
            cts.CancelAfter(2000);

            try
            {
                await LongRunningOperationAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation was cancelled!");
            }

            Console.WriteLine("\nProgram Finished.");
        }

        static async Task GetUsersAsync()
        {
            Console.WriteLine("Loading Users...");
            await Task.Delay(1000);
            Console.WriteLine("Users Loaded.");
        }

        
        static async Task GetOrdersAsync()
        {
            Console.WriteLine("Loading Orders...");
            await Task.Delay(1500);
            Console.WriteLine("Orders Loaded.");
        }

     
        static async Task GetProductsAsync()
        {
            Console.WriteLine("Loading Products...");
            await Task.Delay(2000);
            Console.WriteLine("Products Loaded.");
        }

        static async Task LongRunningOperationAsync(CancellationToken token)
        {
            Console.WriteLine("Starting long-running operation...");

            for (int i = 1; i <= 5; i++)
            {
                token.ThrowIfCancellationRequested();

                Console.WriteLine($"Processing step {i}...");
                await Task.Delay(1000, token);
            }

            Console.WriteLine("Long-running operation completed.");
        }
    }
}