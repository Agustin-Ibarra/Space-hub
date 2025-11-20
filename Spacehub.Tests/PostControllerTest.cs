using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using SpaceHub.Application.Controllers;
using SpaceHub.Application.Dtos;
using SpaceHub.Application.Hubs;
using SpaceHub.Application.Repository;

namespace Spacehub.Tests;

public class UnitTest1
{
    [Fact]
    public async Task GetPosts_ReturnsOk()
    {
      // Averrage
      List<PostDto> postList = [];
      var post = new PostDto
      {
        Category = "Exploraciones espacioales",
        Id = 1,
        ImagePath = "/images/posts/exploracion_a_marte.png",
        Title = "Space X realizara una exploracion a Marte"
      };
      postList.Add(post);
      var postRepositoryMock = new Mock<IpostRepository>();
      var hubContextMock = new Mock<IHubContext<NotifyHub>>();
      postRepositoryMock
      .Setup(post => post.GetPostsList(0))
      .ReturnsAsync(postList);
      var postController = new PostController(postRepositoryMock.Object, hubContextMock.Object);
      
      // Act
      var request = await postController.ApiPosts(0);
      var okResult = Assert.IsType<OkObjectResult>(request);
      var returnObject = Assert.IsType<List<PostDto>>(okResult.Value);

      Assert.Equal(200,okResult.StatusCode);
      Assert.Equal(postList,returnObject);
    }
}