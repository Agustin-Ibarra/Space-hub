namespace SpaceHub.Application.Dtos;

public class PostDto
{
  public int Id { get; set; }
  public required string Title { get; set; }
  public required string Category { get; set; }
  public required string ImagePath {get;set;}
}

public class PostDetailDto
{
  public int Id { get; set; }
  public required string Title {get;set;}
  public required string Category { get; set; }
  public required string ImagePath { get; set; }
  public required string TextDescription { get; set; }
  public required string TextContent { get; set; }
  public DateTime CreatedAt { get; set; }
}