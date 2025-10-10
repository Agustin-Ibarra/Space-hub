using Microsoft.EntityFrameworkCore;
using SpaceHub.Application.Data;
using SpaceHub.Application.Dtos;

namespace Spacehub.Application.Repository;

public interface IAstronomicalObjectRepository
{
  Task<List<AstronomicalObjectDto>> GetAstronomicalObjectList(int offset);
}

public class AstronomicalObjectRepository : IAstronomicalObjectRepository
{
  private readonly AppDbContext _AppDbContext;
  public AstronomicalObjectRepository(AppDbContext appDbContext)
  {
    _AppDbContext = appDbContext;
  }
  public async Task<List<AstronomicalObjectDto>> GetAstronomicalObjectList(int offset)
  {
    return await _AppDbContext.AstronomicalObjects
    .OrderBy(astronomical => astronomical.id_object)
    .Include( astronomical => astronomical.CategoryFk)
    .Select(astronomical => new AstronomicalObjectDto
    {
      ImagePath = astronomical.path_image,
      InformationText = astronomical.text_content,
      Title = astronomical.title,
      Category = astronomical.CategoryFk != null ? astronomical.CategoryFk.category : "Sin categoria"
    })
    .Skip(offset)
    .Take(10)
    .ToListAsync();
  }
}