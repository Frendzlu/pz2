namespace lab09.Views;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Uzytkownik> Loginy { get; set; }
    public DbSet<Dane> Dane { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}

public class Uzytkownik
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Haslo { get; set; }
}

public class Dane
{
    public int Id { get; set; }
    public string Tresc { get; set; }
}
