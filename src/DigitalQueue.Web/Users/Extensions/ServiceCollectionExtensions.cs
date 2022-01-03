using System.Text;

using DigitalQueue.Web.Data;
using DigitalQueue.Web.Domain;
using DigitalQueue.Web.Users.JWT;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DigitalQueue.Web.Users.Extensions;

public static class ServiceCollectionExtensions
{
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
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";

            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        }).AddRoles<IdentityRole>()
          .AddEntityFrameworkStores<DigitalQueueContext>()
          .AddTokenProvider<JwtRefreshTokenProvider>(JwtRefreshTokenProvider.ProviderName);

        services.AddScoped<UsersService>();

        // Configure JWT refresh token provider
        services.Configure<JwtRefreshTokenProvider.JwtRefreshTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromMinutes(configuration.GetValue<double>("JwtOptions:RefreshTokenLifeTime"));
            options.Name = JwtRefreshTokenProvider.ProviderName;
        });

        services.AddScoped<JwtTokenService>();

    }
}
