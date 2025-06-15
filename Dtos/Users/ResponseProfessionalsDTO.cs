namespace API.Dtos;

public record ResponseProfessionalsDTO()
{
    public Guid ProfessionalId { get; set; }
    public string ProfessionalName { get; set; }
};