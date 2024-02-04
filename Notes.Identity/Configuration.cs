using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Notes.Identity
{
    public static class Configuration
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("NoteWebApi", "WebApi")
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new ApiResource("NotesWebApi", "WebApi", 
                    new [] { JwtClaimTypes.Name}) 
                { 
                    Scopes = {"NotesWebApi"}
                }
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            { 
                new Client
                { 
                    ClientId = "notes-web-api",
                    ClientName = "Notes Web",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireClientSecret = false,
                    RequirePkce = true,
                    RedirectUris = 
                    { 
                        "http://.../signin-oidc"
                    },
                    AllowedCorsOrigins =
                    { 
                        "http://..."
                    },
                    PostLogoutRedirectUris = 
                    {
                        "http://.../signout-oidc"
                    },
                    AllowedScopes = 
                    { 
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "NotesWebApi"
                    },
                    AllowAccessTokensViaBrowser = true,
                }
            };
    }
}
