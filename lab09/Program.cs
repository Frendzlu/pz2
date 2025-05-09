using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using lab09.Models;

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

// Tworzenie bazy i danych testowych
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Loginy.Any())
    {
        db.Loginy.Add(new Uzytkownik
        {
            Login = "admin",
            Haslo = HashHelper.HashMD5("test")
        });

        db.Dane.Add(new Dane { Tresc = "Przykładowy wpis" });
        db.SaveChanges();
    }
}

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=IO}/{action=Logowanie}/{id?}");

app.Run();

public class AppDbContext : DbContext
{
    public DbSet<Uzytkownik> Loginy { get; set; }
    public DbSet<Dane> Dane { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}

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

// KONFIGURACJA APLIKACJI