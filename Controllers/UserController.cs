using api_web.Controllers.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography;
using WebAPI;

namespace api_web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly EF_DataContext _context;

        public UserController(EF_DataContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public RestData Register(User user)
        {
            try
            {
                string hashPassword = HashedPasswordV2(user.Password!);

                var new_user = new User()
                {
                    UserName = user.UserName,
                    Password = hashPassword
                };

                _context.Users.Add(new_user);

                _context.SaveChanges();

                return new RestData();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("login")]
        public RestData Login(User user)
        {
            try
            {
                //hash mật khẩu từ người dùng và so sánh với database
                string hassPassword = HashedPasswordV2(user.Password!);
                var isVerify = _context.Users.Any(stm => stm.UserName == user.UserName && stm.Password == hassPassword);

                if (isVerify)
                {
                    //to login
                    return new RestData();
                }
                return new RestData(status: "Sai tài khoản hoặc mật khẩu!");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string HashedPasswordV2(string password)
        {
            try
            {
                // Generate a 128-bit salt using a sequence of
                // cryptographically strong random bytes.
                byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
                Debug.WriteLine($"Salt: {Convert.ToBase64String(salt)}");

                // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));
                return hashed;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
