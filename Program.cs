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
    try
    {
        Console.WriteLine($"DATABASE_URL = {databaseUrl}"); // Mostrar la URL leída

        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');
        var port = uri.Port != -1 ? uri.Port : 5432; // Usar puerto 5432 si no se especifica

        var connectionString = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = port,
            Database = uri.AbsolutePath.Trim('/'),
            Username = userInfo[0],
            Password = userInfo[1],
            SslMode = SslMode.Prefer,
            TrustServerCertificate = true
        }.ToString();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al procesar DATABASE_URL: {ex.Message}");
        throw;
    }
}
else
{
    Console.WriteLine("DATABASE_URL no está configurada. Usando la configuración predeterminada de appsettings.json.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
