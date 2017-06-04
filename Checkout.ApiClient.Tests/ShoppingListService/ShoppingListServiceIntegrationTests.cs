using System.Linq;
using System.Net;
using Checkout;
using Checkout.ApiServices.ShoppingList.RequestModels;
using Checkout.Helpers;
using FluentAssertions;
using NUnit.Framework;

// ReSharper disable once CheckNamespace
namespace Tests
{
    // Run integration tests on explicit run only, since tests depend on network connection
    [Explicit]
    [TestFixture(Category = "ShoppingListApi")]
    public sealed class ShoppingListServiceIntegrationTests
    {
        private APIClient _checkoutClient;

        [SetUp]
        public void Init()
        {
            _checkoutClient = new APIClient("123", Environment.ShoppingListTest);
        }

        [Test]
        public void CreateDrink_ShouldReturnCreatedDrink()
        {
            // Arrange
            var requestModel = TestHelper.GetDrinkWithRandomNameCreateModel();

            // Act
            var response = _checkoutClient.ShoppingListService.CreateDrink(requestModel);

            // Assert
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.Created);
            response.Model.ShouldBeEquivalentTo(requestModel);
        }

        [Test]
        public void GetDrinkById_ShouldGetDrink()
        {
            // Arrange
            var requestModel = TestHelper.GetDrinkWithRandomNameCreateModel();
            var createResponse = _checkoutClient.ShoppingListService.CreateDrink(requestModel);

            // Act
            var response = _checkoutClient.ShoppingListService.GetDrink(createResponse.Model.Name);

            // Assert
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.ShouldBeEquivalentTo(createResponse.Model);
        }

        [Test]
        public void GetDrinkList_ShouldGetAllDrinks()
        {
            // Arrange
            var request = new DrinkGetList();
            var requestModel1 = TestHelper.GetDrinkWithRandomNameCreateModel();
            var response1 = _checkoutClient.ShoppingListService.CreateDrink(requestModel1);
            var requestModel2 = TestHelper.GetDrinkWithRandomNameCreateModel();
            var response2 = _checkoutClient.ShoppingListService.CreateDrink(requestModel2);

            // Act
            var response = _checkoutClient.ShoppingListService.GetDrinkList(request);

            // Assert
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.Count.Should().BeGreaterOrEqualTo(2);
            response.Model.Data.Should().Contain(d => d.Name == response1.Model.Name);
            response.Model.Data.Should().Contain(d => d.Name == response2.Model.Name);
        }

        [Test]
        public void GetDrinkList_ShouldGetDifferentResultsInSubsequentCalls_WhenCountAndOffsetSet()
        {
            // Arrange
            var request1 = new DrinkGetList
            {
                Count = 1,
                Offset = 1
            };

            var request2 = new DrinkGetList
            {
                Count = 1,
                Offset = 2
            };

            var requestModel1 = TestHelper.GetDrinkWithRandomNameCreateModel();
            _checkoutClient.ShoppingListService.CreateDrink(requestModel1);
            var requestModel2 = TestHelper.GetDrinkWithRandomNameCreateModel();
            _checkoutClient.ShoppingListService.CreateDrink(requestModel2);

            // Act
            var response1 = _checkoutClient.ShoppingListService.GetDrinkList(request1);
            var response2 = _checkoutClient.ShoppingListService.GetDrinkList(request2);

            // Assert
            response1.Should().NotBeNull();
            response2.Should().NotBeNull();
            response1.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response2.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response2.Model.Data.First().Name.Should().NotBe(response1.Model.Data.First().Name);
        }

        [Test]
        public void UpdateDrink_ShouldUpdateQuantity()
        {
            // Arrange
            var requestModel = TestHelper.GetDrinkWithRandomNameCreateModel();
            var createResponse = _checkoutClient.ShoppingListService.CreateDrink(requestModel);
            var request = new DrinkUpdate
            {
                Name = createResponse.Model.Name,
                Quantity = 777
            };

            // Act
            var response = _checkoutClient.ShoppingListService.UpdateDrink(request.Name, request);

            // Assert
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.OK);
            response.Model.ShouldBeEquivalentTo(request);
        }

        [Test]
        public void DeleteDrink_ShouldDeleteTheDrink()
        {
            // Arrange
            var requestModel1 = TestHelper.GetDrinkWithRandomNameCreateModel();
            var response1 = _checkoutClient.ShoppingListService.CreateDrink(requestModel1);

            // Act
            var response = _checkoutClient.ShoppingListService.DeleteDrink(response1.Model.Name);

            // Assert
            response.Should().NotBeNull();
            response.HttpStatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
