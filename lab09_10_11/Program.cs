using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using lab09.Models;
using lab09.Views;

// KONTEKST BAZY DANYCH
var builder = WebApplication.CreateBuilder(args);

// Dodanie usług
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=dane.db"));

var app = builder.Build();

// ŚRODOWISKO
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Obsługa błędu 404 – przekierowanie na /IO/Logowanie
app.Use(async (ctx, next) =>
{
    await next();

    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
    {
        var originalPath = ctx.Request.Path.Value;
        ctx.Items["originalPath"] = originalPath;
        ctx.Request.Path = "/IO/Logowanie";
        await next();
    }
});

// Tworzenie bazy danych i danych testowych
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Ensure database is created and all tables exist
    db.Database.EnsureCreated();

    // Dodanie przykładowych użytkowników, jeśli ich nie ma
    if (!db.Loginy.Any())
    {
        var token1 = Guid.NewGuid().ToString();
        var token2 = Guid.NewGuid().ToString();

        db.Loginy.Add(new Uzytkownik
        {
            Login = "admin",
            Haslo = HashHelper.HashMD5("admin"),
            Token = token1,
            Rola = "admin"
        });

        db.Loginy.Add(new Uzytkownik
        {
            Login = "user",
            Haslo = HashHelper.HashMD5("user"),
            Token = token2,
            Rola = "user"
        });

        Console.WriteLine("Admin token: {0}", token1);
        Console.WriteLine("User token: {0}", token2);

        db.SaveChanges();
    }

    // Możesz też dodać dane testowe dla Acts, Layers, Enemies, jeśli chcesz
    if (!db.Acts.Any())
    {
        db.Acts.Add(new Act { ActName = "Act I" });
        db.SaveChanges();
    }

    if (!db.Layers.Any())
    {
        db.Layers.Add(new Layer { LayerName = "Layer 1" });
        db.SaveChanges();
    }

    if (!db.Enemies.Any())
    {
        db.Enemies.AddRange(
            new Enemy { Name = "Soldier", Type = "Basic", Health = 100, Weight = "Medium" },
            new Enemy { Name = "Boss", Type = "Heavy", Health = 500, Weight = "Heavy" }
        );
        db.SaveChanges();
    }
}

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=IO}/{action=Logowanie}/{id?}");

app.Run();

// POMOCNICZA KLASA DO HASHOWANIA
public static class HashHelper
{
    public static string HashMD5(string input)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(input);
        var hash = md5.ComputeHash(bytes);
        return Convert.ToHexString(hash); // .NET 5+
    }
}
