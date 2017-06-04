using System.Collections.Generic;
using System.Linq;
using CkoShoppingList.Identity.Models;
using IdentityServer4.Models;

namespace CkoShoppingList.Identity
{
    public class IdentityConfig
    {
        public static IEnumerable<Client> GetClients(AppSettings appSettings)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = appSettings.ClientId,
                    ClientName = appSettings.ClientName,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret>
                    {
                        new Secret(appSettings.ClientSecretHash)
                    },
                    AllowedScopes = appSettings.Scopes.Split(',')
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources(AppSettings appSettings)
        {
            return new List<ApiResource>
            {
                new ApiResource
                {
                    Name = "Shopping List API",
                    DisplayName = "Shopping List API",
                    Scopes = appSettings.Scopes.Split(',').Select(s => new Scope(s)).ToList()
                }
            };
        }
    }
}