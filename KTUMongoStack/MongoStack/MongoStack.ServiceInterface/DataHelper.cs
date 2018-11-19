using MongoDB.Bson;
using System.Security.Cryptography;
using System.Text;

namespace MongoStack.ServiceInterface
{
    public class DataHelper : IHelper
    {
        public string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.ASCII.GetBytes(text));
            var result = md5.Hash;

            var strBuilder = new StringBuilder();
            foreach (var t in result)
            {
                strBuilder.Append(t.ToString("x2"));
            }

            return strBuilder.ToString();
        }


        public object Map(object from, object to)
        {
            foreach (var prop in to.GetType().GetProperties())
            {
                var source = from.GetType().GetProperty(prop.Name);
                if (source == null || (source.GetValue(@from, null) == null)) continue;
                if (prop.GetValue(to) != null && prop.GetValue(to).ToString().Length == 24)
                    prop.SetValue(to, new ObjectId(prop.GetValue(to).ToString()));
                else
                    prop.SetValue(to, source.GetValue(@from, null), null);
            }
            return to;
        }
    }
}
