using System.Security.Cryptography;

namespace MongoStack.ServiceInterface
{
    public interface IHelper
    {
        object Map(object from, object to);
        string MD5Hash(string input);
    }
}
