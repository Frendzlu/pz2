namespace lab09.Models;

public class Act
{
    public int Id { get; set; }
    public string ActName { get; set; }

    public ICollection<Level> Levels { get; set; }
}
