using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

using Microsoft.Extensions.Configuration;
using System.IO;
using CBS.Main.Services;
using CBS.Dto.ViewModel;

namespace CBS.Main.Extensions
{
    public class Config
    {

        public static IConfigurationRoot Configuration { get; set; }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }


        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            SSOConfigurationService ssoservice = new SSOConfigurationService();
            IList<ApplicationDto> apps = new List<ApplicationDto>();

            #region old code
            //if (Configuration["Developement"] == "Local")
            //{
            //    apps = ssoservice.GetAppConfigurations(Configuration.GetConnectionString("AuthConnectionTest").ToString());
            //}
            //else if (Configuration["Developement"] == "Dev")
            //{
            //    apps = ssoservice.GetAppConfigurations(Configuration.GetConnectionString("DevAuthConnection").ToString());
            //}
            //else
            //{
            //    apps = ssoservice.GetAppConfigurations(Configuration.GetConnectionString("AuthConnection").ToString());
            //}
            #endregion

            //apps = ssoservice.GetApplications(Configuration.GetConnectionString("AuthConnection").ToString());

            List<Client> clients = new List<Client>();

            //foreach (var item in apps)
            //{
            //    var iclient = new Client
            //    {
            //        ClientId = item.ClientID,
            //        ClientName = item.ClientName,
            //        AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

            //        RequireConsent = false,

            //        ClientSecrets =
            //                        {
            //                            new Secret(item.Sha256.Sha256())
            //                        },

            //        RedirectUris = { item.RedirectURIs },
            //        PostLogoutRedirectUris = { item.PostLogoutRedirectUris },

            //        AllowedScopes =
            //                        {
            //                            IdentityServerConstants.StandardScopes.OpenId,
            //                            IdentityServerConstants.StandardScopes.Profile,
            //                            IdentityServerConstants.ProfileDataCallers.ClaimsProviderIdentityToken,
            //                            "roles",
            //                            "orgid"
            //                        },
            //        AllowOfflineAccess = true,
            //        AlwaysSendClientClaims = true,
            //        AllowedCorsOrigins = { item.CorsOrigins }
            //    };

            //    clients.Add(iclient);

            //};

            return clients;
        }

    }
}
