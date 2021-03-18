using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceSystemNRDCL.Data;
using ServiceSystemNRDCL.Helpers;
using ServiceSystemNRDCL.Models;
using ServiceSystemNRDCL.Repository;
using ServiceSystemNRDCL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceSystemNRDCL
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
            services.AddControllersWithViews();

            services.AddDbContext<CustomerContext>(options=>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            //adding the identity services
            services.AddIdentity<ApplicationUser, IdentityRole>().
                AddEntityFrameworkStores<CustomerContext>().AddDefaultTokenProviders();

            //redirect to page for user to log in
            services.ConfigureApplicationCookie(Config=> {
                Config.LoginPath = "/Home/LogIn/";
            });

            services.AddRazorPages().AddRazorRuntimeCompilation();

            //configuring identity options;
            services.Configure<IdentityOptions>(options=> {
                options.User.RequireUniqueEmail =true;
                options.SignIn.RequireConfirmedEmail =true;
            });

            //adding account repository services
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddRouting();

            //adding custom userclaim principal
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>,ApplicationUserClaims>();

            //adding Ihttpcontext accessor
            services.AddScoped<IUserService,UserService>();

            //adding SMTPConfig model
            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));

            //adding email service
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISiteRepository, SiteRepository>();
            services.AddScoped<IDepositRepository, DepositRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=LogIn}/{id?}");
                 
            });
        }
    }
}
