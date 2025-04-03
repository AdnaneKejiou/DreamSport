using gestionEmployer.Core.Interfaces;
using gestionEmployer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using gestionEmployer.Infrastructure.Data.Repositories;
using gestionEmployer.Core.Services;
using gestionEmployer.Infrastructure.ExternServices;
using Shared.Messaging.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder interfaces services and repositories
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IPasswordServiceAdmin, PasswordServiceAdmin>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ISiteService, SiteService>();



var app = builder.Build();




app.UseAuthorization();

app.MapControllers();

app.Run();
