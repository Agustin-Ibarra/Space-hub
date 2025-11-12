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
		var posts = await _postRepository.GetPostsList(offset);
		return Ok(posts);
	}

	[HttpGet]
	[Route("/api/posts/info/{idPost}")]
	public async Task<IActionResult> ApiPostDetail(int idPost)
	{
		var post = await _postRepository.GetPostDetail(idPost);
		if (post != null)
		{
			return Ok(post);
		}
		else
		{
			return NotFound(new { error = $"No se encontro la publicacion con id: {idPost}" });
		}
	}

	[HttpGet]
	[Route("/api/posts/info/suggestion/{idPost}")]
	public async Task<IActionResult> ApiPostSuggestion(int idPost)
	{
		var suggestion = await _postRepository.GetPostsSuggestion(idPost);
		return Ok(suggestion);
	}

	[Authorize(Roles = "editor")]
	[HttpPost]
	[Route("/api/posts")]
	public async Task<IActionResult> ApiCreatePost([FromBody] PostDataDto postData)
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
			// al crear el post se envia una notificacion a los usuarios conectados en el hub
			await _hubContext.Clients.All.SendAsync("ReceiveNotification", "Se publico un nuevo posts");
			return Created("/posts", postData);
		}
	}
}