namespace lab09.Models;

public class Level
{
    public int Id { get; set; }

    // Foreign Keys
    public int ActId { get; set; }
    public Act Act { get; set; }

    public int LayerId { get; set; }
    public Layer Layer { get; set; }

    public string Name { get; set; }
    public string PRankTime { get; set; }
    public int PRankKills { get; set; }
    public int PRankStyle { get; set; }
    public string Challenge { get; set; }

    public ICollection<EnemiesLevels> EnemiesLevels { get; set; }
}
