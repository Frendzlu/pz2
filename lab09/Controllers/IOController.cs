using lab09.Views;

namespace lab09.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

public class IOController : Controller
{
    private const string LOGIN = "admin";
    private const string PASSWORD = "test";

    private readonly AppDbContext _db;
    
    public IOController(AppDbContext db)
    {
        _db = db;
    }
    
    public IActionResult Logowanie()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Logowanie(string login, string haslo)
    {
        var hashed = HashHelper.HashMD5(haslo);
        var user = _db.Loginy.FirstOrDefault(u => u.Login == login && u.Haslo == hashed);

        if (user != null)
        {
            HttpContext.Session.SetString("zalogowany", "true");
            return RedirectToAction("Panel");
        }

        ViewBag.Error = "Błędny login lub hasło.";
        return View();
    }


    public IActionResult Panel()
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie");

        return View();
    }

    [HttpPost]
    public IActionResult Wyloguj()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Logowanie");
    }
}
