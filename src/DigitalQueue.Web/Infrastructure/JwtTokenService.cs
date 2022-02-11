using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DigitalQueue.Web.Infrastructure;

public sealed class JwtTokenService
{
    private readonly JwtRefreshTokenProvider _jwtRefreshTokenProvider;
    private readonly UserManager<User> _userManager;
    private readonly JwtOptions _jwtTokenOptions;

    public JwtTokenService(
        IOptions<JwtOptions> jwtTokenOptions,
        JwtRefreshTokenProvider jwtRefreshTokenProvider,
        UserManager<User> userManager
    )
    {
        _jwtRefreshTokenProvider = jwtRefreshTokenProvider;
        _userManager = userManager;
        _jwtTokenOptions = jwtTokenOptions.Value;
    }

    public async Task<(string Token, string RefreshToken)> GenerateToken(
        IEnumerable<Claim> claims,
        User user)
    {
        if (claims == null)
        {
            throw new ArgumentNullException(nameof(claims));
        }

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtTokenOptions.Secret));
        SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: this._jwtTokenOptions.Issuer,
            audience: this._jwtTokenOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(this._jwtTokenOptions.TokenLifeTime),
            signingCredentials: signingCredentials
        );

        var refreshToken =
            await this._jwtRefreshTokenProvider.GenerateAsync(JwtRefreshTokenProvider.Purpose, _userManager, user);

        var stringifiedToken = new JwtSecurityTokenHandler().WriteToken(token);

        return (Token: stringifiedToken, RefreshToken: refreshToken);
    }

    public async Task<(string? Token, string? RefreshToken)> RefreshToken(string refreshToken, User user,
        IEnumerable<Claim> claims)
    {
        var isValid =
            await this._jwtRefreshTokenProvider.ValidateAsync(JwtRefreshTokenProvider.Purpose, refreshToken,
                _userManager, user);

        if (!isValid)
        {
            return (null, null);
        }

        return await this.GenerateToken(claims, user);
    }
}
