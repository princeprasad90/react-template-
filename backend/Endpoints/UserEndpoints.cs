using backend.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Linq;

namespace backend.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/users/me", (ClaimsPrincipal user) =>
        {
            var profile = new UserProfile
            {
                Id = user.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? string.Empty,
                Name = user.FindFirst("name")?.Value ?? string.Empty,
                Email = user.FindFirst("email")?.Value ?? string.Empty
            };

            var menuJson = user.FindFirst("menu")?.Value;
            var menu = string.IsNullOrEmpty(menuJson)
                ? Enumerable.Empty<MenuItem>()
                : JsonSerializer.Deserialize<IEnumerable<MenuItem>>(menuJson) ?? Enumerable.Empty<MenuItem>();

            return Results.Ok(new { profile, menu });
        }).RequireAuthorization();
    }
}
