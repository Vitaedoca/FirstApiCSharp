namespace API.Dtos.Appointments;

public class GetAppointmentsForProfessionalDTO()
{
    public Guid Id { get; set; }
    public string ClientName { get; set; }
    public string ServiceName { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Status { get; set; }
};