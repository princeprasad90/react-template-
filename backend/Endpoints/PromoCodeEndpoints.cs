using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using backend.Models;
using System;
using System.Linq;

namespace backend.Endpoints;

public static class PromoCodeEndpoints
{
    public static void MapPromoCodeEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/promocodes", (string? promoCode = "", string? status = "", int page = 1, int pageSize = 10) =>
        {
            var allCodes = Enumerable.Range(1, 50).Select(i => new PromoCode
            {
                PromoCode = $"CODE{i:D3}",
                Variance = "Active",
                Description = $"Promo code {i}",
                GeneratedOn = DateTime.UtcNow.AddDays(-i),
                GeneratedBy = "System",
                ValidFrom = DateTime.UtcNow.AddDays(-i),
                ValidTo = DateTime.UtcNow.AddDays(i),
                Status = i % 2 == 0 ? "Active" : "Inactive"
            });

            if (!string.IsNullOrWhiteSpace(promoCode))
            {
                allCodes = allCodes.Where(c => c.PromoCode.Equals(promoCode, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                allCodes = allCodes.Where(c => c.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            var totalItems = allCodes.Count();
            var data = allCodes.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return Results.Ok(new
            {
                Data = data,
                Pagination = new
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    TotalItems = totalItems
                }
            });
        });
    }
}
