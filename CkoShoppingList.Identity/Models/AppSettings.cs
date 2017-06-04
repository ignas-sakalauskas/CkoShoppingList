using System;

namespace CkoShoppingList.Identity.Models
{
    public class AppSettings
    {
        public Uri AuthorityUri { get; set; }
        public string Scopes { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientSecretHash { get; set; }
    }
}
