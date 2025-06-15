using System.ComponentModel.DataAnnotations;
using API.Enums;

namespace API.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Nome é obrigatório.")] 
    public string Name { get; set; }    
    
    [Required(ErrorMessage = "O e-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "Digite um e-mail válido")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "A senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínino 6 caracteres")]
    public string Password { get; set; }
    public RoleService Role { get; set; } = RoleService.Client;
    
    public DateTime Created { get; set; } = DateTime.Now;

    public List<Service> Services { get; set; } = new();
   
}