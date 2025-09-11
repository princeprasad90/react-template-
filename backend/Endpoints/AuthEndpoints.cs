using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Security.Claims;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Identity;
using backend.Clients;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;

namespace backend.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/auth");

        group.MapPost("/login", async (LoginRequest request, UserService userService, JwtService jwtService) =>
        {
            var success = await userService.LoginAsync(request);
            if (!success) return Results.Unauthorized();
            var profile = new UserProfile { Id = Guid.NewGuid().ToString(), Name = request.Username, Email = string.Empty };
            var tokens = jwtService.GenerateTokenPair(profile, Enumerable.Empty<MenuItem>());
            return Results.Ok(tokens);
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

            var tokens = jwtService.GenerateTokenPair(authResponse.Profile, authResponse.Menu);
            var redirect = config["Frontend:AuthRedirect"] ?? "/";
            return Results.Redirect($"{redirect}?token={tokens.AccessToken}&refreshToken={tokens.RefreshToken}");
        }).AllowAnonymous();

        group.MapPost("/refresh", (RefreshRequest request, JwtService jwtService) =>
        {
            var principal = jwtService.ValidateRefreshToken(request.RefreshToken);
            if (principal is null) return Results.Unauthorized();
            var profile = new UserProfile
            {
                Id = principal.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? string.Empty,
                Name = principal.FindFirst("name")?.Value ?? string.Empty,
                Email = principal.FindFirst("email")?.Value ?? string.Empty
            };
            var menuJson = principal.FindFirst("menu")?.Value ?? "[]";
            var menu = JsonSerializer.Deserialize<IEnumerable<MenuItem>>(menuJson) ?? Enumerable.Empty<MenuItem>();
            var tokens = jwtService.GenerateTokenPair(profile, menu);
            return Results.Ok(tokens);
        }).AllowAnonymous();
    }
}
