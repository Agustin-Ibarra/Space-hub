using System.ComponentModel.DataAnnotations;

namespace SpaceHub.Application.Dtos;

public class RegisterDto
{
  [Required(ErrorMessage = "El nombre completo es requerido")]
  [Length(4,30,ErrorMessage = "El nombre completo debe ser minimo de 4 caracteres y maximo 30 caracteres")]
  [RegularExpression(@"^[a-zA-Z0-1\s]+$", ErrorMessage = "El nombre de usuario solo admite letras, numeros, y guiones bajos")]
  public required string Fullname { get; set; }

  [Required(ErrorMessage = "El correo es requerido")]
  [EmailAddress(ErrorMessage = "El correo no es valido")]
  public required string Email { get; set; }

  [Required(ErrorMessage = "El nombre de usuario es requerido")]
  [RegularExpression(@"^[a-zA-Z\w]+$", ErrorMessage = "El nombre de usuario solo admite letras y espacios en blanco")]
  [Length(4,30,ErrorMessage = "El nombre de usuario debe ser minimo de 4 caracteres y maximo 30 caracteres")]
  public required string Username { get; set; }

  [Required(ErrorMessage = "La contraseña es requerida")]
  [RegularExpression(@"^[a-zA-Z0-1]+$", ErrorMessage = "La contraseña solo admite letras y numeros")]
  [Length(8,30,ErrorMessage = "la contraseña debe ser minimo de 8 caracteres y maximo 30 caracteres")]
  public required string Password { get; set; }
}

public class LoginDto
{
  [Required(ErrorMessage = "El nombre de usuario es requerido")]
  public required string Username { get; set; }
  [Required(ErrorMessage = "La contraseña es requerida")]
  public required string Password { get; set; }
}

public class UserDto
{
  public int IdUser { get; set; }
  public required string Password { get; set; }
  public required string Role {get;set;}
}