using System.ComponentModel.DataAnnotations;

namespace SpaceHub.Application.Dtos;

public class PromptDto
{
  [Required(ErrorMessage = "El id del chat es requerido")]
  public int idChat { get; set; }

  [Required(ErrorMessage = "El mensaje es requerido")]
  [StringLength(maximumLength: 300, MinimumLength = 5, ErrorMessage = "el mensaje debe incluir por lo menos un minimo de 5 letras y un maximo de 300 letras")]
  [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÜüÑñ0-9\-—\/.,():'‘’“”¿?\s\r\n]+$", ErrorMessage = "El mensaje no admite caracteres especiales")]
  public required string UserPrompt { get; set; }
}