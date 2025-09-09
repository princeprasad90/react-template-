using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Identity;
using backend.Clients;
using Microsoft.Extensions.Configuration;

namespace backend.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth");

        group.MapPost("/login", async (LoginRequest request, UserService userService) =>
        {
            var success = await userService.LoginAsync(request);
            return success ? Results.Ok() : Results.Unauthorized();
        }).AllowAnonymous();

        group.MapPost("/logout", async (UserService userService) =>
        {
            await userService.LogoutAsync();
            return Results.Ok();
        });

        group.MapPost("/change-password", async (ChangePasswordRequest request, UserManager<AppUser> userManager, UserService userService, ClaimsPrincipal user) =>
        {
            var appUser = await userManager.GetUserAsync(user);
            if (appUser is null) return Results.Unauthorized();
            var success = await userService.ChangePasswordAsync(appUser, request);
            return success ? Results.Ok() : Results.BadRequest();
        }).RequireAuthorization();

        group.MapGet("/external-login", async (string key, IAuthClient authClient, JwtService jwtService, IConfiguration config, CancellationToken ct) =>
        {
            var authResponse = await authClient.ExchangeKeyAsync(key, ct);
            if (authResponse is null) return Results.Unauthorized();

            var token = jwtService.GenerateToken(authResponse.Profile, authResponse.Menu);
            var redirect = config["Frontend:AuthRedirect"] ?? "/";
            return Results.Redirect($"{redirect}?token={token}");
        }).AllowAnonymous();
    }
}
