using backend.Clients;
using backend.Data;
using backend.Endpoints;
using backend.Models;
using backend.Observability;
using backend.Policies;
using backend.Services;
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

app.MapAuthEndpoints();
app.MapItemEndpoints();
app.MapPromoCodeEndpoints();
app.MapProductEndpoints();


app.Run();
