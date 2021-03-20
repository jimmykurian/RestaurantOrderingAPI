using API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static Application.Orders.Queries.GetOrder;

namespace Tests.APITests.UnitTests
{
    public class OrderControllerUnitTests
    {
        private Mock<IMediator> _mediator;

        public OrderControllerUnitTests()
        {
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public void Place_An_Order()
        {
            // Arrange
            _mediator
                .Setup(x => x.Send(It.IsAny<Query>(), new CancellationToken()))
                .ReturnsAsync(It.IsAny<string>());
            var ordersController = new OrdersController(_mediator.Object);

            // Act
            var result = ordersController.GetOrder(new Query());

            // Assert 
            Assert.IsType<Task<ActionResult<string>>>(result);
        }
    }
}
