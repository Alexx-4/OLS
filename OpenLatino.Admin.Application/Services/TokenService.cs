using System.Text;
using System.IdentityModel.Tokens.Jwt;
using OpenLatino.Core.Domain.Services;
using OpenLatino.Core.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Options;
using OpenLatino.Core.Domain.Entities;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace OpenLatino.Admin.Application.Services
{
    public class TokenService:ITokenService
    {
        private string secret_key;
        public TokenService(IOptions<ConfigurationModel> appSettings)
        {
            secret_key = appSettings.Value.Secret_key;
        }

        public string GetTokenFor(ClientCredentialModel client)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret_key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, client.ClientId),
                        new Claim(ClaimTypes.Name, client.Name),
                        new Claim(nameof(client.AllowedOrigen), client.AllowedOrigen),
                        new Claim("expiresFt", client.ExpirationDate.ToString())
                    }
                ),
                Expires = DateTime.FromFileTimeUtc(client.ExpirationDate),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string GetRequestToken(HttpRequest request)
        {
            return request.Headers["Authorization"].ToString().Split("bearer ")[1];
        }

        public static string GetRequestToken(HttpRequestHeaders headers)
        {
            return headers.Authorization.Parameter;
        }

        public static string GetClientId(string str_token)
        {
            var handler = new JwtSecurityTokenHandler();

            //test if token is valid
            if (!handler.CanReadToken(str_token))
                return null;
            var token = handler.ReadJwtToken(str_token);

            return token.Claims.First(c => c.Type == "nameid").Value;
        }
    }
}
