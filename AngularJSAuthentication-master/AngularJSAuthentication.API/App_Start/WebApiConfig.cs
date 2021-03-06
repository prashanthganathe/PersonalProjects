﻿using AngularJSAuthentication.API.Filters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Mvc;

namespace AngularJSAuthentication.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Custom
            config.EnableCors();
            config.Filters.Add(new ActionLogFilter());
          //  config.Filters.Add(new ElmahErrorAttribute());
            AreaRegistration.RegisterAllAreas();
           
            // Web API routes
            config.MapHttpAttributeRoutes();
          
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {controller=RouteParameter.Optional, id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Default",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new {  id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

          
            
        }
    }
}
