using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RestaurantApp.Web.Settings;
using RestaurantApp.Infrastructure.Data;
using System.Linq;
using RestaurantApp.Core.Interface;
using RestaurantApp.Infrastructure;
using FluentValidation.AspNetCore;
using System.IO;
using Microsoft.Extensions.Logging;

namespace RestaurantApp.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            AppConfiguration(services);

            ConfigureSettings(services, Configuration);
            ConfigureDbContext(services, Configuration);

            InitializingContainersForDependencyInjection(services);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              ILoggerFactory loggerFactory,
                              ApplicationDbContext context,
                              SwaggerSettings swaggerSettings,
                              SeedData seedData)
        {
            /* Set up logger file - Maybe move this in appSettings*/
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\Log.txt");

            RunMigration(context);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(swaggerSettings.JsonRoute, $"{swaggerSettings.Title} {swaggerSettings.Version}");
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            seedData.DataSeed();
        }

        private static void RunMigration(ApplicationDbContext context)
        {
            var pendingMigrations = context.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                context.Database.Migrate();
            }
        }

        private static void AppConfiguration(IServiceCollection services)
        {
            /*Configure cors*/
            services.AddCors(o => o.AddPolicy("AllowAny", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            /*Configure fluent validatior*/
            services.AddControllers().AddFluentValidation();
        }

        private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
        {
            /*Initialize Db context*/
            services.AddDbContext<ApplicationDbContext>(b => b.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                                                              .UseLazyLoadingProxies());
        }

        private static void ConfigureSettings(IServiceCollection services, IConfiguration configuration)
        {
            /*Jwt settings configuration*/
            var jwtSettings = new JwtSettings();
            configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);

            /*Swagger configuration*/
            var swaggerSettings = new SwaggerSettings();
            configuration.Bind(key: nameof(swaggerSettings), swaggerSettings);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerSettings.Version, new OpenApiInfo { Title = swaggerSettings.Title, Version = swaggerSettings.Version });
            });

            services.AddSingleton(swaggerSettings);
            services.AddSingleton(jwtSettings);
        }

        private static void InitializingContainersForDependencyInjection(IServiceCollection services)
        {
            /*configuring DI*/
            services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

            services.AddScoped<SeedData>();
        }
    }
}
