using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CBS.Data.Identity;
using CBS.Data.Context;
using CBS.Main.Services;
using CBS.Service.Interfaces;
using CBS.Data.Repository;
using CBS.Data.Repository.Interface;
using CBS.Main.Extensions;
using CBS.Common.Interface;
using CBS.Common.Services.Azure_Storage;
using CBS.Dto.ViewModel;
using CBS.Common.Services.Email;

using System.Globalization;
using System.Threading;

using Swashbuckle.AspNetCore.Swagger;


namespace CBS.Main
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
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd MMM yyyy";
            culture.DateTimeFormat.LongTimePattern = "";
            Thread.CurrentThread.CurrentCulture = culture;

            services.AddIdentity<ApplicationUser, IdentityRole>(m =>
            {
                m.Password.RequireDigit = true;
                m.Password.RequireLowercase = false;
                m.Password.RequireUppercase = false;
                m.Password.RequireNonAlphanumeric = false;
                m.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AuthConnection")));

            services.AddDbContext<CBSContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AuthConnection")));
                

            services.Configure<AzureContainerSettings>(Configuration.GetSection("AzureContainerSettings"));

            // Add application services
            services.AddTransient<IManageServices, ManageServices>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDepartmentService, DepartmentService>();
            services.AddTransient<IPlatformService, PlatformService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IApplicationsService, ApplicationsService>();

            services.AddScoped<IEmailSender>(factory =>
            {
                return new EmailSender(new EmailSettings(
                    host: Configuration["SMTP:Server"],
                    username: Configuration["SMTP:Username"],
                    password: Configuration["SMTP:Password"],
                    port: Convert.ToInt32(Configuration["SMTP:Port"])));
            });

            services.AddScoped(typeof(IRepository<>), typeof(DataRepository<>));
            services.AddTransient<IContext, CBSContext>();

            services.AddMvc();

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryIdentityResources(Config.GetIdentityResources())
                    .AddInMemoryClients(Config.GetClients())
                    .AddAspNetIdentity<ApplicationUser>()
                    .AddProfileService<ProfileService>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials()
                .Build());
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetService<CBSContext>().Database.Migrate();
            }
        }
    }
}
