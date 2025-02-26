

using chatEtInvitation.API.Extentions;
using chatEtInvitation.Infrastructure.ExternServices;
using gestionEmployer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHttpClient<UserService>();
builder.Services.addRepositoriesDependencies();
builder.Services.addServicesDependencies();
var app = builder.Build();



app.UseAuthorization();

app.MapControllers();

app.Run();
