namespace lab09.Models;

public class Uzytkownik
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Haslo { get; set; }
    public string Token { get; set; }
    public string Rola { get; set; } // "admin", "user"
}    