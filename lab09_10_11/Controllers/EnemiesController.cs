using lab09.Models;
using lab09.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab09.Controllers;

[Route("[controller]")]
public class EnemiesController : Controller
{
    private readonly AppDbContext _context;

    public EnemiesController(AppDbContext context) => _context = context;

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        return View(await _context.Enemies.ToListAsync());
    }

    [HttpGet("Edit")]
    public async Task<IActionResult> Edit(int id)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        var enemy = await _context.Enemies.FindAsync(id);
        return enemy == null ? NotFound() : View(enemy);
    }

    [HttpPost("Edit")]
    public async Task<IActionResult> Edit(Enemy enemy)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        _context.Enemies.Update(enemy);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        var enemy = await _context.Enemies.FindAsync(id);
        if (enemy != null)
        {
            _context.Enemies.Remove(enemy);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        return View();
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(Enemy enemy)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        _context.Enemies.Add(enemy);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    // REST API
    [HttpGet("api")]
    public async Task<IActionResult> GetAll([FromQuery] string username, [FromQuery] string token)
    {
        if (!_context.Loginy.Any(u => u.Login == username && u.Token == token))
            return Unauthorized();

        return Ok(await _context.Enemies.ToListAsync());
    }

    [HttpGet("api/{id}")]
    public async Task<IActionResult> Get(int id, [FromQuery] string username, [FromQuery] string token)
    {
        if (!_context.Loginy.Any(u => u.Login == username && u.Token == token))
            return Unauthorized();

        var enemy = await _context.Enemies.FindAsync(id);
        return enemy == null ? NotFound() : Ok(enemy);
    }

    [HttpPost("api")]
    public async Task<IActionResult> Post([FromQuery] string username, [FromQuery] string token, [FromBody] Enemy enemy)
    {
        if (!_context.Loginy.Any(u => u.Login == username && u.Token == token))
            return Unauthorized();

        _context.Enemies.Add(enemy);
        await _context.SaveChangesAsync();
        return Ok(enemy);
    }

    [HttpPut("api/{id}")]
    public async Task<IActionResult> Put(int id, [FromQuery] string username, [FromQuery] string token, [FromBody] Enemy enemy)
    {
        if (!_context.Loginy.Any(u => u.Login == username && u.Token == token))
            return Unauthorized();

        if (id != enemy.Id) return BadRequest();
        _context.Entry(enemy).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return Ok(enemy);
    }

    [HttpDelete("api/{id}")]
    public async Task<IActionResult> DeleteApi(int id, [FromQuery] string username, [FromQuery] string token)
    {
        if (!_context.Loginy.Any(u => u.Login == username && u.Token == token))
            return Unauthorized();

        var enemy = await _context.Enemies.FindAsync(id);
        if (enemy == null) return NotFound();
        _context.Enemies.Remove(enemy);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
