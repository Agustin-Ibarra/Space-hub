using System.ComponentModel.DataAnnotations;

namespace SpaceHub.Application.Dtos;

public class CartItemDto
{
  public int IdItem { get; set; }
  public int Quantity { get; set; }
}

public class CartItemsDto
{
  public int IdCart { get; set; }
  public int IdItem { get; set; }
  public required string ItemName { get; set; }
  public required string ImagePath { get; set; }
  public decimal UnitPrice { get; set; }
  public int Quantity { get; set; }
}

public class CartDto
{
  [Range(1, 1000, ErrorMessage = "el id del carrito de compras esta fuera del rango")]
  public int IdCart { get; set; }
  [Range(1, 100, ErrorMessage = "El id del articulo esta fuera del rango")]
  public int IdItem { get; set; }
  [Range(1, 5, ErrorMessage = "La cantidad esta fuera del rango")]
  public int Quantity { get; set; }
}