using MongoStack.Core;
using MongoStack.Core.Entities;

namespace MongoStack.ServiceInterface.Interfaces
{
    public interface IUserService
    {
        ResultWithEntities<User> GetAllUsers();
        ResultWithEntity<User> GetUserById(string id);
        ResultWithEntity<User> LoginUser(string userName, string password);
        ResultWithEntity<User> GetUserByUsername(string username);
        ResultWithEntity<User> GetUserByEmail(string email);
        ResultWithEntity<User> CreateUser(User user);
        ResultBase UpdateUser(User user);
        ResultBase DeleteUserById(string id);
        string MD5Encode(string data);
    }
}