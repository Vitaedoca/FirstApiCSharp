using System.Data.Common;
using API.Context;
using API.Dtos.Appointments;
using API.Entities;
using API.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route ("[controller]")]
public class AppointmentsController: ControllerBase
{
    private readonly AgendaContext _context;
    
    public AppointmentsController(AgendaContext agendaContext)
    {
        _context = agendaContext;
    }

    [HttpPost]
    public IActionResult CreateAppointment(CreateAppointmentsDTO dto)
    {
        var dbClient = _context.Users.First(e => e.Id == dto.ClientId
                                                 && e.Role == RoleService.Client);

        var dbProfessional = _context.Users.First(e => e.Id == dto.ProfessionalId
                                                       && e.Role == RoleService.Professional);

        var dbService = _context.Services.First(e => e.Id == dto.ServiceId);

        var appointments = new Appointments()
        {
            ClientId = dto.ClientId,
            Client = dbClient,
            ProfessionalId = dto.ProfessionalId,
            Professional = dbProfessional,
            ServiceId = dto.ServiceId,
            Service = dbService,
            Date = dto.Date,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };
        
        _context.Appointments.Add(appointments);
        _context.SaveChanges();
        return Ok(appointments);
    }

    [HttpGet]
    public IActionResult GetAppointments()
    {
        var dbAppointments = _context.Appointments
            .Include(a => a.Professional)
            .Include(a => a.Service)
            .Include(a => a.Client);
        
        return Ok(dbAppointments);
    }

    [HttpGet("getProfessional/{id}")]
    public IActionResult GetAppointmentsForProfessional(Guid id)
    {
        var dbAppointments = _context.Appointments.Where(e => e.ProfessionalId == id)
            .Include(a => a.Service)
            .Include(a => a.Client)
            .ToList();
        
        var appointments = dbAppointments.Select(a => new GetAppointmentsForProfessionalDTO
        {
            Id = a.Id,
            ClientName = a.Client.Name,
            ServiceName = a.Service.Name,
            Date = a.Date,
            StartTime = a.StartTime,
            EndTime = a.EndTime,
            Status = a.Status
            
        }).ToList();
        
        return Ok(appointments);
    }

    [HttpGet("getProfessionalClient/{id}")]
    public IActionResult GetAppointmentsForClient(Guid id)
    {
        var dbAppointments = _context.Appointments.Where(e => e.ClientId == id)
            .Include(e => e.Service)
            .Include(e => e.Professional)
            .ToList();

        var appointments = dbAppointments.Select(a => new GetAppointmentsForClients()
        {
            Id = a.Id,
            ProfessionalName = a.Professional.Name,
            ServiceName = a.Service.Name,
            Date = a.Date,
            StartTime = a.StartTime,
            EndTime = a.EndTime,
            Status = a.Status
        });
        
        return Ok(appointments);
    }

    [HttpGet("available-times")]
    public IActionResult GetAvailableTimes(Guid ProfessionalId, Guid ClientId, DateTime Date)
    {
        var dbClient = _context.Appointments.Any(e => e.ClientId == ClientId && e.Date == Date);

        if (dbClient)
            return Ok("Usuário já tem agendamento neste dia");
        
        var availability = _context.Availabilities.First(a => a.ProfessionalId == ProfessionalId);
        
        var duration = TimeSpan.FromHours(1);
        List<TimeSpan> availableTimes = new List<TimeSpan>();
        
        for (var time = availability.StartTime; time <= availability.EndTime; time += duration)
        {
            availableTimes.Add(time);
        }

        var availableAppointments = _context.Appointments
            .Where(e => e.ProfessionalId == ProfessionalId && e.Date == Date)
            .Select(e => e.StartTime)
            .ToList();

        var result = availableTimes
            .Where(e => !availableAppointments.Contains(e))
            .ToList();

        return Ok(result);
    }
}