# Product Sorting API

This project provides an ASP.NET Core Web API that serves product data using pagination and sorting,
based on pre-sorted text files. It also includes a hosted service that generates these sorted files on application startup.

## Features

- API endpoint: `/api/products` with support for:
  - Pagination: `pageNumber`, `pageSize`
  - Sorting: `sortBy` (options: `id`, `name`, `price`)
- Background service that:
  - Reads `products.txt`
  - Sorts products by ID, Name, and Price
  - Outputs to:
    - `products_sorted_by_id.txt`
    - `products_sorted_by_name.txt`
    - `products_sorted_by_price.txt`


## How to Use

1. Run the project. The hosted service will automatically:
   - Create the `Data` directory (if not exists)
   - Generate the sorted files

3. Call the API:
   ```
   GET /api/products?pageNumber=1&pageSize=10&sortBy=name
   ```
