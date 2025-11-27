using Microsoft.AspNetCore.Mvc;
using Moq;
using Spacehub.Application.Repository;
using SpaceHub.Application.Controllers;
using SpaceHub.Application.Dtos;

namespace Spacehub.Tests;

public class AstronomicalObjectCotnrollerTest
{
  [Fact]
  public async Task GetAstronomicalObejcts_retunrOk()
  {
    // Averrage
    List<AstronomicalObjectDto> objectsLists = [];
    
    var astronomicalObjects = new AstronomicalObjectDto
    {
      Category = "Fenomenos astronomicos",
      ImagePath = "/images/objects/3i-atlas.png",
      Title = "El cometa 3i-atlas ya es visible desde la tierra"
    };
    objectsLists.Add(astronomicalObjects);
    
    var astronomicalObejctRepositoryMock = new Mock<IAstronomicalObjectRepository>();
    astronomicalObejctRepositoryMock
    .Setup(a => a.GetAstronomicalObjectList(0))
    .ReturnsAsync(objectsLists);

    var astronomicalObjectController =  new AstronomicalObjectController(astronomicalObejctRepositoryMock.Object);

    // Act
    var request = await astronomicalObjectController.ApiAstronomicalObject(0);
    var okResult = Assert.IsType<OkObjectResult>(request);
    var retunrObject = Assert.IsType<List<AstronomicalObjectDto>>(okResult.Value);

    Assert.Equal(200, okResult.StatusCode);
    Assert.Equal(objectsLists, retunrObject);
  }

  [Fact]
  public async Task GetSuggestion_retunrOk()
  {
    // Averrage
    List<AstronomicalObjectDto> suggestionList = [];
    var suggestion = new AstronomicalObjectDto
    {
      Category = "Fenomenos astronomicos",
      ImagePath = "/images/objects/3i-atlas.png",
      Title = "El cometa 3i-atlas ya es visible desde la tierra"
    };
    suggestionList.Add(suggestion);

    var astronomicalObejctRepositoryMock = new Mock<IAstronomicalObjectRepository>();
    astronomicalObejctRepositoryMock
    .Setup(astronomicalObject => astronomicalObject.GetAstronomicalObjectSuggestion(1))
    .ReturnsAsync(suggestionList);

    var astronomicalObjectController = new AstronomicalObjectController(astronomicalObejctRepositoryMock.Object);

    // Act
    var request = await astronomicalObjectController.ApiAstronomicalObjectsSuggestion(1);
    var okResult = Assert.IsType<OkObjectResult>(request);
    var returnObject = Assert.IsType<List<AstronomicalObjectDto>>(okResult.Value);

    Assert.Equal(200, okResult.StatusCode);
    Assert.Equal(suggestionList, returnObject);
  }
}