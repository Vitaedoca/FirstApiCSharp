using API.Context;
using API.Dtos;
using API.Entities;
using API.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ServicesController: ControllerBase
{
    private readonly AgendaContext _context;

    public ServicesController(AgendaContext agendaContext)
    {
        _context = agendaContext;
    }
    
    [HttpPost]
    public IActionResult CreateService(CreateServiceDTO dto)
    {
        // Existe um item nessa lista que satisfaça minha condição?
        var nameServiceExists = _context.Services.Any(x => x.Name == dto.Name);
        
        // Retorna uma lista com o profissional que eu passei e verifica se a role dele é de um profissional

        if (nameServiceExists)
            throw new Exception("Já existe ums serviço com esse nome");

        var service = new Service
        {
            Name = dto.Name,
            Duration = dto.Duration,
            Price = dto.Price,
        };
        
        _context.Services.Add(service);
        _context.SaveChanges();

        var responseProfessional = new ResponseProfessionalDTO
        {
            Id = service.Id,
            Name = service.Name,
            Duration = service.Duration,
            Price = service.Price,
        };
        
        return Ok(responseProfessional);
    }


    [HttpPost("add-professional")]
    public IActionResult AddProfessionalToService(AddProfessionalServiceDTO addProfessionalServiceDTO)
    {
        var service = _context.Services
            .Include(s => s.Professionals)
            .FirstOrDefault(s => s.Id == addProfessionalServiceDTO.ServiceId);

        if (service == null)
            return NotFound("Serviço não encontrado");
        
        var professionalDB = _context.Users.FirstOrDefault(x => x.Id == addProfessionalServiceDTO.ProfessionalId);
        
        if (professionalDB == null)
            return NotFound("Usuário não encontrado");
        
        service.Professionals.Add(professionalDB);
        _context.SaveChanges();
        var _professional = service.Professionals.FirstOrDefault(x => x.Id == addProfessionalServiceDTO.ProfessionalId).Name;
        var _service = service.Name;
        
        return Ok($"Professional {_professional} agora realiza {_service}");
    }

    [HttpPut("{id}")]
    public IActionResult UpdateService(Guid id,Service service)
    {
        var serviceBanco = _context.Services.Find(id);
        
        serviceBanco.Name = service.Name;
        serviceBanco.Duration = service.Duration;
        serviceBanco.Price = service.Price;
        
        _context.Services.Update(serviceBanco);
        _context.SaveChanges();

        var serviceProfessionalDTO = new ResponseProfessionalDTO
        {
            Name = serviceBanco.Name,
            Duration = serviceBanco.Duration,
            Price = serviceBanco.Price,
        };
        
        return Ok(serviceProfessionalDTO);
    }
    
    [HttpGet("{serviceId}")]
    public IActionResult GetServiceId(Guid serviceId)
    {
        var service = _context.Services.Find(serviceId);
        return Ok(service);
    }

    [HttpGet("getServices")]
    public IActionResult GetServices()
    {
        var services = _context.Services.ToList();
        return Ok(services);
    }

    [HttpDelete("{serviceId}")]
    public IActionResult DeleteService(Guid serviceId)
    {
        var service = _context.Services.Find(serviceId);
        
        _context.Services.Remove(service);
        _context.SaveChanges();
        return Ok($"Service {service.Name} deletado com sucesso");
    }
}