# PagingToolkit

Lightweight, async-friendly paging utility for .NET (targeting .NET9).

`PagingToolkit` provides a small set of helpers to standardize paging (slicing) of `IQueryable<T>` sources and to return a page of results together with paging metadata.

Package

- PackageId: `PagingToolkit`
- Version: `1.0.0`
- Targets: `.NET9` (net9.0)
- Dependency: `Microsoft.EntityFrameworkCore` (used for async LINQ extensions)

Features

- `PagingParameters` — a simple DTO to carry paging and optional sorting parameters.
- `PagingToolkit<T>` — a generic page container that holds the items for a single page and metadata such as current page, total pages, page size and total count.
- Synchronous and asynchronous factory methods that operate on `IQueryable<T>`.

Install

- Using the .NET CLI:

`dotnet add package PagingToolkit --version 1.0.0`

- Or add a `PackageReference` to your project file:

`<PackageReference Include="PagingToolkit" Version="1.0.0" />`

Quick overview

- `PagingParameters`
 - `PageNumber` (default `1`) — this library's parameter is1-based for convenience when used in APIs.
 - `PageSize` (default `10`, maximum `100`) — values greater than the maximum are capped to `100`.
 - `SortBy` / `SortDesc` — optional sorting hints (no automatic sorting applied by the library).

- `PagingToolkit<T>`
 - Properties: `CurrentPage`, `TotalPages`, `PageSize`, `TotalCount`.
 - Inherits `List<T>` so it enumerates the page items.
 - Factory methods:
 - `Create(IQueryable<T> source, int pageNumber, int pageSize)` — synchronous.
 - `CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken ct)` — asynchronous (uses EF Core async extensions).

Important note about page indexing

The `PagingToolkit.Create` / `CreateAsync` methods expect a zero-based page index (0 = first page). If you are using `PagingParameters.PageNumber` (which is1-based), pass `pagingParams.PageNumber -1` to the `Create` / `CreateAsync` methods.

Example (Entity Framework Core)

```csharp
//1. Prepare paging parameters (1-based page number)
var pagingParams = new PagingParameters { PageNumber =1, PageSize =20, SortBy = "Name" };

//2. Build your query (apply filters and ordering BEFORE paging)
var query = context.Products.AsNoTracking().OrderBy(p => p.Name);

//3. Use the async paging helper (convert1-based to0-based index)
var page = await PagingToolkit<Product>.CreateAsync(query, pagingParams.PageNumber -1, pagingParams.PageSize, CancellationToken.None);

Console.WriteLine($"Showing page {page.CurrentPage} of {page.TotalPages} ({page.TotalCount} total items)");
foreach (var item in page)
{
 Console.WriteLine(item.Name);
}

// Synchronous equivalent
var syncPage = PagingToolkit<Product>.Create(query, pagingParams.PageNumber -1, pagingParams.PageSize);
```

Best practices

- Apply filtering and ordering to the `IQueryable<T>` before calling the paging helpers so the results are deterministic.
- Use `AsNoTracking()` for read-only queries when using EF Core to improve performance.
- Pass a `CancellationToken` to the async method to support cancellation.

Contributing

Contributions and issues are welcome. Please open issues or pull requests on the repository.

License

This project follows the license in the repository. Check the `LICENSE` file for details.