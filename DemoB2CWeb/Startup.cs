using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoB2CWeb
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
            //services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            //});

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration.GetSection( "AzureAd"));
               // .AddMicrosoftIdentityWebApp(Configuration, "AzureAd");
            services.AddRazorPages().AddMvcOptions(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                              .RequireAuthenticatedUser()
                              .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddMicrosoftIdentityUI();

            CookiePolicyOptions cookiePolicy = new CookiePolicyOptions()
            {
                Secure = CookieSecurePolicy.Always,
            };


            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            //.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            //{
            //    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.Authority = "https://login.microsoftonline.com/ddb8b9a8-4880-4b09-a403-c8f57e08e28f";
            //    options.ClientId = "8e110aa3-349e-422d-9965-4cc43ed233d6";
            //    options.ResponseType = OpenIdConnectResponseType.Code;
            //    options.ResponseType = OpenIdConnectResponseType.IdToken;
            //    options.SaveTokens = true;
            //    options.Scope.Add("profile");
            //    options.Scope.Add("openid");
            //});

            // services.AddMicrosoftIdentityWebAppAuthentication(Configuration, "AzureAd");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
