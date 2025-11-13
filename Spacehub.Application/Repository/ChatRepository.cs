using SpaceHub.Application.Data;
using SpaceHub.Application.Models;

namespace SpaceHub.Application.Repository;

public interface IChatRepository {
  public Task AddChat(ChatMessage chatMessage);
}

public class ChatRepository : IChatRepository
{
  private readonly AppDbContext _appDbContext;

  public ChatRepository(AppDbContext appDbContext){
    _appDbContext = appDbContext;
  }
  public async Task AddChat(ChatMessage chatMessage)
  {
    _appDbContext.ChatMessages.Add(chatMessage);
    await _appDbContext.SaveChangesAsync();
  }
}