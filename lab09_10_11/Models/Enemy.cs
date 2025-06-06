namespace lab09.Models;

public class Enemy
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public double Health { get; set; }
    public string Weight { get; set; }

    public ICollection<EnemiesLevels> EnemiesLevels { get; set; }
}
