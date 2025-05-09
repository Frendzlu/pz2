using lab09.Views;

namespace lab09.Controllers;

using lab09.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

public class DaneController : Controller
{
    private readonly AppDbContext _db;

    public DaneController(AppDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        return View(_db.Dane.ToList());
    }

    public IActionResult Dodaj()
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        return View();
    }

    [HttpPost]
    public IActionResult Dodaj(string tresc)
    {
        _db.Dane.Add(new Dane { Tresc = tresc });
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
}
