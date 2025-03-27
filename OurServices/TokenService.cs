using api.Interfaces;
using api.Models;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.OurServices
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _confg; //To read appsettings

        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config  )
        {

            _confg = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_confg["JWT:SigningKey"])); //Key is important that's why we have used it in appsettings and Encoded it with UTF8.GetBytes as it will contain data in bytes for the string.
        }
        string ITokenService.CreateToken(AppUser user)
        {
            //Generating claims
            var claims = new List<Claim> { 
            new Claim(JwtRegisteredClaimNames.Email ,user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName,user.UserName)
            
            };//What we have done here we create a List of Clam and then initialize this list on the fly and initialize the each claim object here.

            //Signing Credintials , basically what type of encryption do you want
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            //now creating object representaion of token
            var tokendescriptor = new SecurityTokenDescriptor { 
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds,
            Issuer = _confg["JWT:Issuer"],
            Audience = _confg["JWT:Audience"]
            
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokendescriptor);//Creating token from C# object token descriptor using tokenhandler.
            return tokenHandler.WriteToken(token); // returning token in string so no body can break through the integrity of the token.            

        }
    }
}
