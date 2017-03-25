using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace _99meat
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
           
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles); GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
        }
        protected void Application_BeginRequest()
        {
            
                Request.Headers.Remove("Access-Control-Allow-Origin");
                Request.Headers.Add("Access-Control-Allow-Origin", "*");
                Response.Headers.Remove("Access-Control-Allow-Origin");
                  Response.Headers.Add("Access-Control-Allow-Origin", "*");
           
        }

        protected void Application_EndRequest()
        {
            Request.Headers.Remove("Access-Control-Allow-Origin");
            Request.Headers.Add("Access-Control-Allow-Origin", "*");
            Response.Headers.Remove("Access-Control-Allow-Origin");
            if (Response.BufferOutput == true)
                Response.Headers.Add("Access-Control-Allow-Origin", "*");

        }
    }
}
