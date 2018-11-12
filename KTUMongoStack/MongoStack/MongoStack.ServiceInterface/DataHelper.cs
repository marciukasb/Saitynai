using MongoDB.Bson;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace MongoStack.ServiceInterface
{
    public class DataHelper : IHelper
    {
        public string encrypt(string password)
        {
            string hash;
            var salt = RandomString();

            using (MD5 md5Hash = MD5.Create())
            {
                hash = GetMd5Hash(md5Hash, password + salt);
            }

            return hash + ':' + salt;
        }
        private string RandomString()
        {
            Random rd = new Random();
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var builder = new StringBuilder();

            for (var i = 0; i < 32; i++)
            {
                var c = pool[rd.Next(0, pool.Length)];
                builder.Append(c);
            }

            return builder.ToString();
        }
        public string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }


        public object Map(object from, object to)
        {
            foreach (PropertyInfo prop in to.GetType().GetProperties())
            {
                PropertyInfo source = from.GetType().GetProperty(prop.Name);
                if (source != null && (source.GetValue(from, null) != null))
                {
                    if (prop.GetValue(to) != null && prop.GetValue(to).ToString().Length == 24)
                        prop.SetValue(to, new ObjectId(prop.GetValue(to).ToString()));
                    else
                        prop.SetValue(to, source.GetValue(from, null), null);
                }
            }
            return to;
        }

        public string GenerateToken(string username, string password)
        {
            var secret = ConfigurationManager.AppSettings["Secret"];
            string s = string.Format("{0};{1};{2}", username, password, secret); 

            var bits = string.Empty;
            foreach (var character in s)
            {
                bits += Convert.ToString(character, 2).PadLeft(8, '0');
            }

            string base64 = string.Empty;

            const byte threeOctets = 24;
            var octetsTaken = 0;
            while (octetsTaken < bits.Length)
            {
                var currentOctects = bits.Skip(octetsTaken).Take(threeOctets).ToList();

                const byte sixBits = 6;
                int hextetsTaken = 0;
                while (hextetsTaken < currentOctects.Count())
                {
                    var chunk = currentOctects.Skip(hextetsTaken).Take(sixBits);
                    hextetsTaken += sixBits;

                    var bitString = chunk.Aggregate(string.Empty, (current, currentBit) => current + currentBit);

                    if (bitString.Length < 6)
                    {
                        bitString = bitString.PadRight(6, '0');
                    }
                    var singleInt = Convert.ToInt32(bitString, 2);

                    base64 += Base64Letters[singleInt];
                }

                octetsTaken += threeOctets;
            }
            for (var i = 0; i < (bits.Length % 3); i++)
            {
                base64 += "=";
            }
            return base64;
        }



        private static readonly char[] Base64Letters = new[]
                                                {
                                              'M'
                                            , 'L'
                                            , 'K'
                                            , 'J'
                                            , 'I'
                                            , 'H'
                                            , 'G'
                                            , 'F'
                                            , 'E'
                                            , 'D'
                                            , 'C'
                                            , 'B'
                                            , 'A'
                                            , 'Z'
                                            , 'Y'
                                            , 'X'
                                            , 'W'
                                            , 'V'
                                            , 'U'
                                            , 'T'
                                            , 'S'
                                            , 'R'
                                            , 'Q'
                                            , 'P'
                                            , 'O'
                                            , 'N'
                                            , 'a'
                                            , 'b'
                                            , 'c'
                                            , 'd'
                                            , 'f'
                                            , 'e'
                                            , 'g'
                                            , 'h'
                                            , 'j'
                                            , 'i'
                                            , 'k'
                                            , 'l'
                                            , 'o'
                                            , 'n'
                                            , 'm'
                                            , 'p'
                                            , 'q'
                                            , 'r'
                                            , 's'
                                            , 't'
                                            , 'v'
                                            , 'u'
                                            , 'w'
                                            , 'x'
                                            , 'y'
                                            , 'z'
                                            , '9'
                                            , '8'
                                            , '7'
                                            , '6'
                                            , '5'
                                            , '4'
                                            , '1'
                                            , '2'
                                            , '3'
                                            , '0'
                                            , '+'
                                            , '/'
                                            , '='
                                        };
    }
}
