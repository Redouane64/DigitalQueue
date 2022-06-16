using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using DigitalQueue.Web.Data.Entities;
using DigitalQueue.Web.Infrastructure;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DigitalQueue.Web.Areas.Accounts.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtRefreshTokenProvider _refreshTokenProvider;
    private readonly JwtOptions _jwtOptions;

    public AuthenticationService(UserManager<User> userManager,
        JwtRefreshTokenProvider refreshTokenProvider,
        IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _refreshTokenProvider = refreshTokenProvider;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<string?> CreateUserToken(ClaimsPrincipal user, string tokenPurpose)
    {
        var _user = await _userManager.GetUserAsync(user);
        return await _userManager.GenerateUserTokenAsync(_user, UserTokenProvider.ProviderName, tokenPurpose);
    }

    public string? CreateAccessToken(IEnumerable<Claim> claims)
    {
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtOptions.Secret!));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: this._jwtOptions.Issuer,
            audience: this._jwtOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(this._jwtOptions.TokenLifeTime),
            signingCredentials: signingCredentials
        );
        
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return accessToken;
    }

    public async Task<string?> CreateRefreshToken(ClaimsPrincipal user)
    {
        var _user = await _userManager.GetUserAsync(user);
        var refreshToken = await this._refreshTokenProvider.GenerateAsync(
            JwtRefreshTokenProvider.Purpose,
            _userManager,
            _user);

        return refreshToken;
    }

    public async Task<bool> ValidateRefreshToken(string token, ClaimsPrincipal user)
    {
        var _user = await _userManager.GetUserAsync(user);
        return await this._refreshTokenProvider.ValidateAsync(
            JwtRefreshTokenProvider.Purpose,
            token,
            _userManager,
            _user);
    }
}
