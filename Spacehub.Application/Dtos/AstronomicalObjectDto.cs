namespace SpaceHub.Application.Dtos;

public class AstronomicalObjectDto
{
  public int Id { get; set; }
  public required string Title { get; set; }
  public required string ImagePath { get; set; }
  public required string Category { get; set; }
}

public class AstronomicalObjectDetail
{
  public required string Title { get; set; }
  public required string ImagePath { get; set; }
  public required string TextContent { get; set; }
  public required string Category { get; set; }
}