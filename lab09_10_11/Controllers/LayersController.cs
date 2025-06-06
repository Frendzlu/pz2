using lab09.Models;
using lab09.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab09.Controllers;

[Route("[controller]")]
public class LayersController : Controller
{
    private readonly AppDbContext _context;

    public LayersController(AppDbContext context) => _context = context;

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        return View(await _context.Layers.ToListAsync());
    }
    [HttpGet("Edit")]
    public async Task<IActionResult> Edit(int id)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        var layer = await _context.Layers.FindAsync(id);
        return layer == null ? NotFound() : View(layer);
    }

    [HttpPost("Edit")]
    public async Task<IActionResult> Edit(Layer layer)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        _context.Layers.Update(layer);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    [HttpGet("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        var layer = await _context.Layers.FindAsync(id);
        if (layer != null)
        {
            _context.Layers.Remove(layer);
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
    public async Task<IActionResult> Create(Layer layer)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        _context.Layers.Add(layer);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    // REST API
    [HttpGet("api")]
    public async Task<IActionResult> GetAll([FromQuery] string username, [FromQuery] string token)
    {
        if (!_context.Loginy.Any(u => u.Login == username && u.Token == token))
            return Unauthorized();

        return Ok(await _context.Layers.ToListAsync());
    }

    [HttpGet("api/{id}")]
    public async Task<IActionResult> Get(int id, [FromQuery] string username, [FromQuery] string token)
    {
        if (!_context.Loginy.Any(u => u.Login == username && u.Token == token))
            return Unauthorized();

        var layer = await _context.Layers.FindAsync(id);
        return layer == null ? NotFound() : Ok(layer);
    }

    [HttpPost("api")]
    public async Task<IActionResult> Post([FromQuery] string username, [FromQuery] string token, [FromBody] Layer layer)
    {
        if (!_context.Loginy.Any(u => u.Login == username && u.Token == token))
            return Unauthorized();

        _context.Layers.Add(layer);
        await _context.SaveChangesAsync();
        return Ok(layer);
    }

    [HttpPut("api/{id}")]
    public async Task<IActionResult> Put(int id, [FromQuery] string username, [FromQuery] string token, [FromBody] Layer layer)
    {
        if (!_context.Loginy.Any(u => u.Login == username && u.Token == token))
            return Unauthorized();

        if (id != layer.Id) return BadRequest();
        _context.Entry(layer).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return Ok(layer);
    }

    [HttpDelete("api/{id}")]
    public async Task<IActionResult> DeleteApi(int id, [FromQuery] string username, [FromQuery] string token)
    {
        if (!_context.Loginy.Any(u => u.Login == username && u.Token == token))
            return Unauthorized();

        var layer = await _context.Layers.FindAsync(id);
        if (layer == null) return NotFound();
        _context.Layers.Remove(layer);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
