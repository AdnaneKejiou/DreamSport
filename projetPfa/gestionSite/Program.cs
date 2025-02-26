
using gestionSite.Core.Interfaces.SportInterfaces;
using gestionSite.Core.Services;
using gestionSite.Infrastructure.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddScoped<ISportRepository, SportRepository>();
builder.Services.AddScoped<ISportService, SportService>();

builder.Services.AddSiteDependencies();
builder.Services.addAnnoncesDependencies ();
builder.Services.AddFaqDependencies();
builder.Services.AddTerrainDependencies();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();




app.UseAuthorization();

app.MapControllers();

app.Run();
