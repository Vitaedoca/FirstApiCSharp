using API.Enums;

namespace API.Dtos;

public  record UpdateUserDTO(string? Name, string? Email, RoleService? Role, string? Password);