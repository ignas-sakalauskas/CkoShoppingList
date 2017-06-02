using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CkoShoppingList.Service.Exceptions;
using CkoShoppingList.Service.Services;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CkoShoppingList.Service.Tests.Services
{
    [TestClass]
    public sealed class StorageServiceTests
    {
        private ConcurrentDictionary<string, int> _drinksDictionary;

        [TestInitialize]
        public void Init()
        {
            // Make sure we create an instance of ConcurrentDictionary with the same settings as in DI,
            // since we cannot fully mock ConcurrentDictionary due to non-virtual methods
            _drinksDictionary = new ConcurrentDictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);
        }

        [TestCleanup]
        public void Finish()
        {
            _drinksDictionary = null;
        }

        [TestMethod]
        public void Constructor_ShouldThrowArgumentNullException_WhenConcurrentDictionaryIsNull()
        {
            // Arrange
            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new StorageService(null);

            // Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void GetDrinks_ShouldReturnEmptyList_WhenDictionaryIsEmpty()
        {
            // Arrange
            var service = new StorageService(_drinksDictionary);

            // Act
            var result = service.GetDrinks();

            // Assert
            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetDrinks_ShouldReturnTwoItems_WhenDictionaryHasTwoItems()
        {
            // Arrange
            _drinksDictionary.TryAdd("test1", 1);
            _drinksDictionary.TryAdd("test2", 2);
            var service = new StorageService(_drinksDictionary);

            // Act
            var result = service.GetDrinks();

            // Assert
            result.Should().HaveCount(2);
        }

        [TestMethod]
        public void GetDrink_ShouldThrowItemNotFoundException_WhenKeyNotFound()
        {
            // Arrange
            const string name = "notfound";
            var service = new StorageService(_drinksDictionary);

            // Act
            Action action = () => service.GetDrink(name);

            // Assert
            action.ShouldThrow<ItemNotFoundException>();
        }

        [TestMethod]
        public void GetDrink_ShouldReturnDrink_WhenNameMatchesAndGetsValue()
        {
            // Arrange
            const string name = "name";
            const int value = 2;
            _drinksDictionary.TryAdd("test1", 1);
            _drinksDictionary.TryAdd(name, value);
            var service = new StorageService(_drinksDictionary);

            // Act
            var result = service.GetDrink(name);

            // Assert
            result.Key.Should().Be(name);
            result.Value.Should().Be(value);
        }

        [TestMethod]
        public void AddDrink_ShouldThrowDuplicateItemException_WhenDuplicatedKeyDetected()
        {
            // Arrange
            const string name = "name";
            _drinksDictionary.TryAdd(name, 0);
            var service = new StorageService(_drinksDictionary);

            // Act
            Action action = () => service.AddDrink(new KeyValuePair<string, int>(name, 0));

            // Assert
            action.ShouldThrow<DuplicateItemException>();
        }

        [TestMethod]
        public void AddDrink_ShouldReturnAddedDrink_WhenKeyIsNotPresent()
        {
            // Arrange
            const string name = "name";
            const int value = 2;
            var service = new StorageService(_drinksDictionary);

            // Act
            var result = service.AddDrink(new KeyValuePair<string, int>(name, value));

            // Assert
            result.Key.Should().Be(name);
            result.Value.Should().Be(value);
        }

        [TestMethod]
        public void UpdateDrink_ShouldThrowItemNotFoundException_WhenKeyNotFound()
        {
            // Arrange
            const string name = "notfound";
            var service = new StorageService(_drinksDictionary);

            // Act
            Action action = () => service.UpdateDrink(name, new KeyValuePair<string, int>());

            // Assert
            action.ShouldThrow<ItemNotFoundException>();
        }

        [TestMethod]
        public void UpdateDrink_ShouldReturnUpdatedDrink_WhenKeyIsNotPresentInDictionary()
        {
            // Arrange
            const string name = "name";
            const int value = 2;
            _drinksDictionary.TryAdd(name, 1);
            var service = new StorageService(_drinksDictionary);

            // Act
            var result = service.UpdateDrink(name, new KeyValuePair<string, int>("anyname", value));

            // Assert
            result.Key.Should().Be(name);
            result.Value.Should().Be(value);
        }

        [TestMethod]
        public void DeleteDrink_ShouldThrowItemNotFoundException_WhenKeyNotFound()
        {
            // Arrange
            const string name = "notfound";
            var service = new StorageService(_drinksDictionary);

            // Act
            Action action = () => service.DeleteDrink(name);

            // Assert
            action.ShouldThrow<ItemNotFoundException>();
        }

        [TestMethod]
        public void DeleteDrink_ShouldMakeDictionaryEmpty_WhenTheSingleItemDeleted()
        {
            // Arrange
            const string name = "name";
            _drinksDictionary.TryAdd(name, 0);
            var service = new StorageService(_drinksDictionary);

            // Act
            service.DeleteDrink(name);

            // Assert
            _drinksDictionary.Should().BeEmpty();
        }
    }
}
