using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace AdventureWorksAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
               name: "BaseAddress",
               routeTemplate: "api",
               defaults: new { controller = "Employee", action = "GetBaseAddress" }
           );

            //Verb = GET
            config.Routes.MapHttpRoute(
                name: "EmployeeData",
                routeTemplate: "api/employee/{BusinessEntityId}",
                defaults: new { controller = "Employee", action = "GetEmployee" },
                constraints: new { BusinessEntityId = @"\d+" }
            );

            config.Routes.MapHttpRoute(
                name: "FindEmployee",
                routeTemplate: "api/employees/search/{SearchText}",
                defaults: new { controller = "Employee", action = "FindEmployee" }//,
                //constraints: new { SearchText = @"^[a - zA - Z] +$" }
            );

            //Verb = GET
            config.Routes.MapHttpRoute(
                name: "Employees",
                routeTemplate: "api/employees",
                defaults: new { controller = "Employee", action = "Get" }
            );

            //Verb = POST
            config.Routes.MapHttpRoute(
                name: "EmployeePatch",
                routeTemplate: "api/employee/address/post",
                defaults: new { controller = "Employee", Action = "PostAddress" }
            );

            //Verb = POST
            config.Routes.MapHttpRoute(
                name: "EmployeeAdd",
                routeTemplate: "api/employee/address/add",
                defaults: new { controller = "Employee", Action = "AddAddress" }
            );

            config.Routes.MapHttpRoute(
                name: "Departments",
                routeTemplate: "api/Department/{DepartmentID}",
                defaults: new { controller = "Department", action = "Get", DepartmentID = RouteParameter.Optional }
            );

            //Verb = DELETE 
            config.Routes.MapHttpRoute(
                name: "Thing",
                routeTemplate: "api/Thing/{id}",
                defaults: new { controller = "Thing", id = RouteParameter.Optional }
            );


            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
