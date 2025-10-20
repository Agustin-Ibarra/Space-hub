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

public class CartDto{
  public int IdCart { get; set; }
  public int IdItem {get;set;}
  public int Quantity { get; set; }
}