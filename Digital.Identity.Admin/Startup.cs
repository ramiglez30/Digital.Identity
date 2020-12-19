using Digital.Identity.Admin.Data;
using Digital.Identity.Admin.Models;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Digital.Identity.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Get MySQl Server Version.
            var serverVersion = ServerVersion.AutoDetect(Configuration.GetConnectionString("MySqlConnection"));

            services.AddControllers();

            services.AddDbContext<AdminDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MySqlConnection"), serverVersion);
            }

            );

            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = false)
                .AddEntityFrameworkStores<AdminDbContext>();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddConfigurationDbContext(options =>
                    options.ConfigureDbContext = opt => opt.UseMySql(Configuration.GetConnectionString("MySqlConnection"), serverVersion,
                    sql => sql.MigrationsAssembly(migrationsAssembly))
            );

            services.AddOperationalDbContext(options =>
                    options.ConfigureDbContext = opt => opt.UseMySql(Configuration.GetConnectionString("MySqlConnection"), serverVersion,
                    sql => sql.MigrationsAssembly(migrationsAssembly))
            );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(name: "v1", info: new OpenApiInfo { Title = "My Identity Server 4 Admin API V1", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Identity Server 4 Admin API V1");
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
