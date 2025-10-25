using Microsoft.AspNetCore.Mvc;
using SpaceHub.Application.Repository;

namespace SpaceHub.Application.Controllers;

public class PostController : Controller
{
  public IpostRepository _postRepository;

  public PostController(IpostRepository postRepository)
  {
    _postRepository = postRepository;
  }

  [HttpGet]
  [Route("/posts")]
  public IActionResult Post()
  {
    return View();
  }

  [HttpGet]
  [Route("/posts/info")]
  public IActionResult PostInfo()
  {
    return View();
  }


  [HttpGet]
  [Route("/api/posts/{offset}")]
  public async Task<IActionResult> PostApi(int offset)
  {
    try
    {
      var posts = await _postRepository.GetPostsList(offset);
      return Ok(posts);
    }
    catch (Exception)
    {
      return StatusCode(503, new { error = "Ocurrio un error en la base de datos" });
    }
  }

  [HttpGet]
  [Route("/api/posts/info/{idPost}")]
  public async Task<IActionResult> PostDetail(int idPost)
  {
    try
    {
      var posts = await _postRepository.GetPostDetail(idPost);
      return Ok(posts);
    }
    catch (Exception)
    {
      return StatusCode(503, new { error = "Ocurrio un error en la base de datos" });
    }
  }

  [HttpGet]
  [Route("/api/posts/info/suggestion/{idPost}")]
  public async Task<IActionResult> PostSuggestion(int idPost)
  {
    try
    {
      var suggestion = await _postRepository.GetPostsSuggestion(idPost);
      return Ok(suggestion);
    }
    catch (Exception)
    {
      return StatusCode(503, new { error = "Ocurrio un error en la base de datos" });
    }
  }
}