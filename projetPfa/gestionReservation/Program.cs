using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Ajouter la configuration du DbContext (accès à la base de données)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ajouter les services nécessaires à l'injection de dépendances
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();  // Repository pour Reservation
builder.Services.AddScoped<IReservationService, ReservationService>();  // Service pour Reservation
builder.Services.AddScoped<IStatusService, StatusService>();  // Service pour Status (si besoin)

// Si vous utilisez des mappers ou des services supplémentaires, vous pouvez les ajouter ici aussi
// builder.Services.AddScoped<IReservationMapper, ReservationMapper>();

// Ajouter les contrôleurs pour exposer les API
builder.Services.AddControllers();

//http client
builder.Services.AddHttpClient();

// Ajouter Swagger pour la documentation de l'API (si besoin)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Utiliser Swagger si activé
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configurer le pipeline de requêtes HTTP
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();  // Mapper les contrôleurs

// Démarrer l'application
app.Run();
