using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCars.ServerConfigs
{
    public class IdentityServerConfig
    {
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("CarAPI", "My Cars API")
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
          new List<IdentityResource>
          {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
          };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "testclient",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("Test123$".Sha256())
                    },
                    AllowedScopes = { "CarAPI" }
                }
            };
    }
}
