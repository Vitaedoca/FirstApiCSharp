namespace API.Entities;

public class Service
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Duration { get; set; }
    public float Price { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public List<User> Professionals { get; set; } = new();
}