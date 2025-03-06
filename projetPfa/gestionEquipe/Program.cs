using gestionEquipe.Infrastructure.Data;
using gestionEquipe.Infrastructure.ExternServices;
using Microsoft.EntityFrameworkCore;
using gestionEquipe.Core.Interfaces;
using gestionEquipe.Infrastructure.Data.Repositories;
using gestionEquipe.Core.Services;
using gestionEquipe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IMembersRepository, MembersRepository>();
builder.Services.AddScoped<IMembersService, MembersService>();
builder.Services.AddHttpClient<SiteService>();
builder.Services.AddScoped<IEquipeRepository, EquipeRepository>();
builder.Services.AddScoped<IEquipeService, EquipeService>();
builder.Services.AddScoped<ISiteService, SiteService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllersWithViews();

var app = builder.Build();




app.UseRouting();


app.UseAuthorization();

app.MapControllers();

app.Run();
