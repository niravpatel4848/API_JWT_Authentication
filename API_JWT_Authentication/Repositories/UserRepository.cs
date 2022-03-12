using API_JWT_Authentication.Context;
using API_JWT_Authentication.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace API_JWT_Authentication.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public UserRepository(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public User Authenticate(string username, string password)
        {
            var user = _db.Users.SingleOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtKey = _config.GetValue<string>("JWTConfig:Key");

            var key = Encoding.ASCII.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            user.Token = tokenHandler.WriteToken(token);

            user.Password = "";

            return user;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.Users.SingleOrDefault(x => x.Username == username);

            if (user == null)
                return true;

            return false;
        }

        public User Register(string username, string password)
        {
            User userObj = new User()
            {
                Username = username,
                Password = password,
                Role = "User"
            };

            _db.Users.Add(userObj);

            _db.SaveChanges();

            userObj.Password = "";

            return userObj;
        }
    }
}


