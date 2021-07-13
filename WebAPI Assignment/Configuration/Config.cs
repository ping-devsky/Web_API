using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebApplication1.Model
{
    //The Link that you should put in URl
    // For fetching all employees  https://localhost:44350/api/Employee/
    //For fetching employee by id https://localhost:44350/api/Employee/Get/1
    public class Config
    {
        public static void Register(HttpConfiguration configuration) 
        {
            configuration.MapHttpAttributeRoutes();
            configuration.Routes.MapHttpRoute(
                    name:"DefaultAPI",
                    routeTemplate:"api/{controller}/{action}/{id}",
                    defaults:new {action="Get",Controller="Employee",id=RouteParameter.Optional}          
                );
        }
    }
}