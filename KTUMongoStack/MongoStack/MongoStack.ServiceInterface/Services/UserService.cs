using System.Text;
using System.Security.Cryptography;
using MongoStack.ServiceInterface.Interfaces;
using MongoStack.Core.Entities;
using MongoStack.Core;
using System;

namespace MongoStack.ServiceInterface.Services
{
    public class UserService : IUserService
    {       
        private readonly IRepository<User, string> userRepository;

        public UserService(IRepository<User, string> userRepository)
        {
            this.userRepository = userRepository;
        }

        public ResultWithEntity<User> GetUserByUsername(string username)
        {
            var result = userRepository.GetOne(m => m.Username == username);
            return result;
        }

        public ResultWithEntity<User> GetUserByEmail(string email)
        {
            var result = userRepository.GetOne(u => u.Email == email);
            return result;
        }

        public ResultWithEntity<User> GetUserById(string id)
        {
            var result = userRepository.GetOne(id);
            return result;
        }

        public ResultWithEntities<User> GetAllUsers()
        {
            var result = userRepository.GetAll();
            return result;
        }

        public ResultWithEntity<User> CreateUser(User user)
        {
            var result = GetUserByUsername(user.Username);
            //if (!result.Success)
            //{
            //    return new ResultWithEntity<User>
            //    {
            //        Success = false,
            //        Message = result.Message
            //    };
            //}

            if(result.Entity == null)
            {
                return userRepository.AddOne(user);
            }
            else
            {
                return new ResultWithEntity<User>
                {
                    Success= false,
                    Message = "User already exists",

                };
            }
        }

        public ResultBase UpdateUser(User user)
        {
            var result = userRepository.ReplaceOne(user);
            return result;
        }

        public ResultBase DeleteUserById(string id)
        {
            var result = userRepository.DeleteOne(id);
            return result;
        }

        public string MD5Encode(string data)
        {
            MD5 md5 = MD5.Create();
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));
            StringBuilder returnValue = new StringBuilder();

            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            return returnValue.ToString();
        }

        public ResultWithEntity<User> LoginUser(string userName, string password)
        {
            var response = new ResultWithEntity<User>();
            var userExist = GetUserByUsername(userName);
            if (userExist.Success && userExist.Entity != null)
            {
                var hashedPass = MD5Encode(password);
                if (hashedPass.Equals(userExist.Entity.Password))
                {
                    response.Success = true;
                    response.Entity = userExist.Entity;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Wrong password";
                }
            }
            else
            {
                response.Success = false;
                response.Message = string.IsNullOrEmpty(userExist.Message) ? "User doesn't exist" : userExist.Message;
            }

            return response;
        }
    }
}
