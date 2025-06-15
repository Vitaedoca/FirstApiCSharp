using API.Context;
using API.Dtos;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly AgendaContext _context;
    
    public UsersController(AgendaContext agendaContext)
    {
        // Enjeção de dependência, permite acesso ao banco de dados 
        _context = agendaContext;
    }
    
    [HttpPost] 
    public IActionResult CreateUser(CreateUserDTO dto)
    {
        var verifyUserExistent = _context.Users.Any(u => u.Email == dto.Email);

        if (verifyUserExistent)
            throw new ArgumentException("Usuário já existe");
        
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = passwordHash
        };
        
        _context.Users.Add(user);
        _context.SaveChanges();
        
        var responseUser = new ResponseUserDTO(
             user.Id,
             user.Name,
             user.Email
        );
        
        return Ok(responseUser);
    }
    
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();
        return Ok(user);
    }
    
    [HttpGet("ObterPorNome")]
    public IActionResult GetByName(string nome)
    {
        var user = _context.Users.Where(i => i.Name.Contains(nome));
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _context.Users.ToList();
        return Ok(users);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, UpdateUserDTO dto)
    {
        var userBanco = _context.Users.Find(id);
        if (userBanco == null) return NotFound();
        

        if (dto.Name != null)
            userBanco.Name = dto.Name;
        
        if (dto.Email != null)
            userBanco.Email = dto.Email;

        if (dto.Role != null)
            userBanco.Role = dto.Role.Value;

        if (dto.Password != null)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            userBanco.Password = passwordHash;
        }
        
        
        _context.Users.Update(userBanco);
        _context.SaveChanges();
        return Ok(userBanco);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();
        _context.Users.Remove(user);
        _context.SaveChanges();
        return Ok(user);
    }

    [HttpGet("/getProfessionalServiceId/{id}")]
    public IActionResult GetProfessionalFromService(Guid id)
    {
        var dbProfessionals = _context.Services
            .Include(e => e.Professionals)
            .FirstOrDefault(e => e.Id == id);

        var responseProfessionalsDTO = dbProfessionals.Professionals.Select(e => new ResponseProfessionalsDTO()
        {
            ProfessionalId = e.Id,
            ProfessionalName = e.Name
            
        }).ToList();
        
        return Ok(responseProfessionalsDTO);
    }
}