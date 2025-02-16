using gestionEquipe.Infrastructure.Data;
using gestionEquipe.Infrastructure.ExternServices;
using Microsoft.EntityFrameworkCore;
using gestionEquipe.Core.Interfaces;
using gestionEquipe.Infrastructure.Data.Repositories;
using gestionEquipe.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IMembersRepository, MembersRepository>();
builder.Services.AddScoped<IMembersService, MembersService>();
builder.Services.AddHttpClient<SiteService>();
builder.Services.AddScoped<IEquipeRepository, EquipeRepository>();
builder.Services.AddScoped<IEquipeService, EquipeService>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
