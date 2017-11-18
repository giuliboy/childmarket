using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using KinderArtikelBoerse.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Market.WebAPI.Utils;
using Swashbuckle.AspNetCore.Swagger;

namespace Market.WebAPI
{
    public class Startup
    {
        public Startup( IHostingEnvironment env )
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath( env.ContentRootPath )
                .AddJsonFile( "appsettings.json", optional: false, reloadOnChange: true )
                .AddJsonFile( $"appsettings.{env.EnvironmentName}.json", optional: true )
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            _environment = env;
        }

        private IConfigurationRoot Configuration { get; }
        private readonly IHostingEnvironment _environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton<IConfiguration>( Configuration );

            //identity datenbank + restliches coffer zeug
            services.AddDbContext<MarketDbContext>( options =>
            {
                //https://azure.microsoft.com/en-us/blog/windows-azure-web-sites-how-application-strings-and-connection-strings-work/
                var connectionString = _environment.IsDevelopment() ? "LocalDbConnection" : "TODO";
                options.UseSqlServer( Configuration.GetConnectionString( connectionString ) );
            } );

            //identity verhalten
            services.AddIdentity<Seller, IdentityRole>( options => {
                //password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                //add interception : 
                //wenn jemand auf /api zugreift und nicht authorisiert ist
                //soll nicht "redirected" werden, sondern stattdessen ein 401 zurückgegeben werden
                //options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                //{
                //    OnRedirectToLogin = ctx =>
                //    {
                //        if ( ctx.Request.Path.StartsWithSegments( "/api" ) )
                //        {
                //            ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //        }
                //        else
                //        {
                //            ctx.Response.Redirect( ctx.RedirectUri );
                //        }
                //        return Task.FromResult( 0 );
                //    }
                //};
            } )
                .AddEntityFrameworkStores<MarketDbContext>()
                .AddDefaultTokenProviders();

            //swashbuckle hat services die in den DI Container rein müssen
            services.AddSwaggerGen( c =>
            {
                c.SwaggerDoc( "v1", new Info { Title = "My API", Version = "v1" } );
            } );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            int sslPort = 0;
            if ( env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();

                var builder = new ConfigurationBuilder()
                    .SetBasePath( env.ContentRootPath )
                    .AddJsonFile( @"Properties/launchSettings.json", optional: false, reloadOnChange: true );
                var launchConfig = builder.Build();
                sslPort = launchConfig.GetValue<int>( "iisSettings:iisExpress:sslPort" );

            }
            else
            {
                app.UseExceptionHandler( "/Home/Error" );
            }

            //enforce https 
            app.UseHttpsEnforcement( sslPort );

            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseMvc( routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}" );
            } );

            //middleware configuration für swagger und UI testpage
            app.UseSwagger();
            app.UseSwaggerUI( c =>
            {
                c.SwaggerEndpoint( "/swagger/v1/swagger.json", "market api v1" );
            } );

            //var logger = loggerFactory.CreateLogger( "startup logger" );
            //try
            //{
            //    //logger.LogInformation( "starting migration" );

            //    using ( var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope() )
            //    {
            //        var context = serviceScope.ServiceProvider.GetService<MarketDbContext>();
            //        context.Database.Migrate();
            //        if ( context.EnsureSeed() )
            //        {
            //            logger.LogError( "database seeded" );
            //        }
            //    }
            //}
            //catch ( System.Exception ex )
            //{
            //    logger.LogError( "migration or seeding failed {0}", ex );
            //    throw;
            //}
        }
    }
}
