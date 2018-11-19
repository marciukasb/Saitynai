using MongoStack.ServiceInterface.Interfaces;
using ServiceStack.ServiceInterface;
using MongoStack.Core.DTOs;
using System.Linq;
using ServiceStack.Common.Web;
using System.Net;
using MongoStack.Core;
using MongoStack.Core.Entities;
using System.Web;

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

       public object Post(Authenticate request)
       {
            var jwt = JsonWebToken.Encode(request, JwtHashAlgorithm.HS512);
            var result = iuserservice.GetUserByUsername(request.Username);

            if (result.Success)
            {
                var passEncrypt = Helper.MD5Hash(request.Password);

                if (result.Entity.Password == passEncrypt)
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
                        Email = request.Email,
                    });
                    if (!user.Success) return new HttpError(HttpStatusCode.InternalServerError, "An error occurred");

                    var obj = new TokenData {Username = request.UserName, Password = request.Password};
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
            var aspnetRequest = (HttpRequest)base.Request.OriginalRequest;
            var headerValue = aspnetRequest.Headers["apikey"];
            if (headerValue == null || !JsonWebToken.Decode(headerValue, iuserservice))
            {
            }

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
        //    var headerValue = base.Request.Headers["apikey"];

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