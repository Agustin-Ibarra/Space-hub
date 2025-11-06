using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SpaceHub.Application.Dtos;
using SpaceHub.Application.Repository;

namespace SpaceHub.Application.Controllers;

public class LoginController : Controller
{
  private readonly IUserRepository _userRepository;
  public LoginController(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  [HttpGet]
  [Route("/login")]
  public IActionResult Login()
  {
    return View();
  }

  [HttpPost]
  [Route("/api/login")]
  public async Task<IActionResult> ApiLogin([FromBody] LoginDto dto)
  {
    if (!ModelState.IsValid)
    {
      var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
      return BadRequest(new { error = errors });
    }
    else
    {
      var user = await _userRepository.FindUser(dto.Username);
      if (user != null)
      {
        if (BCrypt.Net.BCrypt.Verify(dto.Password, user.Password) == true)
        {
          var claims = new[] {
            new Claim(ClaimTypes.Role,user.Role),
            new Claim(ClaimTypes.NameIdentifier,user.IdUser.ToString()),
            new Claim(ClaimTypes.Name,user.Fullname)
          };
          var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
          var principal = new ClaimsPrincipal(identity); 
          await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
          return Ok();
        }
        else
        {
          return Unauthorized(new { error = "El nombre de usuario o contraseña son incorrectos" });
        }
      }
      else
      {
        return Unauthorized(new { error = "El nombre de usuario o contraseña son incorrectos" });
      }
    }
  }
}