using lab09.Views;
using Microsoft.AspNetCore.Mvc;

namespace lab09.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    public AuthController(AppDbContext db) => _db = db;

    [HttpGet("validate")]
    public IActionResult Validate(string login, string token)
    {
        var user = _db.Loginy.FirstOrDefault(u => u.Login == login && u.Token == token);
        return user != null ? Ok() : Unauthorized();
    }
}