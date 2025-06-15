namespace API.Entities;

public class Availability
{
    public Guid Id { get; set; }
    public Guid ProfessionalId { get; set; }
    public User Professional { get; set; }
    
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}