namespace SpaceHub.Application.Dtos;

public class ItemDto
{
  public int IdItem { get; set; }
  public required string ItemName { get; set; }
  public required string ItemImage { get; set; }
  public double ItemUnitPrice { get; set; }
}