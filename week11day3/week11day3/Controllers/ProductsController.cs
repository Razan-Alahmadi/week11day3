using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApiProject.Models;

namespace week11day3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet("GetProducts")]
        public IActionResult GetProducts(int pageNumber = 1, int pageSize = 10, string sortBy = "id")
        {
            // Validate input
            if (pageNumber < 1 || pageSize < 1)
                return BadRequest("Page number and size must be greater than zero.");

            // Determine the sorted file name based on sortBy
            string fileName = sortBy.ToLower() switch
            {
                "name" => "products_sorted_by_name.txt",
                "price" => "products_sorted_by_price.txt",
                _ => "products_sorted_by_id.txt"
            };

            // Build the file path
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Sorted file not found.");

            // Calculate how many items to skip
            int skip = (pageNumber - 1) * pageSize;

            var products = new List<Product>();

            // Read and parse the file line-by-line
            foreach (var line in System.IO.File.ReadLines(filePath).Skip(skip).Take(pageSize))
            {
                var parts = line.Split(',');
                if (parts.Length != 3) continue;

                if (int.TryParse(parts[0], out int id) &&
                    decimal.TryParse(parts[2], out decimal price))
                {
                    products.Add(new Product
                    {
                        Id = id,
                        Name = parts[1],
                        Price = price
                    });
                }
            }

            return Ok(products);
        }
    }
}