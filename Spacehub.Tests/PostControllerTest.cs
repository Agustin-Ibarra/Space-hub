using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SpaceHub.Application.Controllers;
using SpaceHub.Application.Dtos;
using SpaceHub.Application.Hubs;
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
      TextContent = "",
      TextDescription = "",
      Title = "Space X realizara una exploracion a Marte",
      CreatedAt = DateTime.Now
    };
    var hubContextMock = new Mock<IHubContext<NotifyHub>>();
    var postRepositoryMock = new Mock<IpostRepository>();
    postRepositoryMock
    .Setup(post => post.GetPostDetail(1))
    .ReturnsAsync(post);
    var postController = new PostController(postRepositoryMock.Object,hubContextMock.Object);

    // Act
    var request = await postController.ApiPostDetail(1);
    var okResult = Assert.IsType<OkObjectResult>(request);
    var returnObject = Assert.IsType<PostDetailDto>(okResult.Value);
    
    Assert.Equal(200,okResult.StatusCode);
    Assert.Equal(post,returnObject);
  }
}