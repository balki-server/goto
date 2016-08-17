using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.OData.Formatter;
using System.Web.OData.Routing.Conventions;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using Microsoft.AspNet.Identity;

namespace UrlShortenerWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.EnableCors();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new ElmahLoggingActionFilterAttribute());            
            config.Filters.Add(new HostAuthenticationFilter(DefaultAuthenticationTypes.ApplicationCookie));            

            // Web API routes
            config.MapHttpAttributeRoutes();

            var odataFormatters = ODataMediaTypeFormatters.Create();
            config.Formatters.InsertRange(0, odataFormatters);

            var conventions = ODataRoutingConventions.CreateDefault();

            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<UrlShortenerModels.Models.ShortenedUrl>("ShortenedUrls");

            //builder.Namespace = typeof(UserViewModel).Namespace;


            config.EnableEnumPrefixFree(enumPrefixFree: true);

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: builder.GetEdmModel(),
                pathHandler: new DefaultODataPathHandler(),
                routingConventions: conventions);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
