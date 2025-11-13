namespace SpaceHub.Application.Dtos;

public class GeminiResponseDto
{
  public required List<Candidate> candidates { get; set; }
}

public class Candidate
{
  public required Content content { get; set; }
}

public class Content
{
  public required List<Part> parts { get; set; }
}

public class Part
{
  public required string text { get; set; }
}