using Microsoft.EntityFrameworkCore;
using SistemaAdopcionMascotas.Models;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Leer la variable DATABASE_URL y convertirla en una cadena de conexión
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(databaseUrl))
{
    var connectionStringBuilder = new NpgsqlConnectionStringBuilder(databaseUrl)
    {
        SslMode = SslMode.Prefer,
        TrustServerCertificate = true
    };
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionStringBuilder.ToString()));
}
else
{
    // Usar la configuración predeterminada de appsettings.json si DATABASE_URL no está configurada
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
