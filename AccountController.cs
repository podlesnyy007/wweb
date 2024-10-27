using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MySuperBlogApp.Data;
using MySuperBlogApp.Models;
using MySuperBlogApp.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MySuperBlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UsersService _userService;
        public AccountController(MyAppDataContext dataContext)
        {
            _userService = new UsersService(dataContext);
        }

        [HttpGet]

        public IActionResult Get()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public object Create(UserModel user)
        {

            // add user to DB

            return Ok(user);
        }

        [HttpPatch]
        public object Update(UserModel user)
        {
            // check current user from request with user model
            // update user in DB

            return Ok(user);
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public object GetToken()
        {
            // get user data from DB
            var userData = _userService.GetUserLoginPassFromBasicAuth(Request);
            // get identity
            (ClaimsIdentity claims, int id)? identity = _userService.GetIdentity(userData.login, userData.password);

            if (identity is null) return NotFound("login or password is not correct");

            //create jwt token
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity?.claims.Claims,
                expires: now.AddMinutes(AuthOptions.LIFETIME),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            // return token

            var tokenModel = new AuthToken(
                minutes: AuthOptions.LIFETIME,
                accessToken: encodedJwt,
                userName: userData.login,
                userId: identity.Value.id);

            return Ok(tokenModel);
        }
    }
}
