using backend.Clients;
using backend.Data;
using backend.Models;
using backend.Observability;
using backend.Policies;
using backend.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("AuthDb"));

builder.Services.AddDbContext<TraceDbContext>(options =>
    options.UseInMemoryDatabase("TraceDb"));

builder.Services.AddIdentityCore<AppUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireUppercase = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<UserService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = builder.Configuration["Auth:Authority"];
    options.ClientId = builder.Configuration["Auth:ClientId"];
    options.ClientSecret = builder.Configuration["Auth:ClientSecret"];
    options.ResponseType = "code";
    options.SaveTokens = true;
});

builder.Services.AddHttpClient<IProductsClient, ProductsClient>(client =>
{
    var baseUrl = builder.Configuration["Services:Products"] ?? "https://example.com";
    client.BaseAddress = new Uri(baseUrl);
})
.AddPolicyHandler(ResiliencePolicies.Retry)
.AddPolicyHandler(ResiliencePolicies.Timeout)
.AddPolicyHandler(ResiliencePolicies.CircuitBreaker);

builder.Services.AddOpenTelemetry()
    .WithTracing(t =>
    {
        t.AddAspNetCoreInstrumentation();
        t.AddHttpClientInstrumentation();
        t.AddProcessor(sp =>
            new SimpleActivityExportProcessor(
                new SqlTraceExporter(sp.GetRequiredService<TraceDbContext>())));
    })
    .WithMetrics(m => m.AddAspNetCoreInstrumentation())
    .WithLogging();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/auth/login", async (LoginRequest request, UserService userService) =>
{
    var success = await userService.LoginAsync(request);
    return success ? Results.Ok() : Results.Unauthorized();
}).AllowAnonymous();

app.MapPost("/api/auth/logout", async (UserService userService) =>
{
    await userService.LogoutAsync();
    return Results.Ok();
});

app.MapPost("/api/auth/change-password", async (ChangePasswordRequest request, UserManager<AppUser> userManager, UserService userService, ClaimsPrincipal user) =>
{
    var appUser = await userManager.GetUserAsync(user);
    if (appUser is null) return Results.Unauthorized();
    var success = await userService.ChangePasswordAsync(appUser, request);
    return success ? Results.Ok() : Results.BadRequest();
}).RequireAuthorization();

app.MapGet("/api/items", (int page = 1, int pageSize = 25, string? search = "") =>
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

app.MapGet("/api/promocodes", (string? promoCode = "", string? status = "", int page = 1, int pageSize = 10) =>
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

app.MapGet("/products", async (IProductsClient client, HttpContext ctx) =>
{
    var data = await client.GetProductsAsync(ctx.RequestAborted);
    return Results.Ok(data);
}).RequireAuthorization();

app.Run();
