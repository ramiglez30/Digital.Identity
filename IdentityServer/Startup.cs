// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Digital.Identity.Data;
using Digital.Identity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Digital.Identity
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                //options.EmitStaticAudienceClaim = true;
            }).AddAspNetIdentity<ApplicationUser>()
               .AddConfigurationStore(options =>
               {
                   options.ConfigureDbContext = b => b.UseSqlite(Configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsAssembly(migrationsAssembly));
               }).AddOperationalStore(options =>
               {
                   options.ConfigureDbContext = b => b.UseSqlite(Configuration.GetConnectionString("DefaultConnection"),
                       sql => sql.MigrationsAssembly(migrationsAssembly));
               });

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            //services.AddAuthentication()
            //    .AddGoogle(options =>
            //    {
            //        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    
            //        // register your IdentityServer with Google at https://console.developers.google.com
            //        // enable the Google+ API
            //        // set the redirect URI to https://localhost:5001/signin-google
            //        options.ClientId = "copy client ID from Google here";
            //        options.ClientSecret = "copy client secret from Google here";
            //    });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                InitializeDatabase(app);
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients)
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.IdentityResources.Any())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiScopes.Any())
                {
                    foreach (var scope in Config.ApiScopes)
                    {
                        context.ApiScopes.Add(scope.ToEntity());
                    }
                    context.SaveChanges();
                }

                if (!context.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResources)
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
            }

            CreateUsers(app).GetAwaiter().GetResult();
        }

        private static async Task CreateUsers(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var bob = await userManager.FindByNameAsync("bob");
                if (bob == null)
                {
                    bob = new ApplicationUser { UserName = "bob", Email = "bob@gmail.com", NormalizedUserName = "BOB" };

                    await userManager.CreateAsync(bob, "Pass123$");
                    await userManager.AddClaimAsync(bob, new Claim("name", "Bob Smith"));
                    await userManager.AddClaimAsync(bob, new Claim("family_name", "Smith"));
                    await userManager.AddClaimAsync(bob, new Claim("given_name", "Bob"));
                    await userManager.AddClaimAsync(bob, new Claim("location", "Bogota"));
                    await userManager.AddClaimAsync(bob, new Claim("website", "www.bob.com"));
                }

                var alice = await userManager.FindByNameAsync("alice");
                if (alice == null)
                {
                    alice = new ApplicationUser { UserName = "alice", Email = "alice@gmail.com", NormalizedUserName = "ALICE" };

                    await userManager.CreateAsync(alice, "Pass123$");
                    await userManager.AddClaimAsync(alice, new Claim("name", "Alice Smith"));
                    await userManager.AddClaimAsync(alice, new Claim("family_name", "Smith"));
                    await userManager.AddClaimAsync(alice, new Claim("given_name", "Alice"));
                    await userManager.AddClaimAsync(alice, new Claim("location", "Bogota"));
                    await userManager.AddClaimAsync(alice, new Claim("website", "www.alice.com"));
                }

                var ana = await userManager.FindByNameAsync("ana");
                if (ana == null)
                {
                    ana = new ApplicationUser { UserName = "ana", Email = "ana@gmail.com", NormalizedUserName = "ANA" };

                    await userManager.CreateAsync(ana, "Pass123$");
                    await userManager.AddClaimAsync(ana, new Claim("name", "Ana Perez"));
                    await userManager.AddClaimAsync(ana, new Claim("family_name", "Perez"));
                    await userManager.AddClaimAsync(ana, new Claim("given_name", "Ana"));
                    await userManager.AddClaimAsync(ana, new Claim("location", "Bogota"));
                    await userManager.AddClaimAsync(ana, new Claim("website", "www.ana.com"));
                }
            }
        }
    }
}