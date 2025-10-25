using Microsoft.EntityFrameworkCore;
using SpaceHub.Application.Data;
using SpaceHub.Application.Dtos;

namespace Spacehub.Application.Repository;

public interface IAstronomicalObjectRepository
{
  Task<List<AstronomicalObjectDto>> GetAstronomicalObjectList(int offset);
  Task<AstronomicalObjectDetail?> GetAstronomicalObject(int idObject);
  Task<List<AstronomicalObjectDto>> GetAstronomicalObjectSuggestion(int idObject);
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
    .Include(astronomical => astronomical.CategoryFk)
    .Select(astronomical => new AstronomicalObjectDto
    {
      Id = astronomical.id_object,
      ImagePath = astronomical.path_image,
      Title = astronomical.title,
      Category = astronomical.CategoryFk != null ? astronomical.CategoryFk.category : "Sin categoria"
    })
    .Skip(offset)
    .Take(10)
    .ToListAsync();
  }

  public async Task<AstronomicalObjectDetail?> GetAstronomicalObject(int idObject)
  {
    return await _AppDbContext.AstronomicalObjects
    .OrderBy(astronomical => astronomical.id_object)
    .Include(astronomical => astronomical.CategoryFk)
    .Where(astronomical => astronomical.id_object == idObject)
    .Select(astronomical => new AstronomicalObjectDetail
    {
      Category = astronomical.CategoryFk != null ? astronomical.CategoryFk.category : "Sin categoria",
      ImagePath = astronomical.path_image,
      TextContent = astronomical.text_content,
      Title = astronomical.title
    })
    .FirstOrDefaultAsync();
  }

  public async Task<List<AstronomicalObjectDto>> GetAstronomicalObjectSuggestion(int idObject)
  {
    return await _AppDbContext.AstronomicalObjects
    .OrderBy(astronomical => astronomical.id_object)
    .OrderDescending()
    .Include(astronomical => astronomical.CategoryFk)
    .Where(astronomical => astronomical.id_object != idObject)
    .Select(astronomical => new AstronomicalObjectDto
    {
      Category = astronomical.CategoryFk != null ? astronomical.CategoryFk.category : "Sin categoria",
      ImagePath = astronomical.path_image,
      Title = astronomical.title,
      Id = astronomical.id_object
    })
    .Take(3)
    .ToListAsync();
  }
}