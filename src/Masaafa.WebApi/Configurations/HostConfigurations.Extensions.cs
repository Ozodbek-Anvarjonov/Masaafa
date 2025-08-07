using FluentValidation;
using Masaafa.Application.Common.Abstractions;
using Masaafa.Application.Settings;
using Masaafa.Persistence.UnitOfWork.Interfaces;
using Masaafa.WebApi.ExceptionHandlers;
using Masaafa.WebApi.Mappers;
using Masaafa.WebApi.Routing;
using Masaafa.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Telegram.Bot;

namespace Masaafa.WebApi.Configurations;

public static partial class HostConfigurations
{
    public static void AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options =>
            options.Conventions.Add(new RouteTokenTransformerConvention(new RouteTransformer()))
            );
        services.AddEndpointsApiExplorer();
        services.AddAuthorization();
        services.AddHttpContextAccessor();
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddSwagger();
        services.AddExceptionHandler();
        services.AddAutoMapper(typeof(InventoriesMappingProfile));
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddSecurity(configuration);
        services.AddServices(configuration);
        services.AddMiddlewares(configuration);
        services.AddTelegramBot(configuration);
    }

    private static void AddExceptionHandler(this IServiceCollection services)
    {
        services.AddProblemDetails();

        services.AddExceptionHandler<AppExceptionHandler>();
        services.AddExceptionHandler<InternalServerExceptionHandler>();
    }


    private static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on the textbox below!",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme,
                }
            };

            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() },
            });
        });
    }

    private static void AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SystemSettings>(configuration.GetSection(nameof(SystemSettings)));
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
            ?? throw new InvalidOperationException($"{nameof(JwtSettings)} is not configurated.");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtSettings.ValidateIssuer,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidAudience = jwtSettings.ValidAudience,
                    ValidateAudience = jwtSettings.ValidateAudience,
                    ValidateLifetime = jwtSettings.ValidateLifeTime,
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });
    }

    private static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserContext, HttpUserContext>();
        services.AddScoped<IHeaderService, HeaderService>();
    }

    private static void AddMiddlewares(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });
    }

    private static void AddTelegramBot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(configuration["Telegram:BotToken"]!));
        services.AddHostedService<TelegramPollingService>();
        services.AddMemoryCache();
    }
}
