using ProductApiProject.Models;

namespace week11day3.Services
{
    public class ProductSorterHostedService: IHostedService
    {
        private readonly IWebHostEnvironment _env;

        public ProductSorterHostedService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            string sourceFile = Path.Combine(_env.ContentRootPath, "Data", "products.txt");
            string outputDir = Path.Combine(_env.ContentRootPath, "Data");

            if (!File.Exists(sourceFile))
            {
                Console.WriteLine("Source file not found.");
                return Task.CompletedTask;
            }

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            int batchSize = 1000;
            var batch = new List<Product>();

            using var writerById = new StreamWriter(Path.Combine(outputDir, "products_sorted_by_id.txt"));
            using var writerByName = new StreamWriter(Path.Combine(outputDir, "products_sorted_by_name.txt"));
            using var writerByPrice = new StreamWriter(Path.Combine(outputDir, "products_sorted_by_price.txt"));

            foreach (var line in File.ReadLines(sourceFile))
            {
                var parts = line.Split(',');
                if (parts.Length != 3) continue;

                if (int.TryParse(parts[0], out int id) &&
                    decimal.TryParse(parts[2], out decimal price))
                {
                    batch.Add(new Product
                    {
                        Id = id,
                        Name = parts[1],
                        Price = price
                    });

                    // When batch is full, sort and write
                    if (batch.Count == batchSize)
                    {
                        WriteSortedBatch(batch, writerById, writerByName, writerByPrice);
                        batch.Clear();
                    }
                }
            }

            // Write any remaining products
            if (batch.Any())
            {
                WriteSortedBatch(batch, writerById, writerByName, writerByPrice);
            }

            Console.WriteLine("Product sorting completed successfully.");
            return Task.CompletedTask;
        }

        private void WriteSortedBatch(List<Product> batch, StreamWriter byId, StreamWriter byName, StreamWriter byPrice)
        {
            foreach (var p in batch.OrderBy(p => p.Id))
                byId.WriteLine($"{p.Id},{p.Name},{p.Price}");

            foreach (var p in batch.OrderBy(p => p.Name))
                byName.WriteLine($"{p.Id},{p.Name},{p.Price}");

            foreach (var p in batch.OrderBy(p => p.Price))
                byPrice.WriteLine($"{p.Id},{p.Name},{p.Price}");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}