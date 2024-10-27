using MySuperBlogApp.Data;
using System.Security.Claims;
using System.Text;

namespace MySuperBlogApp.Services
{
    public class UsersService
    {
        private MyAppDataContext _dataContext;
        public UsersService(MyAppDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public (string login, string password) GetUserLoginPassFromBasicAuth(HttpRequest request)
        {
            string userName = "";
            string userPass = "";
            string authHeader = request.Headers["Authorization"].ToString();
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUserNamePass = authHeader.Replace("Basic ", "");
                var encoding = Encoding.GetEncoding("iso-8859-1");

                string[] namePassArray = encoding.GetString(Convert.FromBase64String(encodedUserNamePass)).Split(':');
                userName = namePassArray[0];
                userPass = namePassArray[1];
            }
            return (userName, userPass);
        }

        public (ClaimsIdentity identity, int id)? GetIdentity(string email, string password) 
        {
            User? currentUser = GetUserByLogin(email);

            if (currentUser == null || !VerifyHashedPassword(currentUser.Password, password)) return null;

            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, currentUser.Email)
                };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims,
                "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return (claimsIdentity, currentUser.Id);
        }

        private User? GetUserByLogin(string email)
        {
            return _dataContext.Users.FirstOrDefault(x => x.Email == email); //??? return -> try
        }

        private bool VerifyHashedPassword(string password1, string password2)
        {
            return password1 == password2;
        }
    }
}
