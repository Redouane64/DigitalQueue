using System.Text;
using System.Text.Json.Serialization;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Filters;
using DigitalQueue.Web.Infrastructure;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using CookieOptions = DigitalQueue.Web.Infrastructure.CookieOptions;

namespace DigitalQueue.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options =>
            {
                options.Filters.Add<JsonExceptionFilter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition =
                    JsonIgnoreCondition.WhenWritingNull;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var error = new ErrorDto("One or more validation problem occurred.");

                    var actionExecutingContext = context as ActionExecutingContext;

                    if (context.ModelState.ErrorCount == 0)
                    {
                        return new BadRequestObjectResult(error)
                        {
                            ContentTypes = { "application/problem+json" },
                        };
                    }

                    var errorMessage = context.ModelState
                        .Select(m => m.Value)
                        .FirstOrDefault();

                    error.Message = errorMessage?.Errors.FirstOrDefault()?.ErrorMessage ?? error.Message;

                    return new BadRequestObjectResult(error)
                    {
                        ContentTypes = { "application/problem+json" },
                    };

                };
            });


        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Digital Queue API", Version = "v1" });
        });

        services.AddCors(options =>
        {
            options.AddPolicy("_AnyClient", 
                builder => builder.WithMethods("GET", "OPTIONS", "POST")
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .SetPreflightMaxAge(TimeSpan.FromDays(5)).Build());
        });
    }
    
    public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CookieOptions>(configuration.GetSection(nameof(CookieOptions)));
        var cookieOptions = configuration.GetSection(nameof(CookieOptions)).Get<CookieOptions>();

        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }).AddCookie(options =>
        {
            options.LoginPath = "/accounts/login";
            options.LogoutPath = "/accounts?handler=signout";
            options.AccessDeniedPath = "/accounts?handler=signout";

            options.Cookie.Name = cookieOptions.Name;
            options.Cookie.MaxAge = TimeSpan.FromMinutes(cookieOptions.ExpireTimeSpan);
            options.ExpireTimeSpan = TimeSpan.FromMinutes(cookieOptions.ExpireTimeSpan);

        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; // TODO: development only
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret!)),
                ValidAudience = jwtOptions.Audience,
                ValidIssuer = jwtOptions.Issuer,
            };
        });

        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy("Admin", policy =>
            {
                policy.RequireRole(RoleDefaults.Administrator);
                policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
            });
        });

        services.AddIdentityCore<User>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;

            options.User.RequireUniqueEmail = true;
            options.Tokens.ChangeEmailTokenProvider = StringTokenProvider.ProviderName;
            options.Tokens.PasswordResetTokenProvider = SixDigitsTokenProvider.ProviderName;
            
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        }).AddRoles<IdentityRole>()
          .AddEntityFrameworkStores<DigitalQueueContext>()
          .AddTokenProvider<JwtRefreshTokenProvider>(JwtRefreshTokenProvider.ProviderName)
          .AddTokenProvider<SixDigitsTokenProvider>(SixDigitsTokenProvider.ProviderName)
          .AddTokenProvider<StringTokenProvider>(StringTokenProvider.ProviderName);

        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.Name = StringTokenProvider.ProviderName;
            options.TokenLifespan = TimeSpan.FromHours(24);
        });
        
        // Configure JWT refresh token provider
        services.Configure<JwtRefreshTokenProvider.JwtRefreshTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(configuration.GetValue<double>("JwtOptions:RefreshTokenLifeTime"));
            options.Name = JwtRefreshTokenProvider.ProviderName;
        });

        services.AddScoped<JwtTokenService>();

    }
}
