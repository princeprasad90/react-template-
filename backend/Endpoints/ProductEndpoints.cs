using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using backend.Clients;
using Microsoft.AspNetCore.Http;

namespace backend.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/products", async (IProductsClient client, HttpContext ctx) =>
        {
            var data = await client.GetProductsAsync(ctx.RequestAborted);
            return Results.Ok(data);
        }).RequireAuthorization();
    }
}
