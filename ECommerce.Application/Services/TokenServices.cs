using ECommerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Application.Services
{
    public class TokenServices (IOptions<JWTSettings> options): ITokenServices
    {
        private readonly JWTSettings settings = options.Value;
        public string CreateToken(string userId, string email, string userName, IEnumerable<string> roles)
        {
            var Claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Name, userName),

            };
            Claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
            var Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var Token = new JwtSecurityToken(
                issuer:settings.Issuer,
                audience :settings.Audience,
                claims: Claims ,
                expires : DateTime.Now.AddMinutes(settings.ExpirationMinutes),
                signingCredentials :Credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
    public class JWTSettings
    {
        public string SecretKey { get; set; } = default;
        public string Issuer { get; set; } = default;
        public string Audience { get; set; } = default;
        public int ExpirationMinutes { get; set; } = 60;


    }
}
