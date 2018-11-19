using System;
using System.ComponentModel.DataAnnotations;

namespace MongoStack.Core.Entities
{
    public class User : Entity
    {
        public string Role { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Credentials { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
    }

    public class TokenData
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
