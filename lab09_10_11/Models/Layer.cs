namespace lab09.Models;

public class Layer
{
    public int Id { get; set; }
    public string LayerName { get; set; }

    public ICollection<Level> Levels { get; set; }
}
