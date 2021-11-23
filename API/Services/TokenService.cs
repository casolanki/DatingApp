using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entities;
using API.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        public readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)        
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
           var claims = new List<Claim>
           {
             new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
             //Try check with addon claim property
             //new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString())
           }; 
           
           var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

           var tokenDiscription = new SecurityTokenDescriptor
           {
               Subject = new ClaimsIdentity(claims),
               Expires = DateTime.Now.AddDays(7),
               SigningCredentials = creds
           };  


           var tokenHandler = new JwtSecurityTokenHandler();

           var token = tokenHandler.CreateToken(tokenDiscription);

           return tokenHandler.WriteToken(token);        
        }
    }
}