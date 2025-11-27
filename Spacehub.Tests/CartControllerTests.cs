using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SpaceHub.Application.Controllers;
using SpaceHub.Application.Dtos;
using SpaceHub.Application.Models;
using SpaceHub.Application.Repository;

namespace Spacehub.Tests;

public class CartControllerTest
{
  [Fact]
  public async Task GetCartItems_ReturnsOk()
  {
    // Averrage
    var itemCart = new CartItemsDto
    {
      ImagePath = "/images/items/taza-espacial.png",
      ItemName = "taza edicion nebulosa azul",
    };

    List<CartItemsDto> cartList = [];
    cartList.Add(itemCart);

    var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "1") };
    var identity = new ClaimsIdentity(claims, "testauth");
    var user = new ClaimsPrincipal(identity);
    var cartRepositoryMock = new Mock<ICartRepository>();
    cartRepositoryMock
    .Setup(cart => cart.GetCartItems(1))
    .ReturnsAsync(cartList);

    var cartController = new CartController(cartRepositoryMock.Object);
    cartController.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

    // Act
    var request = await cartController.ApiCartItems();
    var okResult = Assert.IsType<OkObjectResult>(request);
    var retunrObject = Assert.IsType<List<CartItemsDto>>(okResult.Value);

    // Asert
    Assert.Equal(200, okResult.StatusCode);
    Assert.Equal(cartList, retunrObject);
  }

  [Fact]
  public async Task GetCartItems_ReturnsNotFound()
  {
    var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "1") };
    var identity = new ClaimsIdentity(claims, "testauth");
    var user = new ClaimsPrincipal(identity);
    var cartRepositoryMock = new Mock<ICartRepository>();
    cartRepositoryMock
    .Setup(cart => cart.GetCartItems(1))
    .ReturnsAsync([]);

    var cartController = new CartController(cartRepositoryMock.Object);
    cartController.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

    // Act
    var request = await cartController.ApiCartItems();
    var notFoundResult = Assert.IsType<NotFoundObjectResult>(request);

    // Asert
    Assert.Equal(404, notFoundResult.StatusCode);
  }

  [Fact]
  public async Task AddItemoToCart_RetunrsOk()
  {
    var item = new CartItemDto
    {
      IdItem = 5,
      Quantity = 1
    };

    var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "1") };
    var identity = new ClaimsIdentity(claims, "testauth");
    var user = new ClaimsPrincipal(identity);
    var cartRepositoryMock = new Mock<ICartRepository>();
    cartRepositoryMock
    .Setup(cart => cart.AddItemToCart(new Cart
    {
      id_cart = 22,
      id_item = 5
    }));

    var cartController = new CartController(cartRepositoryMock.Object);
    cartController.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

    // Act
    var request = await cartController.ApiAddItemToCart(item);
    var created = Assert.IsType<CreatedResult>(request);

    // Asert
    Assert.Equal(201, created.StatusCode);
  }

}