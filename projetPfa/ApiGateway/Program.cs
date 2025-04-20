using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using ApiGateway.Middleware;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Values;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddOcelot();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:4200")  // Allow your Angular app's origin
               .AllowAnyHeader()                   // Allow any headers
               .AllowAnyMethod()                   // Allow any HTTP methods
               .AllowCredentials();                // Allow credentials (cookies, Authorization headers)
    });
});
builder.Services.AddHttpClient<TenantMiddleware>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddHttpClient("AdminService", client =>
{
    client.BaseAddress = new Uri("http://gestionemployer");
});
var app = builder.Build();

app.UseCors();




app.UseMiddleware<TenantMiddleware>();
app.UseMiddleware<TenantMiddlewareHandler>();
app.UseMiddleware<TenantComparingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseOcelot().Wait();

app.MapControllers();
app.UseRouting(); 
app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics(); // Prometheus /metrics endpoint
});

app.Run();