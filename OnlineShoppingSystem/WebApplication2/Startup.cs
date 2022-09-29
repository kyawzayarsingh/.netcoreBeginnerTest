using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OnlineShoppingSystem.Data;
using OnlineShoppingSystem.Repositories;
using OnlineShoppingSystem.Interfaces;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Newtonsoft.Json.Serialization;

namespace OnlineShoppingSystem
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
            services.AddControllers();
            services.AddMvc();
            services.AddSwaggerGen();
            //services.AddSwaggerGen(c=>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1", Description = "ASP" });
            //});
            services.AddDbContext<FO_DBContext>(options => options.UseSqlServer("Server=.;Database=FoodOrderingDB; Integrated Security=True"));
            //when you save (Ctrl + S ), the view will work without rerunning the program.
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
            //    options =>
            //    {
            //        options.LoginPath = "/Login";
            //        options.AccessDeniedPath = "/denied";
            //    });
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //for AddOpenIdConnect
                options.DefaultChallengeScheme = "GoogleOpenID";
                //for AddGoogle
                //options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            }).AddCookie(
                options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/denied";
                })
            .AddOpenIdConnect("GoogleOpenID", options =>
            {
                options.Authority = "https://accounts.google.com";
                options.ClientId = "666590682493-lhcaokp6nnc80ck9b5oecb7vb7rvhk78.apps.googleusercontent.com";
                options.ClientSecret = "_3V0TlJtr0Z3e7Y2M0Ldr13K";
                options.CallbackPath = "/auth";
                options.SaveTokens = true;
                options.Events = new OpenIdConnectEvents()
                {
                    OnTokenValidated = async context =>
                    {
                        var username = context.Principal.Claims.Skip(5).Take(1).First().Value;

                        if (context.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value == "117849283989223648086")
                        {
                            //var claim = new Claim(ClaimTypes.Role, "Admin");
                            //var claimIdentity = context.Principal.Identity as ClaimsIdentity;
                            //claimIdentity.AddClaim(claim);

                            var claims = new List<Claim>();
                            claims.Add(new Claim(ClaimTypes.Name, username));
                            claims.Add(new Claim(ClaimTypes.Role, "Admin"));

                            var claimIdentity = context.Principal.Identity as ClaimsIdentity;

                            claimIdentity.AddClaims(claims);
                        }
                        else
                        {
                            var claims = new List<Claim>();
                            claims.Add(new Claim(ClaimTypes.Name, username));
                            claims.Add(new Claim(ClaimTypes.Role, "User"));

                            var claimIdentity = context.Principal.Identity as ClaimsIdentity;

                            claimIdentity.AddClaims(claims);
                        }
                    }
                };
            });
            //.AddGoogle(options =>
            //{
            //    options.ClientId = "666590682493-lhcaokp6nnc80ck9b5oecb7vb7rvhk78.apps.googleusercontent.com";
            //    options.ClientSecret = "_3V0TlJtr0Z3e7Y2M0Ldr13K";
            //    options.CallbackPath = "/auth";
            //    options.AuthorizationEndpoint += "?prompt=consent";
            //});

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                //c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
