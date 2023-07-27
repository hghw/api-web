using api_web.Controllers.Helpers;
using api_web.Module.MainPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI;

namespace api_web.Controllers
{
    [ApiController]
    [Route("api/location")]
    public class ApiController : ControllerBase
    {

        private readonly ILogger<ApiController> _logger;

        private readonly EF_DataContext _context;
        public ApiController(EF_DataContext context)
        {
            _context = context;
        }


        [AllowAnonymous]
        [HttpPost("/security/createToken")]
        public string Token(User user)
        {
            if (user.UserName == "joydip" && user.Password == "joydip123")
            {
                var issuer = ConfigurationManagerBuilder.AppSetting["Jwt:Issuer"];
                var audience = ConfigurationManagerBuilder.AppSetting["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes
                (ConfigurationManagerBuilder.AppSetting["Jwt:Key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);
                return stringToken;
            }
            else
            {
                return "";
            }

        }

        [HttpGet("list")]
        public RestData List()
        {
            try
            {
                var list = _context.Locations.ToList();
                return new RestData(status: "OK", data: list);
            }
            catch (Exception e)
            {
                return new RestData(status: e.Message, data: null);
            }

        }
        [HttpGet("getItem")]
        public RestData GetItem(int id)
        {
            try
            {
                Location response = new Location();
                var row = _context.Locations.Where(d => d.id.Equals(id)).FirstOrDefault();

                if (row == null) return new RestData(status: "ERROR", data: null);

                return new RestData(status: "OK", data: row);
            }
            catch (Exception e)
            {
                return new RestData(status: e.Message, data: null);
            }
        }
        [HttpPost("add")]
        public RestData SaveOrder(Location item)
        {
            try
            {
                Location dbTable = new Location();
                if (item.id > 0)
                {
                    //PUT
                    dbTable = _context.Locations.Where(d => d.id.Equals(item.id)).FirstOrDefault();
                    if (dbTable != null)
                    {
                        dbTable.lat = item.lat;
                        dbTable.lng = item.lng;
                        dbTable.user_id = item.user_id;
                    }
                }
                else
                {
                    //POST
                    dbTable.lat = item.lat;
                    dbTable.lng = item.lng;
                    dbTable.user_id = item.user_id;
                    _context.Locations.Add(dbTable);
                }
                _context.SaveChanges();
                return new RestData(status: "OK", data: null);
            }
            catch (Exception e)
            {
                return new RestData(status: e.Message, data: null);
            }


        }
    }
}