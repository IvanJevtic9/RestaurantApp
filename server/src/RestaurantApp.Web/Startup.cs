using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RestaurantApp.Infrastructure.Data;
using System.Linq;
using RestaurantApp.Core.Interface;
using RestaurantApp.Infrastructure;
using FluentValidation.AspNetCore;
using System.IO;
using Microsoft.Extensions.Logging;
using RestaurantApp.Infrastructure.Data.Repository;
using RestaurantApp.Core.RepositoryInterface;
using RestaurantApp.Core.Factory;
using Microsoft.AspNetCore.Identity;
using RestaurantApp.Core.Entity;
using FluentValidation;
using RestaurantApp.Web.WebModel;
using RestaurantApp.Web.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RestaurantApp.Core.Setting;
using RestaurantApp.Core.IdentityProvider;
using RestaurantApp.Core.Service;

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

            app.UseCors(x => x.AllowAnyMethod()
                              .AllowAnyHeader()
                              .SetIsOriginAllowed(origin => true)
                              .AllowCredentials());

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
            services.AddCors();

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

            /*Jwt token configurations*/
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.JwtIssuer,
                    ValidAudience = jwtSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.JwtKey))
                };
            });

            /*Swagger configuration*/
            var swaggerSettings = new SwaggerSettings();
            configuration.Bind(key: nameof(swaggerSettings), swaggerSettings);

            /*Appling app settings*/
            var appSettings = new AppSettings();
            configuration.Bind(key: nameof(appSettings), appSettings);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerSettings.Version, new OpenApiInfo { Title = swaggerSettings.Title, Version = swaggerSettings.Version });
            });

            services.AddSingleton(swaggerSettings);
            services.AddSingleton(jwtSettings);
            services.AddSingleton(appSettings);
        }

        private static void InitializingContainersForDependencyInjection(IServiceCollection services)
        {
            /*configuring DI*/
            services.AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));

            services.AddScoped<SeedData>();
            services.AddScoped<DynamicTypeFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
            services.AddScoped<IAccountManager<Account>, AccountManager>();

            services.AddTransient<IValidator<AccountDto>, AccountValidator>();
            services.AddTransient<IValidator<LoginDto>, LoginValidator>();
        }
    }
}
