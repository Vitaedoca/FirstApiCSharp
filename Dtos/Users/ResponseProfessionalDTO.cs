namespace API.Dtos;

public class ResponseProfessionalDTO
{
    public Guid Id { get; set; }
    public string Name { get; init; }
    public int Duration { get; init; }
    public float Price { get; init; }
    public string NameProfessional { get; init; }
}
