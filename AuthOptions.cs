using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MySuperBlogApp.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";

        public const string AUDIENCE = "MyAuthClient";

        const string KEY = "mysupersecret_secretkey!123";

        public const int LIFETIME = 10;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
