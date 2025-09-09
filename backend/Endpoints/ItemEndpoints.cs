using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;

namespace backend.Endpoints;

public static class ItemEndpoints
{
    public static void MapItemEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/items", (int page = 1, int pageSize = 25, string? search = "") =>
        {
            var allItems = Enumerable.Range(1, 100)
                .Select(i => new { id = i, name = $"Item {i}" });

            if (!string.IsNullOrWhiteSpace(search))
            {
                allItems = allItems.Where(i => i.name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var total = allItems.Count();
            var items = allItems
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Results.Ok(new { items, totalCount = total });
        });
    }
}
