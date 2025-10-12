using Microsoft.EntityFrameworkCore;
using SpaceHub.Application.Data;
using SpaceHub.Application.Dtos;

namespace SpaceHub.Application.Repository;

public interface IpostRepository
{
  Task<List<PostDto>> GetPostsList(int offset);
}

public class PostRepository : IpostRepository
{
  public AppDbContext _appDbContext;
  public PostRepository(AppDbContext appDbContext)
  {
    _appDbContext = appDbContext;
  }
  public async Task<List<PostDto>> GetPostsList(int offset)
  {
    return await _appDbContext.Posts
    .OrderBy(post => post.id_post)
    .Include(post => post.CategoryFk)
    .Select(post => new PostDto
    {
      Category = post.CategoryFk != null ? post.CategoryFk.category : "Sin categoria",
      ImagePath = post.path_image,
      Title = post.title,
      Id = post.id_post
    })
    .Take(10)
    .Skip(offset)
    .ToListAsync();
  }
}