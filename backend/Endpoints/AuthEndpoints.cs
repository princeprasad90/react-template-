using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Identity;

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
    }
}
