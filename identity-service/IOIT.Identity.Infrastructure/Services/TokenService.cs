using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IOIT.Identity.Application.Common;
using IOIT.Identity.Application.Common.Exceptions;
using IOIT.Identity.Application.Common.Interfaces.Token;
using IOIT.Identity.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IOIT.Identity.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptionsSnapshot<JwtConfigOptions> _options;
        public TokenService(IOptionsSnapshot<JwtConfigOptions> options)
        {
            _options = options;
        }

        public ResTokenGeneration GenerateAccessToken(User user, IEnumerable<Claim> extraClaims)
        {
            var identity = GetClaimsIdentity(user);

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("PDv7DrqznYL6nv7DrqzjnQYO9JxIsWdcjnQYL6nu0f"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expiredDate = DateTime.UtcNow.AddMinutes(1440);
            var claims = identity.Claims.ToList();

            if (extraClaims != null)
            {
                claims.AddRange(extraClaims);
            }

            var tokeOptions = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: claims,
                expires: expiredDate,
                signingCredentials: signinCredentials,
                notBefore: DateTime.UtcNow
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            var resToken = new ResTokenGeneration(tokenString, expiredDate);

            return resToken;
        }

        private static ClaimsIdentity GetClaimsIdentity(User user)
        {
            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("fullName", $"{user.FullName}"),
                new Claim("code", user.Code),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow.ToUniversalTime()).ToString(), ClaimValueTypes.Integer64)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Token");

            return claimsIdentity;
        }

        public ResTokenGeneration GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return new ResTokenGeneration(Convert.ToBase64String(randomNumber), DateTime.UtcNow.AddMinutes(_options.Value.RefreshExpirationInMinutes));
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtToken = token.Replace("Bearer ", string.Empty);
            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("PDv7DrqznYL6nv7DrqzjnQYO9JxIsWdcjnQYL6nu0f")),
                ValidateIssuer = true, //_options.Value.TokenValidationParameter.ValidateIssuer,
                ValidateAudience = true,// _options.Value.TokenValidationParameter.ValidateAudience, //you might want to validate the audience and issuer depending on your use case
                ValidateLifetime = true,// _options.Value.TokenValidationParameter.ValidateLifetime, //here we are saying that we don't care about the token's expiration date
                ValidateIssuerSigningKey = true,// _options.Value.TokenValidationParameter.ValidateIssuerSigningKey,
                ValidIssuer = "localhost",
                ValidAudience = "localhost",
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null)
                throw new RequiredTokenExeption(Resources.TOKEN_NULL);
            else if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new RequiredTokenExeption(Resources.TOKEN_INVALID);

            return principal;
        }
    }
}
