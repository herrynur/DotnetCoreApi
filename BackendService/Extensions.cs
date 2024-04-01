using BackendService.Application.Cities.Service;
using BackendService.Application.Common.Settings;
using BackendService.Application.Companies.Service;
using BackendService.Application.Login.Service;
using BackendService.Application.MsUsers.Service;
using BackendService.Application.SignUp.Service;
using BackendService.Helper.Authorization;
using BackendService.Helper.Swagger;
using BackendService.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace BackendService
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            return services;
        }
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddScoped<ICitiesService, CitiesService>();
            services.AddScoped<IMsUserService, MsUserService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ICompaniesService, CompaniesService>();
            services.AddScoped<ISignUpService, SignUpService>();

            return services;
        }
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
        public static IServiceCollection AddAplicationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtAuthSetting>(configuration.GetSection(nameof(JwtAuthSetting)));
            services.Configure<HjexServiceSetting>(configuration.GetSection(nameof(HjexServiceSetting)));

            return services;
        }
        public static IServiceCollection AddSwaggerOption(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.SchemaFilter<SwaggerSchemaExampleFilter>();
                //add security definition
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization using Bearer Scheme"
                });

                //Add security requirement
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                //add doc
                options.SwaggerDoc("v1", new() { 
                    Title = "Backend Service", 
                    Version = "v1",
                    Description = "Backend Service API",
                    Contact = new OpenApiContact
                    {
                        Name = "Backend Service",
                        Email = "herinuraliim@gmail.com"
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var validIssuers = configuration.GetSection("ValidIssuers").Get<string[]>();

            var key = Encoding.ASCII.GetBytes("b6a318da7213e3ac477abf64cb4ecda9358380d99d72983b46380214c1f7d935");
            
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "BearerOrToken";
                    options.DefaultChallengeScheme = "BearerOrToken";
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    // asymmetric
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        NameClaimType = "name"
                    };
                })
                .AddJwtBearer("Token", options =>
                {
                    // symmetric
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidIssuers = validIssuers,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero
                    };
                })
                .AddPolicyScheme("BearerOrToken", "BearerOrToken", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        string authorization = context.Request.Headers[HeaderNames.Authorization].ToString();
                        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Token "))
                        {
                            var token = authorization.Substring("Token ".Length).Trim();
                            var jwtHandler = new JwtSecurityTokenHandler();

                            var canRead = jwtHandler.CanReadToken(token);
                            var issuerIsValid = validIssuers!.Contains(jwtHandler.ReadJwtToken(token).Issuer);

                            if (canRead && issuerIsValid)
                            {
                                context.Request.Headers.Authorization = authorization.Replace("Token", "Bearer");
                                return "Token";
                            }
                        }
                        return JwtBearerDefaults.AuthenticationScheme;
                    };
                });

            return services;
        }

        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });

            services.AddSingleton<IAuthorizationPolicyProvider, RequirePermissionPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, RequirePermissionAuthorizationHandler>();

            return services;
        }
    }
}
