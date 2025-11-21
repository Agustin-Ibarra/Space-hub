using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SpaceHub.Application.Controllers;
using SpaceHub.Application.Dtos;
using SpaceHub.Application.Hubs;
using SpaceHub.Application.Models;
using SpaceHub.Application.Repository;

namespace Spacehub.Tests;

public class PostControllerTest
{
  [Fact]
  public async Task GetPosts_ReturnsOk()
  {
    // Averrage
    List<PostDto> postList = [];
    var post = new PostDto
    {
      Category = "Exploraciones espaciales",
      Id = 1,
      ImagePath = "/images/posts/exploracion_a_marte.png",
      Title = "Space X realizara una exploracion a Marte"
    };
    postList.Add(post);
    var hubContextMock = new Mock<IHubContext<NotifyHub>>();
    var postRepositoryMock = new Mock<IpostRepository>();
    postRepositoryMock
    .Setup(post => post.GetPostsList(0))
    .ReturnsAsync(postList);
    var postController = new PostController(postRepositoryMock.Object, hubContextMock.Object);

    // Act
    var request = await postController.ApiPosts(0);
    var okResult = Assert.IsType<OkObjectResult>(request);
    var returnObject = Assert.IsType<List<PostDto>>(okResult.Value);

    Assert.Equal(200, okResult.StatusCode);
    Assert.Equal(postList, returnObject);
  }

  [Fact]
  public async Task GetPostDetail_ReturnOk()
  {
    // Averrage
    var post = new PostDetailDto
    {
      Category = "Exploraciones espaciales",
      Id = 1,
      ImagePath = "/images/posts/exploracion_a_marte.png",
      TextContent = "Space X realizara una exploracion a Marte ...",
      TextDescription = "Space X realizara una exploracion a Marte ...",
      Title = "Space X realizara una exploracion a Marte",
      CreatedAt = DateTime.Now
    };
    var hubContextMock = new Mock<IHubContext<NotifyHub>>();
    var postRepositoryMock = new Mock<IpostRepository>();
    postRepositoryMock
    .Setup(post => post.GetPostDetail(1))
    .ReturnsAsync(post);
    var postController = new PostController(postRepositoryMock.Object, hubContextMock.Object);

    // Act
    var request = await postController.ApiPostDetail(1);
    var okResult = Assert.IsType<OkObjectResult>(request);
    var returnObject = Assert.IsType<PostDetailDto>(okResult.Value);

    Assert.Equal(200, okResult.StatusCode);
    Assert.Equal(post, returnObject);
  }

  [Fact]
  public async Task CreatePost_ReturnOk()
  {
    var claims = new[]
    {
      new Claim(ClaimTypes.NameIdentifier,"1"),
      new Claim(ClaimTypes.Role,"editor")
    };

    var identity = new ClaimsIdentity(claims, "testauth");
    var user = new ClaimsPrincipal(identity);
    var hubContextMock = new Mock<IHubContext<NotifyHub>>();
    var clientsMock = new Mock<IHubClients>();
    var clientsProxyMock = new Mock<IClientProxy>();
    var postRepositoryMock = new Mock<IpostRepository>();

    clientsMock
    .Setup(clients => clients.All)
    .Returns(clientsProxyMock.Object);

    clientsProxyMock
    .Setup(client => client.SendCoreAsync(
        It.IsAny<string>(),
        It.IsAny<object[]>(), default)
        )
    .Returns(Task.CompletedTask);

    hubContextMock
    .Setup(context => context.Clients)
    .Returns(clientsMock.Object);

    postRepositoryMock
    .Setup(post => post.CreatePost(new Post
    {
      id_post = 1,
      path_image = "/images/posts/exploracion_a_marte.png",
      post_description = "Space X realizara una exploracion a Marte ...",
      text_content = "Space X realizara una exploracion a Marte ...",
      title = "Space X realizara una exploracion a Marte",
      created_at = DateTime.Now,
      id_category = 2
    }
    ));

    var postController = new PostController(postRepositoryMock.Object, hubContextMock.Object);

    // Act
    var postData = new PostDataDto
    {
      ImagePath = "/images/posts/exploracion_a_marte.png",
      PostDescription = "Space X realizara una exploracion a Marte ...",
      TextContent = "Space X realizara una exploracion a Marte ...",
      Title = "Space X realizara una exploracion a Marte",
      IdCategory = 1
    };
    var request = await postController.ApiCreatePost(postData);
    var okResult = Assert.IsType<CreatedResult>(request);
    var returnObject = Assert.IsType<PostDataDto>(okResult.Value);

    Assert.Equal(201, okResult.StatusCode);
    Assert.Equal(postData, returnObject);
  }
}