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
builder.Services.AddCors(Options =>
{
    Options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
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
