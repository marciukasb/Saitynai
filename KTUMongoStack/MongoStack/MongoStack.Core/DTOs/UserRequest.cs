using ServiceStack.ServiceHost;

namespace MongoStack.Core.DTOs
{
    public class AuthResponse
    {
        public string Role { get; set; }
        public string Token { get; set; }
    }

    [Route("/user/authenticate", "POST")]
    public class Authenticate
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [Route("/user/register", "POST")]
    public class CreateUser
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
