using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCars.ServerConfigs
{
    public class IdentityServerConfig
    {
        public static IEnumerable<ApiResource> Apis =>
            new List<ApiResource>
            {
                new ApiResource("CarAPI", "My Cars API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "testclient",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("Test123$".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "CarAPI" }
                }
            };
    }
}
