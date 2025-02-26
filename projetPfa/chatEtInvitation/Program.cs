

using chatEtInvitation.API.Extentions;
using chatEtInvitation.Infrastructure.ExternServices;
using chatEtInvitation.Core.Interfaces.IRepositories;
using chatEtInvitation.Core.Interfaces.IServices;
using chatEtInvitation.Core.Services;
using chatEtInvitation.Infrastructure.Data.Repositories;
using gestionEmployer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//builder interfaces services and repositories
builder.Services.AddScoped<IInvitationService, InvitationService>();
builder.Services.AddScoped<IMemberInvitationRepository, MemberInvitationRepository>();


//builder interfaces services and repositories
builder.Services.AddScoped<IInvitationService, InvitationService>();
builder.Services.AddScoped<IMemberInvitationRepository, MemberInvitationRepository>();


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
