using System.ComponentModel.DataAnnotations;

namespace SpaceHub.Application.Dtos;

public class ItemDto
{
  public int IdItem { get; set; }
  public required string ItemName { get; set; }
  public required string ItemImage { get; set; }
  public decimal ItemUnitPrice { get; set; }
}

public class ItemDetailDto
{
  public int IdItem { get; set; }
  public required string ItemName { get; set; }
  public required string ItemDescription { get; set; }
  public required string ItemImage { get; set; }
  public decimal ItemUnitPrice { get; set; }
  public int Itemstock { get; set; }
  public required string ItemCategory { get; set; }
}

public class ItemReserveDto
{
  public int IdItem { get; set; }
  public int Quantity { get; set; }
}

public class ItemsListDto
{
  [MinLength(1, ErrorMessage = "La lista no contiene articulos")]
  public required List<ItemReserveDto> Items { get; set; }
}