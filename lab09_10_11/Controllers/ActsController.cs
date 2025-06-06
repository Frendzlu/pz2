using lab09.Models;
using lab09.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab09.Controllers;

[Route("[controller]")]
public class ActsController : Controller
{
    private readonly AppDbContext _context;

    public ActsController(AppDbContext context) => _context = context;

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        return View(await _context.Acts.ToListAsync());
    }

    [HttpGet("Edit")]
    public async Task<IActionResult> Edit(int id)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        var act = await _context.Acts.FindAsync(id);
        return act == null ? NotFound() : View(act);
    }

    [HttpPost("Edit")]
    public async Task<IActionResult> Edit(Act act)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        _context.Acts.Update(act);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        var act = await _context.Acts.FindAsync(id);
        if (act != null)
        {
            _context.Acts.Remove(act);
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
    public async Task<IActionResult> Create(Act act)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        _context.Acts.Add(act);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    // REST API
    [HttpGet("api")]
    public async Task<IActionResult> GetAll([FromQuery] string username, [FromQuery] string token)
    {
        if (!_context.Loginy.Any(u => u.Login == username && u.Token == token))
            return Unauthorized();

        return Ok(await _context.Acts.ToListAsync());
    }

    [HttpGet("api/{id}")]
    public async Task<IActionResult> Get(int id, [FromQuery] string username, [FromQuery] string token)
    {
        if (!_context.Loginy.Any(u => u.Login == username && u.Token == token))
            return Unauthorized();

        var act = await _context.Acts.FindAsync(id);
        return act == null ? NotFound() : Ok(act);
    }

    [HttpPost("api")]
    public async Task<IActionResult> Post([FromQuery] string username, [FromQuery] string token, [FromBody] Act act)
    {
        if (!_context.Loginy.Any(u => u.Login == username && u.Token == token))
            return Unauthorized();

        _context.Acts.Add(act);
        await _context.SaveChangesAsync();
        return Ok(act);
    }

    [HttpPut("api/{id}")]
    public async Task<IActionResult> Put(int id, [FromQuery] string username, [FromQuery] string token, [FromBody] Act act)
    {
        if (!_context.Loginy.Any(u => u.Login == username && u.Token == token))
            return Unauthorized();

        if (id != act.Id) return BadRequest();
        _context.Entry(act).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return Ok(act);
    }

    [HttpDelete("api/{id}")]
    public async Task<IActionResult> DeleteApi(int id, [FromQuery] string username, [FromQuery] string token)
    {
        if (!_context.Loginy.Any(u => u.Login == username && u.Token == token))
            return Unauthorized();

        var act = await _context.Acts.FindAsync(id);
        if (act == null) return NotFound();
        _context.Acts.Remove(act);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
