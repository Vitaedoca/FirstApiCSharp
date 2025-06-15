namespace API.Dtos.Availability;

public record CreateAvailabilityDTO(Guid ProfessionalId, DayOfWeek DayOfWeek, TimeSpan StartTime, TimeSpan EndTime);