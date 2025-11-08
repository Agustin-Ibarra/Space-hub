using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SpaceHub.Application.Dtos;
using SpaceHub.Application.Hubs;
using SpaceHub.Application.Models;
using SpaceHub.Application.Repository;

namespace SpaceHub.Application.Controllers;

public class PostController : Controller
{
	private readonly IpostRepository _postRepository;
	private readonly IHubContext<NotifyHub> _hubContext;
	public PostController(IpostRepository postRepository, IHubContext<NotifyHub> hubContext)
	{
		_postRepository = postRepository;
		_hubContext = hubContext;
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
	public async Task<IActionResult> ApiPosts(int offset)
	{
		Console.WriteLine(offset);
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
	public async Task<IActionResult> ApiPostDetail(int idPost)
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
	public async Task<IActionResult> ApiPostSuggestion(int idPost)
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

	[Authorize]
	[HttpPost]
	[Route("/api/posts")]
	public async Task<IActionResult> ApiCreatePost([FromBody] PostDataDto postData)
	{
		var role = User.FindFirstValue(ClaimTypes.Role);
		if (role == "editor")
		{
			if (!ModelState.IsValid)
			{
				var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
				return BadRequest(new { error = errors });
			}
			else
			{
				var post = new Post
				{
					path_image = postData.ImagePath,
					post_description = postData.PostDescription,
					text_content = postData.TextContent,
					title = postData.Title,
					id_category = postData.IdCategory,
					created_at = DateTime.Now
				};

				await _postRepository.CreatePost(post);

				await _hubContext.Clients.All.SendAsync("ReceiveNotification", "Se publico un nuevo posts");

				return Created("/posts", postData);
			}
		}
		else
		{
			return Unauthorized();
		}
	}
}