using System.Security.Cryptography;

namespace MongoStack.ServiceInterface
{
    public interface IHelper
    {
        object Map(object from, object to);
        string GenerateToken(string username, string password);
        string GetMd5Hash(MD5 md5Hash, string input);
        string encrypt(string password);
    }
}
