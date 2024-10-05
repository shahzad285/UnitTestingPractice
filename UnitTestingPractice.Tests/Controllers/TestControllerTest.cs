using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.Services;
using UnitTestingPractice.Controllers;
using Xunit;

namespace UnitTestingPractice.Tests.Controllers
{
    public class TestControllerTest
    {
        //private readonly ITestService _testService;
        private readonly TestController _controller;
        private Mock<ITestService> _testService;

        public TestControllerTest()
        {
            _testService = new Mock<ITestService>();
            _controller = new TestController(_testService.Object);
        }

        [Fact]
        public async Task TestController_GetAll_ReturnsOkResult_WithListOfItems()
        {
            // Arrange          
            _testService.Setup(s => s.GetAllItemsAsync()).ReturnsAsync(new List<TestItem> {
                new TestItem { Id = 1, Name = "Test Item 1" },
                new TestItem { Id = 2, Name = "Test Item 2" },
                new TestItem { Id = 3, Name = "Test Item 3" }
            });
            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsAssignableFrom<IEnumerable<TestItem>>(okResult.Value);
            Assert.Equal(3, items.Count());
        }

        [Fact]
        public async Task TestController_GetAll_ReturnsOkResult_WithEmptyList_WhenNoItems()
        {
            // Arrange
            _testService.Setup(s => s.GetAllItemsAsync()).ReturnsAsync(new List<TestItem>
            {

            });
            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsAssignableFrom<IEnumerable<TestItem>>(okResult.Value);
            Assert.Empty(items);
        }

        [Fact]
        public async Task TestController_GetById_WhenItemFound()
        {
            // Arrange
            _testService.Setup(s => s.GetItemByIdAsync(2)).ReturnsAsync(new TestItem
            {
                Id = 2,
                Name = "Test Item 2"
            }).Verifiable();

            // Act
            var result = await _controller.GetById(2);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var item = Assert.IsAssignableFrom<TestItem>(okResult.Value);
            Assert.NotNull(item);
            _testService.Verify(x => x.GetItemByIdAsync(2), Times.Once);
        }


        [Fact]
        public async Task TestController_GetById_WhenItemNotFound()
        {
            // Arrange
            _testService.Setup(s => s.GetItemByIdAsync(4)).ReturnsAsync((TestItem)null);
            // Act
            var result = await _controller.GetById(2);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }


        [Fact]
        public async Task TestController_Create_WIthValidItem()
        {
            // Arrange
            var newItem = new TestItem { Id = 6, Name = "New Item", Quantity = 50, Email = "newitem@example.com" };

            _testService.Setup(s => s.CreateItemAsync(newItem)).ReturnsAsync(newItem);
            // Act
            var result = await _controller.Create(newItem);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task TestController_Create_WIthInvalidItem()
        {
            // Arrange
            var newItem = new TestItem { };

            _testService.Setup(s => s.CreateItemAsync(newItem)).ReturnsAsync(newItem);
            // Act
            //_controller.ControllerContext.ModelState.AddModelError("Name", "Name is required");
            _controller.ModelState.AddModelError("Name", "Name is required");
            var result = await _controller.Create(newItem);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Theory]
        [InlineData(1, "Test Item 1")]
        [InlineData(2, "Test Item 2")]
        [InlineData(3, "Test Item 3")]
        public async Task GetById_ReturnsOk_WhenItemFound(int id, string expectedName)
        {
            // Arrange
            _testService.Setup(s => s.GetItemByIdAsync(id)).ReturnsAsync(new TestItem
            {
                Id = id,
                Name = expectedName
            });

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var item = Assert.IsAssignableFrom<TestItem>(okResult.Value);
            Assert.Equal(expectedName, item.Name);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public async Task GetById_ReturnsNotFound_WhenItemNotFound(int id)
        {
            // Arrange
            _testService.Setup(s => s.GetItemByIdAsync(id)).ReturnsAsync((TestItem)null);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}
