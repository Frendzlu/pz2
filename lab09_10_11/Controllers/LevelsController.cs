using lab09.Models;
using lab09.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab09.Controllers;

[Route("[controller]")]
public class LevelsController : Controller
{
    private readonly AppDbContext _context;
    public LevelsController(AppDbContext context) => _context = context;

    // Web UI methods
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        var levels = await _context.Levels
            .Include(l => l.EnemiesLevels)
                .ThenInclude(el => el.Enemy)
            .ToListAsync();
        return View(levels);
    }
    [HttpGet("Edit")]
    public async Task<IActionResult> Edit(int id)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        var level = await _context.Levels
            .Include(l => l.EnemiesLevels)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (level == null) return NotFound();

        ViewBag.Acts = await _context.Acts.ToListAsync();
        ViewBag.Layers = await _context.Layers.ToListAsync();
        ViewBag.AllEnemies = await _context.Enemies.ToListAsync();
        ViewBag.SelectedEnemyIds = level.EnemiesLevels.Select(el => el.EnemyId).ToArray();
        return View(level);
    }

    [HttpPost("Edit ")]
    public async Task<IActionResult> Edit(Level level, int[] selectedEnemies)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        if (!ModelState.IsValid)
        {
            ViewBag.AllEnemies = await _context.Enemies.ToListAsync();
            ViewBag.SelectedEnemyIds = selectedEnemies;
            return View(level);
        }

        _context.Entry(level).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        var existingRelations = _context.EnemiesLevels.Where(el => el.LevelId == level.Id);
        _context.EnemiesLevels.RemoveRange(existingRelations);

        foreach (var enemyId in selectedEnemies)
        {
            _context.EnemiesLevels.Add(new EnemiesLevels { EnemyId = enemyId, LevelId = level.Id });
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    [HttpGet("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        var level = await _context.Levels.FindAsync(id);
        if (level != null)
        {
            _context.Levels.Remove(level);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");
        
        ViewBag.Acts = await _context.Acts.ToListAsync();
        ViewBag.Layers = await _context.Layers.ToListAsync();
        ViewBag.AllEnemies = await _context.Enemies.ToListAsync();
        
        return View();
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(Level level, int[] selectedEnemies)
    {
        if (HttpContext.Session.GetString("zalogowany") != "true")
            return RedirectToAction("Logowanie", "IO");

        if (!ModelState.IsValid)
        {
            ViewBag.AllEnemies = await _context.Enemies.ToListAsync();
            return View(level);
        }

        _context.Levels.Add(level);
        await _context.SaveChangesAsync();

        foreach (var enemyId in selectedEnemies)
        {
            _context.EnemiesLevels.Add(new EnemiesLevels { EnemyId = enemyId, LevelId = level.Id });
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }


    // REST API methods

    private bool ValidateUser(string username, string token)
    {
        return _context.Loginy.Any(u => u.Login == username && u.Token == token);
    }

    [HttpGet("api")]
    public async Task<IActionResult> GetAll([FromQuery] string username, [FromQuery] string token)
    {
        if (!ValidateUser(username, token))
            return Unauthorized();

        var levels = await _context.Levels
            .Include(l => l.EnemiesLevels)
                .ThenInclude(el => el.Enemy)
            .ToListAsync();

        return Ok(levels);
    }

    [HttpGet("api/{id}")]
    public async Task<IActionResult> Get(int id, [FromQuery] string username, [FromQuery] string token)
    {
        if (!ValidateUser(username, token))
            return Unauthorized();

        var level = await _context.Levels
            .Include(l => l.EnemiesLevels)
                .ThenInclude(el => el.Enemy)
            .FirstOrDefaultAsync(l => l.Id == id);

        return level == null ? NotFound() : Ok(level);
    }

    [HttpPost("api")]
    public async Task<IActionResult> Post([FromQuery] string username, [FromQuery] string token, [FromBody] Level level)
    {
        if (!ValidateUser(username, token))
            return Unauthorized();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Levels.Add(level);
        await _context.SaveChangesAsync();

        // Optional: If the incoming Level object includes EnemiesLevels, add them too
        if (level.EnemiesLevels != null)
        {
            foreach (var el in level.EnemiesLevels)
            {
                _context.EnemiesLevels.Add(new EnemiesLevels
                {
                    LevelId = level.Id,
                    EnemyId = el.EnemyId
                });
            }
            await _context.SaveChangesAsync();
        }

        return Ok(level);
    }

    [HttpPut("api/{id}")]
    public async Task<IActionResult> Put(int id, [FromQuery] string username, [FromQuery] string token, [FromBody] Level level)
    {
        if (!ValidateUser(username, token))
            return Unauthorized();

        if (id != level.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Entry(level).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        // Update enemies relation
        var existingRelations = _context.EnemiesLevels.Where(el => el.LevelId == id);
        _context.EnemiesLevels.RemoveRange(existingRelations);

        if (level.EnemiesLevels != null)
        {
            foreach (var el in level.EnemiesLevels)
            {
                _context.EnemiesLevels.Add(new EnemiesLevels
                {
                    LevelId = id,
                    EnemyId = el.EnemyId
                });
            }
        }

        await _context.SaveChangesAsync();

        return Ok(level);
    }

    [HttpDelete("api/{id}")]
    public async Task<IActionResult> DeleteApi(int id, [FromQuery] string username, [FromQuery] string token)
    {
        if (!ValidateUser(username, token))
            return Unauthorized();

        var level = await _context.Levels.FindAsync(id);
        if (level == null)
            return NotFound();

        _context.Levels.Remove(level);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
