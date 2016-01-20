using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;

namespace Planc
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional, action = RouteParameter.Optional}
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; 
        }
    }
}
