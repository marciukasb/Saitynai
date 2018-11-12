using System;
using System.Linq;
using System.Web.Optimization;
using System.Web.Routing;

namespace MongoStack
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            new AppHost().Init();
        }
        protected void Application_Start(object sender, EventArgs e)
        {
            new AppHost().Init();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest()
        {
            //var authString = Request.Headers["Authorization"];
            //if (authString != null)
            //{
            //    var token = authString.Split(' ')[1];
            //    if (token != null && token != "null")
            //    {
            //        var pass = JsonWebToken.Decode(token, "Admin");
            //        if (!pass)
            //        {
            //            Response.StatusCode = 401;
            //        }
            //    }
            //}


            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod == "OPTIONS")
            {

                Response.Flush();
            }
        }
    }
}