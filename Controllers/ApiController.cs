using api_web.Controllers.Helpers;
using api_web.Module.MainPage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
        [HttpPost("/login/token")]
        public string Token(User user)
        {
            List<User> list = new List<User>()
            {
                new User()
                {
                    UserName = "huy",
                    Password = "huy123"
                },
                new User()
                {
                    UserName = "sa",
                    Password = "123456"
                }
            };
            var isAuthenticate = list.Where(x => x.UserName == user.UserName && x.Password == user.Password).FirstOrDefault();

            if (isAuthenticate != null)
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
                return "Sai tài khoản hoặc mật khẩu!";
            }
        }
        //jwt
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
                return new RestData(status: e.Message);
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
                return new RestData(status: e.Message);
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
                return new RestData(status: "OK");
            }
            catch (Exception e)
            {
                return new RestData(status: e.Message);
            }


        }
    }
}