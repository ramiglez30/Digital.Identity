// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;

namespace Digital.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("paymentgateway.api.post", "Enviar peticiones de pago a la API PaymentGateway")
            };

        public static IEnumerable<ApiResource> GetApiResources =>
            new ApiResource[]
            {
                new ApiResource("Sodexo PaymentGateway API")
                {
                    Scopes = new string [] { "paymentgateway.api.post" }
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "sdxfront",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:9904/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:9904/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:9904/signout-callback-oidc" },
                    RequireConsent = true,
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile" }
                },
                new Client
                {
                    ClientId = "paymentbtn",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { "https://localhost:9901/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:9901/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:9901/signout-callback-oidc" },
                    RequireConsent = true,
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "paymentgateway.api.post" }
                },
            };
    }
}