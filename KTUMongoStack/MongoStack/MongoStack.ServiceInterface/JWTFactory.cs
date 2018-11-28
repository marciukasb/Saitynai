using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using MongoStack.Core.Entities;
using MongoStack.ServiceInterface.Interfaces;

namespace MongoStack.ServiceInterface
{
    public enum JwtHashAlgorithm
    {
        RS256,
        HS384,
        HS512
    }

    public class JsonWebToken
    {
        private static readonly Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>> HashAlgorithms;


        static JsonWebToken()
        {
            HashAlgorithms = new Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>>
            {
                { JwtHashAlgorithm.RS256, (key, value) => { using (var sha = new HMACSHA256(key)) { return sha.ComputeHash(value); } } },
                { JwtHashAlgorithm.HS384, (key, value) => { using (var sha = new HMACSHA384(key)) { return sha.ComputeHash(value); } } },
                { JwtHashAlgorithm.HS512, (key, value) => { using (var sha = new HMACSHA512(key)) { return sha.ComputeHash(value); } } }
            };
        }

        public static string Encode(object payload, JwtHashAlgorithm algorithm)
        {
            var key = ConfigurationManager.AppSettings["Secret"];
            var segments = new List<string>();
            var header = new { alg = algorithm.ToString(), typ = "JWT" };
            byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Formatting.None));
            byte[] payloadBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload, Formatting.None));

            segments.Add(Base64UrlEncode(headerBytes));
            segments.Add(Base64UrlEncode(payloadBytes));

            var stringToSign = string.Join(".", segments.ToArray());

            var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

            byte[] signature = HashAlgorithms[algorithm](Encoding.UTF8.GetBytes(key), bytesToSign);
            segments.Add(Base64UrlEncode(signature));

            return string.Join(".", segments.ToArray());
        }

        public static bool Decode(string token, IUserService iuserservice)
        {
            token = token.Replace("Bearer ", "");
            var helper = new DataHelper();
            try
            {
                var key = ConfigurationManager.AppSettings["Secret"];
                var parts = token.Split('.');
                var header = parts[0];
                var payload = parts[1];
                var crypto = Base64UrlDecode(parts[2]);

                var headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
                var headerData = JObject.Parse(headerJson);
                var payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
                var payloadData = JObject.Parse(payloadJson);

                var payloadObject = payloadData.ToObject<TokenData>();
                var userFromDb = iuserservice.GetUserByUsername(payloadObject.Username);
                if (payloadObject.Username != userFromDb.Entity.Username)
                {
                    return false;
                }
                if (payloadObject.Expires < DateTime.Now)
                {
                    return false;
                }

                var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
                var keyBytes = Encoding.UTF8.GetBytes(key);
                var algorithm = (string) headerData["alg"];

                var signature = HashAlgorithms[GetHashAlgorithm(algorithm)](keyBytes, bytesToSign);
                var decodedCrypto = Convert.ToBase64String(crypto);
                var decodedSignature = Convert.ToBase64String(signature);

                return decodedCrypto == decodedSignature;
            }
            catch
            {
                return false;
            }
        }

        private static JwtHashAlgorithm GetHashAlgorithm(string algorithm)
        {
            switch (algorithm)
            {
                case "RS256": return JwtHashAlgorithm.RS256;
                case "HS384": return JwtHashAlgorithm.HS384;
                case "HS512": return JwtHashAlgorithm.HS512;
                default: throw new InvalidOperationException("Algorithm not supported.");
            }
        }

        private static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0];     
            output = output.Replace('+', '-'); 
            output = output.Replace('/', '_'); 
            return output;
        }

        private static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+');
            output = output.Replace('_', '/'); 
            switch (output.Length % 4) 
            {
                case 0: break; 
                case 2: output += "=="; break;
                case 3: output += "="; break; 
                default: throw new Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); 
            return converted;
        }
    }
}
