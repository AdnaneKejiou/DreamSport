using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using ApiGateway.Middleware;
using System.Text;

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

var app = builder.Build();

app.UseCors();




app.UseMiddleware<TenantMiddleware>();
app.UseMiddleware<TenantMiddlewareHandler>();

app.UseAuthorization();

app.UseOcelot().Wait();

app.MapControllers();
app.Run();
