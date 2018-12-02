using System;
using System.Linq;
using System.Net;
using MongoStack.Core;
using MongoStack.Core.DTOs;
using MongoStack.Core.Entities;
using MongoStack.ServiceInterface.Interfaces;
using ServiceStack.Common;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface;

namespace MongoStack.ServiceInterface
{
    public class MyServices : Service
    {
        public IHelper Helper { get; set; }

        private readonly ICategoryService icategoryService;
        private readonly IProductService iproductservice;
        private readonly IUserService iuserservice;

        public MyServices(ICategoryService icategoryService, IProductService iproductservice, IUserService iuserservice)
        {
            this.icategoryService = icategoryService;
            this.iproductservice = iproductservice;
            this.iuserservice = iuserservice;
        }

        #region Users

        public object Post(Authenticate request)
        {
            var obj = new TokenData { Username = request.Username, Expires = DateTime.Now.AddHours(1) };
            var jwt = JsonWebToken.Encode(obj, JwtHashAlgorithm.HS512);
            var result = iuserservice.GetUserByUsername(request.Username);

            if (!result.Success && string.IsNullOrEmpty(result.Message)) return new HttpError(HttpStatusCode.Forbidden, "Username does not exist");
            if (!result.Success && !string.IsNullOrEmpty(result.Message)) return StatusCode(result);

            var passEncrypt = Helper.MD5Hash(request.Password);
            if (result.Entity.Password == passEncrypt)
            {
                return new AuthResponse {Token = jwt, Admin = result.Entity.Admin };
            }

            return new HttpError(HttpStatusCode.Forbidden, "Incorrect password");

        }
        public object Get(Authorize request)
        {
            if (Request.Headers["Authorization"] == null || !JsonWebToken.DecodeAdminToken(Request.Headers["Authorization"], iuserservice))
                return new HttpError(HttpStatusCode.BadRequest, "Invalid token");

            return new AuthResponse();
        }

        public object Post(CreateUser request)
        {
            var result = iuserservice.GetUserByUsername(request.UserName);

            if (!result.Success && result.Entity == null)
            {
                var passEncrypt = Helper.MD5Hash(request.Password);

                try
                {
                    var user = iuserservice.CreateUser(new User
                    {
                        Username = request.UserName,
                        Password = passEncrypt,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        Email = request.Email
                    });
                    if (!user.Success) return new HttpError(HttpStatusCode.InternalServerError, "An error occurred");

                    var obj = new TokenData { Username = request.UserName, Expires = DateTime.Now.AddHours(1), Admin = result.Entity.Admin };
                    var jwt = JsonWebToken.Encode(obj, JwtHashAlgorithm.HS512);
                    return new AuthResponse {Token = jwt};
                }
                catch
                {
                    return new HttpError(HttpStatusCode.InternalServerError, "An error occurred");
                }
            }
            if (result.Success && result.Entity != null)
            {
                return new HttpError(HttpStatusCode.BadRequest, "Username already exists");
            }
            return new AuthResponse { Token = null };
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
            catch
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
            if (Request.Headers["Authorization"] == null || !JsonWebToken.Decode(Request.Headers["Authorization"], iuserservice))
                return new HttpError(HttpStatusCode.Forbidden, "Invalid token");

            var result = iproductservice.GetAllProducts();
            if (!string.IsNullOrEmpty(result.Message))
            {
                return StatusCode(result);
            }
            if (!result.Entities.Any())
            {
                return HttpError.NotFound("Not Found");
            }
            return result.Entities.ToList();
        }

        public object Get(GetProduct request)
        {
            if (Request.Headers["Authorization"] == null || !JsonWebToken.Decode(Request.Headers["Authorization"], iuserservice))
                return new HttpError(HttpStatusCode.Forbidden, "Invalid token");

            if (request.Id?.Length != 24) return new HttpError(HttpStatusCode.BadRequest, "Bad Request");
            var result = iproductservice.GetProductById(request.Id);
            if (!string.IsNullOrEmpty(result.Message))
            {
                return StatusCode(result);
            }

            if (result.Entity == null)
            {
                return HttpError.NotFound("Not Found");
            }
            return result.Entity;
        }

        public object Post(AddProduct request)
        {
            if (Request.Headers["Authorization"] == null || !JsonWebToken.Decode(Request.Headers["Authorization"], iuserservice))
                return new HttpError(HttpStatusCode.Forbidden, "Invalid token");

            var result = iproductservice.CreateProduct(request);
            if (!string.IsNullOrEmpty(result.Message)) return StatusCode(result);

            return result.Entity == null ? new HttpError(HttpStatusCode.BadRequest, "BadRequest") : new HttpError(HttpStatusCode.Created, "Created", result.Entity.Id);
        }

        public object Delete(DeleteProduct request)
        {
            if (Request.Headers["Authorization"] == null || !JsonWebToken.Decode(Request.Headers["Authorization"], iuserservice))
                return new HttpError(HttpStatusCode.Forbidden, "Invalid token");

            if (request.Id?.Length != 24) return new HttpError(HttpStatusCode.BadRequest, "Bad Request");
            var result = iproductservice.DeleteProduct(request.Id);
            return !result.Success ? StatusCode(result) : null;
        }

        public object Put(UpdateProduct request)
        {
            if (Request.Headers["Authorization"] == null || !JsonWebToken.Decode(Request.Headers["Authorization"], iuserservice))
                return new HttpError(HttpStatusCode.Forbidden, "Invalid token");

            if (request.Id?.Length != 24) return new HttpError(HttpStatusCode.BadRequest, "Bad Request");
            var result = iproductservice.UpdateProduct(request);
            return !result.Success ? StatusCode(result) : null;
        }

        #endregion

        private HttpError StatusCode(ResultBase result)
        {
            if (result.Message.Contains("timeout"))
                return new HttpError(HttpStatusCode.RequestTimeout, "RequestTimeout");
            if(result.Message.Contains("not match"))
                return new HttpError(HttpStatusCode.InternalServerError, "Data mismatch");
            if (result.Message.Length == 0 && result.Success == false)
                return new HttpError(HttpStatusCode.NotFound, "Not Found");

            return new HttpError(HttpStatusCode.BadRequest, "BadRequest");
        }
    }
}