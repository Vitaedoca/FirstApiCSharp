using API.Context;
using API.Dtos.Availability;
using API.Entities;
using API.Enums;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AvailabilityController: ControllerBase
{
    
    private readonly AgendaContext _context;
    
    public AvailabilityController(AgendaContext agendaContext)
    {
        _context = agendaContext;
    }
    
    [HttpPost]
    public IActionResult CreateAvailability(CreateAvailabilityDTO dto)
    {
        var dbProfessional = _context.Users.Any(e => e.Id == dto.ProfessionalId
                                                      && e.Role == RoleService.Professional);

        if (dbProfessional == false)
            throw new Exception("Professional não existe");

        var availability = new Availability()
        {
            ProfessionalId = dto.ProfessionalId,
            DayOfWeek = dto.DayOfWeek,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };

        
        _context.Availabilities.Add(availability);
        _context.SaveChanges();
        return Ok(availability);
    }

    [HttpDelete]
    public IActionResult DeleteAvailability(Guid id)
    {
        var dbAvailability = _context.Availabilities.Find(id);
        _context.Availabilities.Remove(dbAvailability);
        _context.SaveChanges();
        return Ok("Disponibilidade deletada com sucesso");
    }
    
}