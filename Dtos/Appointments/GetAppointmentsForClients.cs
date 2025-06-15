namespace API.Dtos.Appointments;

public class GetAppointmentsForClients
{
    public Guid Id { get; set; }
    public string ProfessionalName { get; set; }
    public string ServiceName { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Status { get; set; }
}