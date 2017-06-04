# Cko Shopping List

## CkoShoppingList.Service
Simple RESTful WEB API to manage drinks in the shopping list. URL: http://localhost:5000

Features:
* Drinks list stored in memory as thread-safe ConcurrentDictionary.
* Parameters validation.
* Error handling and logging. Default logs path: C:\temp\logs
* Swagger UI: http://localhost:5000/swagger/ui
* Token based authentication (enabled for all endpoints by default)

Considerations:
* Use int/guid for drink ID and implement additional logic to keep names in the list unique. At the moment for the sake of simplicity Name is treated as an ID.

## CkoShoppingList.Service.Tests
Unit tests for the Shopping List Service.

## CkoShoppingList.Identity
Simple implementation of IdentityServer4 for Shopping List Service only. All data is stored in memory only. URL: https://localhost:44373/
Please note the Identity project uses HTTPS, hence if your `localhost` SSL certificate is not trusted, you might get SSL errors when authenticating. 
A quick guide on how to generate a SSL certificate for development: https://ignas.me/tech/self-signed-multi-domain-ssl-certificate/

## Checkout.ApiClient.Net45
Pre-existing API library with newly added logic to interact with Shopping List Service endpoints. Also tweaked logic to correctly handle Created and NoContent HTTP status codes.

## Checkout.ApiClient.Net45.Tests
Pre-existing integration tests, and the ones added for Shopping List Service endpoints.

## CkoShoppingList.Client
Very simple client application to retrieve authentication token, and call all Shopping List Service endpoints via API Client for testing purposes.

## TODO
* Enable authentication for integration tests in API Client.
* Configure authentication in Swagger UI in the Shopping List Service.
* Refactor API Client to abstract out API calls to make them unit testable, and add unit tests.

## TESTING
For end-to-end testing you need to run projects in following order:
1. CkoShoppingList.Service
2. CkoShoppingList.Identity
3. CkoShoppingList.Client

Check Client console window for HTTP responses.
