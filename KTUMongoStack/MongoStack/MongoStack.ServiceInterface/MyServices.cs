using MongoStack.ServiceInterface.Interfaces;
using ServiceStack.ServiceInterface;
using System.Security.Cryptography;
using MongoStack.Core.DTOs;
using MongoDB.Driver;
using System.Linq;
using System;
using ServiceStack.Common.Web;
using System.Net;
using MongoStack.Core;

namespace MongoStack.ServiceInterface
{
    public class MyServices : Service
    {
        public IHelper Helper { get; set; }

        private readonly ICategoryService icategoryService;
        private readonly IProductService iproductservice;
        private readonly IUserService iuserservice;

        public MyServices(ICategoryService icategoryService,
                          IProductService iproductservice,
                          IUserService iuserservice)
        {
            this.icategoryService = icategoryService;
            this.iproductservice = iproductservice;
            this.iuserservice = iuserservice;
        }

        #region Users

        public object Post(IsAdmin request)
        {
            if (JsonWebToken.Decode(request.Token, request.Username))
            {
                var result = iuserservice.GetUserByUsername(request.Username);
                if (result.Success && result.Entity.Role == "Admin")
                {
                    return new AuthoriseResponse { Result = true };
                }
                else
                {
                    return new AuthoriseResponse { Result = false };
                }
            }
            else
            {
                return new AuthoriseResponse { Result = false };
            }
        }

        public object Post(Authenticate request)
        {
            var jwt = JsonWebToken.Encode(request, JwtHashAlgorithm.HS512);
            var credentials = Helper.GenerateToken(request.Username, request.Password);
            var result = iuserservice.GetUserByUsername(request.Username);

            if (result.Success)
            {
                var hashparts = result.Entity.Password.Split(':');
                string hash;
                using (MD5 md5Hash = MD5.Create())
                {
                    hash = Helper.GetMd5Hash(md5Hash, request.Password + hashparts[1]);
                }
                if (hash == hashparts[0])
                {
                    return new AuthResponse { Token = jwt, Role = result.Entity.Role };
                }
                else
                {
                    return new AuthResponse { Token = null };
                }
            }
            return new AuthResponse { Token = null };
        }

        public object Post(GetUser request)
        {
            if (JsonWebToken.Decode(request.Token, request.UserName))
            {
                var result = iuserservice.GetUserByUsername(request.UserName);
                if (result.Success)
                {
                    return new UserResponse { Result = true, User = result.Entity };
                }
                return new UserResponse { Result = false };
            }
            else
            {
                return new UserResponse { Result = false };
            }
        }

        public object Post(GetAllUsers request)
        {
            if (request.Token != null && JsonWebToken.Decode(request.Token, request.Username))
            {
                var user = iuserservice.GetAllUsers();
                if (user.Success)
                {
                    return new UsersResponse { Result = false };
                }
                return new UserResponse { Result = false };
            }
            else
            {
                return new UserResponse { Result = false };
            }
        }


        #endregion

        #region Categories

        public object Post(AddCategory request)
        {
            var result = icategoryService.CreateCategory(request);
            if (result.Success)
            {
                return null;
            }

            // STATUS CODE
            return null;
        }


        public object Post(DeleteCategory request)
        {
            var result = icategoryService.DeleteCategory(request.Id);
            if (result.Success)
            {
                return null;
            }

            // STATUS CODE

            return null;
        }

        public object Get(GetCategories request)
        {
            var result = icategoryService.GetAllCategories();
            if (result.Success)
            {
                return result.Entities.ToList();

            }

            // STATUS CODE

            return null;
        }


        public object Get(GetCategory request)
        {
            var result = icategoryService.GetCategoryById(request.Id);
            if (result.Success)
            {
                return result.Entity;
            }

            // STATUS CODE

            return null;
        }

        public object Post(UpdateCategory request)
        {
            try
            {
                icategoryService.UpdateCategory(request.Category);
            }
            catch (Exception ex)
            {
                return null;

            }
            // STATUS CODE

            return null;
        }

        #endregion

        #region Products

        public object Get(GetProducts request)
        {
            var result = iproductservice.GetAllProducts();
            if (!string.IsNullOrEmpty(result.Message))
            {
                return StatusCode(result);
            }
            else if (result.Entities.Count() == 0)
            {
                return HttpError.NotFound("Not Found");
            }
            return result.Entities.ToList();
        }

        public object Get(GetProduct request)
        {
            if (request.Id?.Length != 24) return new HttpError(HttpStatusCode.BadRequest, "Bad Request");
            var result = iproductservice.GetProductById(request.Id);
            if (!string.IsNullOrEmpty(result.Message))
            {
                return StatusCode(result);
            }
            else if (result.Entity == null)
            {
                return HttpError.NotFound("Not Found");
            }
            return result.Entity;
        }

        public object Post(AddProduct request)
        {
            var result = iproductservice.CreateProduct(request);

            if (!string.IsNullOrEmpty(result.Message))
            {
                return StatusCode(result);
            }
            else if (result.Entity == null)
            {
                return new HttpError(HttpStatusCode.BadRequest, "BadRequest");
            }
            return new HttpError(HttpStatusCode.Created, "Created", result.Entity.Id);
        }

     

        public object Delete(DeleteProduct request)
        {
            if (request.Id?.Length != 24) return new HttpError(HttpStatusCode.BadRequest, "Bad Request");
            var result = iproductservice.DeleteProduct(request.Id);
            if (!result.Success)
            {
                return StatusCode(result);
            }
            return null;
        }

        public object Put(UpdateProduct request)
        {
            if (request.Id?.Length != 24) return new HttpError(HttpStatusCode.BadRequest, "Bad Request");
            var result = iproductservice.UpdateProduct(request);
            if (!result.Success)
            {
                return StatusCode(result);
            }
            return null;
        }

        #endregion

        private HttpError StatusCode(ResultBase result)
        {
            if (result.Message.Contains("timeout"))
                return new HttpError(HttpStatusCode.RequestTimeout, "RequestTimeout");
            else if(result.Message.Contains("not match"))
                return new HttpError(HttpStatusCode.InternalServerError, "Data missmatch");
            else if (result.Message.Length == 0 && result.Success == false)
                return new HttpError(HttpStatusCode.NotFound, "Not Found");

            return new HttpError(HttpStatusCode.BadRequest, "BadRequest");
        }
    }
}