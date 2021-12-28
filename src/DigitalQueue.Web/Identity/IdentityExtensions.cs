using System.Text;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

namespace DigitalQueue.Web.Identity;

public static class IdentityExtensions
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
        }).AddCookie(options =>
        {
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
                ValidAudience = jwtOptions.Audience,
                ValidIssuer = jwtOptions.Issuer,
            };
        });
    }
}
