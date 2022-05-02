using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using DigitalQueue.Web.Areas.Accounts.Dtos;
using DigitalQueue.Web.Data.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DigitalQueue.Web.Infrastructure;

public sealed class JwtTokenService
{
    private readonly JwtRefreshTokenProvider _jwtRefreshTokenProvider;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<JwtTokenService> _logger;
    private readonly JwtOptions _jwtTokenOptions;

    public JwtTokenService(
        IOptions<JwtOptions> jwtTokenOptions,
        JwtRefreshTokenProvider jwtRefreshTokenProvider,
        UserManager<User> userManager,
        ILogger<JwtTokenService> logger)
    {
        _jwtRefreshTokenProvider = jwtRefreshTokenProvider;
        _userManager = userManager;
        _logger = logger;
        _jwtTokenOptions = jwtTokenOptions.Value;
    }

    public async Task<TokenResult> GenerateToken(IEnumerable<Claim> claims, User user)
    {
        if (claims == null)
        {
            throw new ArgumentNullException(nameof(claims));
        }

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtTokenOptions.Secret!));
        SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: this._jwtTokenOptions.Issuer,
            audience: this._jwtTokenOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(this._jwtTokenOptions.TokenLifeTime),
            signingCredentials: signingCredentials
        );

        var refreshToken = await this._jwtRefreshTokenProvider.GenerateAsync(
            JwtRefreshTokenProvider.Purpose, 
            _userManager, 
            user);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new TokenResult(accessToken, refreshToken);
    }

    public async Task<TokenResult?> RefreshToken(string refreshToken, User user, IEnumerable<Claim> claims)
    {
        var isValid = await this._jwtRefreshTokenProvider.ValidateAsync(
            JwtRefreshTokenProvider.Purpose, 
            refreshToken,
            _userManager, 
            user);

        if (!isValid)
        {
            _logger.LogWarning("Invalid refresh token from user {Id}", user.Id);
            return null;
        }

        return await this.GenerateToken(claims, user);
    }
}
