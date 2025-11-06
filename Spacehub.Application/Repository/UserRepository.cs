using Microsoft.EntityFrameworkCore;
using SpaceHub.Application.Data;
using SpaceHub.Application.Dtos;
using SpaceHub.Application.Models;

namespace SpaceHub.Application.Repository;

public interface IUserRepository
{
  Task CreateUser(User user);
  Task<UserDto?> FindUser(string username);
}

public class UserRepository : IUserRepository
{
  private readonly AppDbContext _appDbContext;
  public UserRepository(AppDbContext appDbContext)
  {
    _appDbContext = appDbContext;
  }

  public async Task CreateUser(User user)
  {
    _appDbContext.Users.Add(user);
    await _appDbContext.SaveChangesAsync();
  }

  public async Task<UserDto?> FindUser(string username)
  {
    var user = await _appDbContext.Users
    .OrderBy(user => user.id_user)
    .Include(user => user.RoleFk)
    .Where(user => user.username == username)
    .Select(user => new UserDto
    {
      Password = user.user_password,
      IdUser = user.id_user,
      Role = user.RoleFk != null ? user.RoleFk.type_role : "sin role",
      Fullname = user.fullname
    })
    .FirstOrDefaultAsync();   

    return user;
  }
}