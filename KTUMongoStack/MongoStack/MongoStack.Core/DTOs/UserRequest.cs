using ServiceStack.ServiceHost;
using MongoStack.Core.Entities;

namespace MongoStack.Core.DTOs
{
    public class Unauthorised
    {
        public string Message { get; set; }
    }

    public class AuthoriseResponse
    {
        public bool Result { get; set; }
    }

    public class SendMailResponse
    {
        public bool Result { get; set; }
    }

    public class UserResponse
    {
        public bool Result { get; set; }
        public User User { get; set; }
    }

    public class UsersResponse
    {
        public bool Result { get; set; }
        public User[] Users { get; set; }
    }
    public class UserSearchResponse
    {
        public bool Result { get; set; }
    }

    public class AuthResponse
    {
        public string Role { get; set; }
        public string Token { get; set; }
    }
    public class ChangeResponse
    {
        public string Success { get; set; }
    }
    public class VerifyResponse
    {
        public string Code { get; set; }
        public string Username { get; set; }  
    }

    [Route("/authenticate", "POST")]
    public class Authenticate
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    [Route("/recover", "POST")]
    public class Recover
    {
        public string Email { get; set; }
    }

    [Route("/change", "POST")]
    public class Change
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


    [Route("/getuser", "POST")]
    public class GetUser
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }

    [Route("/getallusers", "POST")]
    public class GetAllUsers
    {
        public string Token { get; set; }
        public string Username { get; set; }
    }

    [Route("/create", "POST")]
    public class CreateUser
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

    
    }

    [Route("/addinfo", "POST")]
    public class AddInfo
    {
        public string UserName { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
    }

    [Route("/isadmin", "POST")]
    public class IsAdmin
    {
        public string Token { get; set; }
        public string Username { get; set; }
    }

    [Route("/sendmail", "POST")]
    public class SendMail
    {
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
