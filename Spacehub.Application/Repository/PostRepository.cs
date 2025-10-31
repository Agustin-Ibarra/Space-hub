using Microsoft.EntityFrameworkCore;
using SpaceHub.Application.Data;
using SpaceHub.Application.Dtos;

namespace SpaceHub.Application.Repository;

public interface IpostRepository
{
  Task<List<PostDto>> GetPostsList(int offset);
  Task<PostDetailDto?> GetPostDetail(int idPost);
  Task<List<PostDto>> GetPostsSuggestion(int idPost);
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
    .OrderDescending()
    .Include(post => post.CategoryFk)
    .Select(post => new PostDto
    {
      Category = post.CategoryFk != null ? post.CategoryFk.category : "Sin categoria",
      ImagePath = post.path_image,
      Title = post.title,
      Id = post.id_post
    })
    .Skip(offset)
    .Take(12)
    .ToListAsync();
  }

  public async Task<PostDetailDto?> GetPostDetail(int idPost)
  {
    return await _appDbContext.Posts
    .OrderBy(post => post.id_post)
    .Include(post => post.CategoryFk)
    .Where(post => post.id_post == idPost)
    .Select(post => new PostDetailDto
    {
      Category = post.CategoryFk != null ? post.CategoryFk.category : "Sin categoria",
      ImagePath = post.path_image,
      TextContent = post.text_content,
      TextDescription = post.post_description,
      Title = post.title,
      Id = post.id_post,
      CreatedAt = post.created_at
    })
    .FirstOrDefaultAsync();
  }
  
  public async Task<List<PostDto>> GetPostsSuggestion(int idPost)
  {
    return await _appDbContext.Posts
    .OrderBy(post => post.id_post)
    .OrderDescending()
    .Include(post => post.CategoryFk)
    .Where(post => post.id_post != idPost)
    .Select(post => new PostDto
    {
      Category = post.CategoryFk != null ? post.CategoryFk.category : "Sin categoria",
      ImagePath = post.path_image,
      Title = post.title,
      Id = post.id_post
    })
    .Take(3)
    .ToListAsync();
  }
}