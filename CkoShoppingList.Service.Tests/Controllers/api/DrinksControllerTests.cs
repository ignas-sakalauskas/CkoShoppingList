using System;
using System.Collections.Generic;
using CkoShoppingList.Service.Controllers.api;
using CkoShoppingList.Service.Exceptions;
using CkoShoppingList.Service.Models;
using CkoShoppingList.Service.Models.Responses;
using CkoShoppingList.Service.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CkoShoppingList.Service.Tests.Controllers.api
{
    [TestClass]
    public sealed class DrinksControllerTests
    {
        private Mock<IStorageService> _storageServiceMock;
        private Mock<ILoggerFactory> _loggerFactoryMock;

        [TestInitialize]
        public void Init()
        {
            _storageServiceMock = new Mock<IStorageService>(MockBehavior.Strict);
            _loggerFactoryMock = new Mock<ILoggerFactory>(MockBehavior.Strict);

            var loggerMock = new Mock<ILogger<DrinksController>>();
            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(loggerMock.Object);
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenLoggerFactoryIsNull()
        {
            // Arrange
            _loggerFactoryMock.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(null as ILogger);

            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenCreateLoggerReturnsNull()
        {
            // Arrange
            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new DrinksController(_storageServiceMock.Object, null);

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenServiceIsNull()
        {
            // Arrange
            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new DrinksController(null, _loggerFactoryMock.Object);

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void Get_ShouldReturnBadResult_WhenExceptionIsThrown()
        {
            // Arrange
            var filterOptions = new ListFilterOptions();
            _storageServiceMock.Setup(x => x.GetDrinks(It.Is<ListFilterOptions>(f => f == filterOptions)))
                .Throws(new Exception("Exception"));
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Get(filterOptions);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Exception");
            _storageServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Get_ShouldReturnOkResult_WhenServiceReturnsEmptyListOfDrinks()
        {
            // Arrange
            var filterOptions = new ListFilterOptions();
            _storageServiceMock.Setup(x => x.GetDrinks(It.Is<ListFilterOptions>(f => f == filterOptions)))
                .Returns(new List<KeyValuePair<string, int>>());

            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Get(filterOptions);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<DrinksList>();
            _storageServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Get_ShouldCountAndDataCountMatch_WhenServiceReturnsListOfDrinks()
        {
            // Arrange
            var filterOptions = new ListFilterOptions();
            var list = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>(), 
                new KeyValuePair<string, int>()
            };

            _storageServiceMock.Setup(x => x.GetDrinks(It.Is<ListFilterOptions>(f => f == filterOptions)))
                .Returns(list);

            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Get(filterOptions);
            var objectResult = (DrinksList) ((OkObjectResult) result).Value;

            // Assert
            objectResult.Data.Should().HaveCount(2);
            objectResult.Count.Should().Be(list.Count);
            _storageServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Get_ShouldServiceAndControllerResponsesReturnTheSameData_WhenServiceReturnsListOfDrinks()
        {
            // Arrange
            var filterOptions = new ListFilterOptions();
            var list = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("test1", 1), 
                new KeyValuePair<string, int>("test2", 2)
            };

            _storageServiceMock.Setup(x => x.GetDrinks(It.Is<ListFilterOptions>(f => f == filterOptions)))
                .Returns(list);

            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Get(filterOptions);
            var objectResult = (DrinksList) ((OkObjectResult) result).Value;

            // Assert
            objectResult.Data.ShouldBeEquivalentTo(list);
            _storageServiceMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void GetById_ShouldReturnBadResult_WhenNameIsInvalid(string name)
        {
            // Arrange
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Get(name);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Invalid name");
        }

        [TestMethod]
        public void GetById_ShouldReturnBadResult_WhenExceptionIsThrown()
        {
            // Arrange
            const string name = "name";
            _storageServiceMock.Setup(x => x.GetDrink(It.Is<string>(a => a == name)))
                .Throws(new Exception("Exception"));
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Get(name);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Exception");
            _storageServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetById_ShouldReturnNotFoundResult_WhenItemNotFoundExceptionIsThrown()
        {
            // Arrange
            const string name = "name";
            _storageServiceMock.Setup(x => x.GetDrink(It.Is<string>(a => a == name)))
                .Throws(new ItemNotFoundException("ItemNotFoundException"));
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Get(name);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("ItemNotFoundException");
            _storageServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetById_ShouldReturnOkResult_WhenServiceReturnsDrink()
        {
            // Arrange
            const string name = "name";
            _storageServiceMock.Setup(x => x.GetDrink(It.Is<string>(a => a == name)))
                .Returns(new KeyValuePair<string, int>(name, 0));
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Get(name);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<KeyValuePair<string, int>>()
                .Which.Key.Should().Be(name);
            _storageServiceMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void Post_ShouldReturnBadResult_WhenNameIsInvalid(string name)
        {
            // Arrange
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Post(new KeyValuePair<string, int>(name, 0));

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Invalid name");
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void Post_ShouldReturnBadResult_WhenValueIsInvalid(int value)
        {
            // Arrange
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Post(new KeyValuePair<string, int>("a", value));

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Invalid value");
        }

        [TestMethod]
        public void Post_ShouldReturnBadResult_WhenExceptionIsThrown()
        {
            // Arrange
            var drink = new KeyValuePair<string, int>("name", 1);
            _storageServiceMock.Setup(x => x.AddDrink(It.Is<KeyValuePair<string, int>>(a => a.Key == drink.Key)))
                .Throws(new Exception("Exception"));
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Post(drink);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Exception");
            _storageServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Post_ShouldReturnOkResult_WhenServiceReturnsAddedDrink()
        {
            // Arrange
            var drink = new KeyValuePair<string, int>("name", 1);
            _storageServiceMock.Setup(x => x.AddDrink(It.Is<KeyValuePair<string, int>>(a => a.Equals(drink))))
                .Returns(drink);
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Post(drink);

            // Assert
            result.Should().BeOfType<CreatedResult>()
                .Which.Value.Should().BeOfType<KeyValuePair<string, int>>()
                .Which.Should().Be(drink);
            _storageServiceMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void Put_ShouldReturnBadResult_WhenNameIsInvalid(string name)
        {
            // Arrange
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Put(name, new KeyValuePair<string, int>());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Invalid name");
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void Put_ShouldReturnBadResult_WhenValueIsInvalid(int value)
        {
            // Arrange
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Put("name", new KeyValuePair<string, int>("a", value));

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Invalid value");
        }

        [TestMethod]
        public void Put_ShouldReturnNotFoundResult_WhenItemNotFoundExceptionIsThrown()
        {
            // Arrange
            const string name = "name";
            var drink = new KeyValuePair<string, int>(name, 1);
            _storageServiceMock.Setup(x => x.UpdateDrink(It.Is<string>(a => a == name), It.Is<KeyValuePair<string, int>>(a => a.Equals(drink))))
                .Throws(new ItemNotFoundException("ItemNotFoundException"));
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Put(name, drink);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("ItemNotFoundException");
            _storageServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Put_ShouldReturnBadResult_WhenExceptionIsThrown()
        {
            // Arrange
            const string name = "name";
            var drink = new KeyValuePair<string, int>(name, 1);
            _storageServiceMock.Setup(x => x.UpdateDrink(It.Is<string>(a => a == name), It.Is<KeyValuePair<string, int>>(a => a.Equals(drink))))
                .Throws(new Exception("Exception"));
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Put(name, drink);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Exception");
            _storageServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Put_ShouldReturnOkResult_WhenServiceReturnsUpdatedDrink()
        {
            // Arrange
            const string name = "name";
            var drink = new KeyValuePair<string, int>(name, 1);
            _storageServiceMock.Setup(x => x.UpdateDrink(It.Is<string>(a => a == name), It.Is<KeyValuePair<string, int>>(a => a.Equals(drink))))
                .Returns(drink);
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Put(name, drink);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<KeyValuePair<string, int>>()
                .Which.Should().Be(drink);
            _storageServiceMock.VerifyAll();
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow(null)]
        public void Delete_ShouldReturnBadResult_WhenKeyIsInvalid(string name)
        {
            // Arrange
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Delete(name);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Invalid name");
        }

        [TestMethod]
        public void Delete_ShouldReturnNotFoundResult_WhenItemNotFoundExceptionIsThrown()
        {
            // Arrange
            const string name = "name";
            _storageServiceMock.Setup(x => x.DeleteDrink(It.Is<string>(a => a == name)))
                .Throws(new ItemNotFoundException("ItemNotFoundException"));
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Delete(name);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("ItemNotFoundException");
            _storageServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Delete_ShouldReturnBadResult_WhenExceptionIsThrown()
        {
            // Arrange
            const string name = "name";
            _storageServiceMock.Setup(x => x.DeleteDrink(It.Is<string>(a => a == name)))
                .Throws(new Exception("Exception"));
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Delete(name);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeOfType<string>()
                .Which.Should().Contain("Exception");
            _storageServiceMock.VerifyAll();
        }

        [TestMethod]
        public void Delete_ShouldReturnNoContent_WhenServiceCalled()
        {
            // Arrange
            const string name = "name";
            _storageServiceMock.Setup(x => x.DeleteDrink(It.Is<string>(a => a == name)));
            var controller = new DrinksController(_storageServiceMock.Object, _loggerFactoryMock.Object);

            // Act
            var result = controller.Delete(name);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _storageServiceMock.VerifyAll();
        }
    }
}
