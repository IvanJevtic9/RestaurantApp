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
using RestaurantApp.Core.Manager;
using RestaurantApp.Core.Lib;
using AutoMapper;
using RestaurantApp.Web.ResponseSerializer;
using System.Reflection;

namespace RestaurantApp.Web
{
    public class Startup
    {
        private const string policyName = "AllowAny";
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureSettings(services, Configuration, this.GetType().Assembly);

            ConfigureDbContext(services, Configuration);

            InitializingContainersForDependencyInjection(services, Configuration, WebHostEnvironment);

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

            app.UseCors(policyName);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(swaggerSettings.JsonRoute, $"{swaggerSettings.Title} {swaggerSettings.Version}");
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

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

        private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
        {
            /*Initialize Db context*/
            services.AddDbContext<ApplicationDbContext>(b => b.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                                                              .UseLazyLoadingProxies());
        }

        private static void ConfigureSettings(IServiceCollection services, IConfiguration configuration, Assembly assembly)
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

            /*Configure cors*/
            services.AddCors(options =>
            {
                options.AddPolicy(policyName, builder =>
                {
                    builder.AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowAnyOrigin();
                });
            });

            services.AddAutoMapper(assembly.GetType().Assembly);

            /*Configure fluent validatior*/
            services.AddControllers().AddFluentValidation();

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

        private static void InitializingContainersForDependencyInjection(IServiceCollection services, IConfiguration conf, IWebHostEnvironment env)
        {
            /*configuring DI*/
            services.AddScoped<SeedData>();
            services.AddScoped<DynamicTypeFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
            services.AddScoped<IAccountManager<Account>, AccountManager>();
            services.AddScoped(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
            services.AddScoped(m =>
            {
                var unitOfWork = m.Resolve<IUnitOfWork>();
                var logger = m.Resolve<ILoggerAdapter<Image>>();
                return new ImageManager(unitOfWork, logger, env.WebRootPath, conf.GetValue("HostUrl", ""));
            });
            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                var unitOfWork = provider.Resolve<IUnitOfWork>();
                cfg.AddProfile(new Profiler(unitOfWork));
            }).CreateMapper());

            services.AddTransient<IValidator<AccountDto>, AccountValidator>();
            services.AddTransient<IValidator<ChangePasswordDto>, ChangePasswordValidator>();
            services.AddTransient<IValidator<AccountUpdateDto>, AccountUpdateValidator>();
            services.AddTransient<IValidator<RestaurantMenuDto>, RestaurantValidator>();
            services.AddTransient<IValidator<LoginDto>, LoginValidator>();
            services.AddTransient<IValidator<GalleryDto>, GalleryValidator>();
            services.AddTransient<IValidator<RestaurantMenuItemDto>, MenuItemValidator>();
        }
    }
}
