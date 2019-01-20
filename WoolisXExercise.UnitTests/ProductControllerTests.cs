using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WollisXExercise.Controllers;
using WollisXExercise.Models;
using WollisXExercise.Proxies;
using WoolisXExercise.UnitTests.Mocks;
using Xunit;

namespace WoolisXExercise.UnitTests
{
    public class ProductControllerTests
    {
        private  IProductProxy _productProxy;

        private ProductController _controller;

        public ProductControllerTests()
        {
            _productProxy = new ProductProxyFake();
            _controller = new ProductController(_productProxy);
        }

        [Theory]
        [InlineData("Low")]
        [InlineData("High")]
        [InlineData("Ascending")]
        [InlineData("Descending")]
        [InlineData("Recommended")]
        public async void Sort_WhenCalled_ReturnOkResult(string sortOption)
        {
            //Act
            var okResult = await _controller.Sort(sortOption);

            //Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public async void Sort_WhenSortOptionNull_ReturnBadRequest()
        {
            //Act
            var result = await _controller.Sort("");

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }


        [Fact]
        public async void Sort_WhenGetProductsReturnError_ReturnBadRequest()
        {
            //Arrange
            _productProxy = new ProductProxyFake(true);
            _controller = new ProductController(_productProxy);

            //Act
            var result = await _controller.Sort("Low");

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void Sort_WhenGetShopperHistoryReturnError_ReturnBadRequest()
        {
            //Arrange
            _productProxy = new ProductProxyFake(false, true);
            _controller = new ProductController(_productProxy);

            //Act
            var result = await _controller.Sort("Recommended");

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }


    }
}
