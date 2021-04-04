using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUser user)
        {
            //Identify claims to put inside this token
            var claims = new List<Claim>
            {
                //NameId is a name identifier for just about everything.
                //we are going use NameId to store user.UserName(The only claim we added for now)
                //We are going to everything using the username that we store in our Jwt
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            //Creating credentials
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            //Descriping our token, what goes inside the token, how iit's gonna look
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            //Token handler
            var tokenHandler = new JwtSecurityTokenHandler();
            //Create token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //Returning a written token to whoever needs it
            return tokenHandler.WriteToken(token);
        }
    }
}