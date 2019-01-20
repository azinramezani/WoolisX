using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WollisXExercise.Controllers;
using WollisXExercise.Models;
using WollisXExercise.Proxies;
using Xunit;

namespace WoolisXExercise.UnitTests
{
    public class TrolleyControllerTests
    {
        private Mock<ITrolleyProxy> _trolleyProxy;

        private TrolleyController _controller;

        public TrolleyControllerTests()
        {
            _trolleyProxy = new Mock<ITrolleyProxy>();
            var resultValue = new ProxyResponse<decimal>();
            resultValue.Content = 0.0M;

            _trolleyProxy.Setup(t => t.CalculateTrolley(It.IsAny<TrolleyCalculatorRequest>())).Returns(Task.FromResult(resultValue));
            _controller = new TrolleyController(_trolleyProxy.Object);
        }

        [Fact]
        public async void Sort_WhenCallsed_ReturnOkResult()
        {
            //Act
            var okResult = await _controller.CalculateTrolley(new TrolleyCalculatorRequest());

            //Assert
            Assert.IsType<OkObjectResult>(okResult.Result);
        }


        [Fact]
        public async void Sort_WhenRequestIsNull_ReturnBadRequest()
        {
            //Arrange
            var resultValue = new ProxyResponse<decimal>();
            resultValue.ErrorMessage = "Error!";

            _trolleyProxy.Setup(t => t.CalculateTrolley(It.IsAny<TrolleyCalculatorRequest>())).Returns(Task.FromResult(resultValue));
            _controller = new TrolleyController(_trolleyProxy.Object);

            //Act
            var result = await _controller.CalculateTrolley(null);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}
