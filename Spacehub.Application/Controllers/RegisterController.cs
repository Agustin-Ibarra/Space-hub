using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceHub.Application.Dtos;
using SpaceHub.Application.Models;
using SpaceHub.Application.Repository;

namespace SpaceHub.Application.Controllers;

public class RegisterController : Controller
{
  private readonly IUserRepository _userRepository;
  public RegisterController(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  [HttpGet]
  [Route("/register")]
  public IActionResult Register()
  {
    return View();
  }

  [HttpPost]
  [Route("/api/register")]
  public async Task<IActionResult> ApiRegister([FromBody] RegisterDto dto)
  {
    if (!ModelState.IsValid)
    {
      var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
      return BadRequest(new { error = errors });
    }
    else
    {
      var user = new User
      {
        email = dto.Email,
        fullname = dto.Fullname,
        user_password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        username = dto.Username,
        id_role = 1
      };
      try
      {
        await _userRepository.CreateUser(user);
        return Created();
      }
      catch (DbUpdateException)
      {
        return Conflict(new { error = "ya existe un usuario con es email" });
      }
    }
  }
}