

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace PointOfSales.WebUI.Providers
{
    public interface IJwtProvider  
    {
         string GenerateJWT();
    }
    public class CustomJwtProvider : IJwtProvider
    {
        private IConfiguration Configuration;   
        public CustomJwtProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public string GenerateJWT()
        {
            var issuer = Configuration["Jwt:Issuer"];
            var audience = Configuration["Jwt:Audience"];
            var expiry = System.DateTime.Now.AddMinutes(120);
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var credentials = new SigningCredentials (securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: issuer, 
            audience:audience,
            expires: System.DateTime.Now.AddMinutes(120), 
            signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }
    }
}