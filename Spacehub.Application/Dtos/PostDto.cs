using System.ComponentModel.DataAnnotations;

namespace SpaceHub.Application.Dtos;

public class PostDto
{
  public int Id { get; set; }
  public required string Title { get; set; }
  public required string Category { get; set; }
  public required string ImagePath { get; set; }
}

public class PostDetailDto
{
  public int Id { get; set; }
  public required string Title { get; set; }
  public required string Category { get; set; }
  public required string ImagePath { get; set; }
  public required string TextDescription { get; set; }
  public required string TextContent { get; set; }
  public DateTime CreatedAt { get; set; }
}

public class PostDataDto
{
  [Range(1, 12, ErrorMessage = "el id de la categoria esta fuera del rango")]
  public int IdCategory { get; set; }

  [Required(ErrorMessage = "El titulo es requerido")]
  [StringLength(maximumLength: 250, MinimumLength = 10, ErrorMessage = "El titulo no puede ser menor a 10 caracteres y mayor a 100 caracteres")]
  [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÜüÑñ0-9\-—\/.,():'‘’“”\s\r\n]+$", ErrorMessage = "El titulo no admite caracteres especiales")]
  public required string Title { get; set; }

  [Required(ErrorMessage = "La descripcion es requerida")]
  [StringLength(maximumLength: 250, MinimumLength = 10, ErrorMessage = "La descripcion no puede ser menor a 10 caracteres y mayor a 250 caracteres")]
  [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÜüÑñ0-9\-—\/.,():'‘’“”\s\r\n]+$", ErrorMessage = "La descripcion no admite caracteres especiales")]
  public required string PostDescription { get; set; }

  [Required(ErrorMessage = "El texto es requerido")]
  [StringLength(maximumLength: 2500, MinimumLength = 100, ErrorMessage = "El texto no puede ser menor a 100 caracteres y mayor a 2500 caracteres")]
  [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÜüÑñ0-9\-—\/.,():'‘’“”\s\r\n]+$", ErrorMessage = "El texto no admite caracteres especiales")]
  public required string TextContent { get; set; }

  [Required(ErrorMessage = "La ruta de imagen es requerida")]
  [StringLength(maximumLength: 50, MinimumLength = 10, ErrorMessage = "La ruta de imagen no puede ser menor a 10 caracteres y mayor a 50 caracteres")]
  [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÜüÑñ0-9\-\/.,():\s\r\n]+$", ErrorMessage = "La ruta de imagen no admite caracteres especiales")]
  public required string ImagePath { get; set; }
}