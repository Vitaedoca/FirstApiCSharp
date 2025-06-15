namespace API.Dtos.Appointments;

public record CreateAppointmentsDTO(Guid ClientId, Guid ProfessionalId, Guid ServiceId, DateTime Date, TimeSpan StartTime, TimeSpan EndTime);