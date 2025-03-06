using Auth.Interfaces;
using Auth.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<UserService>();
builder.Services.AddHttpClient<EmployerService>();
builder.Services.AddHttpClient<AdminService>();


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmployerService, EmployerService>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddSingleton<IJwtService, JwtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
