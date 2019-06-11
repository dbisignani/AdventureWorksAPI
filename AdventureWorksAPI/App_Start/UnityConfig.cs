using AdventureWorksAPI.Models;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace AdventureWorksAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            container.RegisterType<IEmployeeDataRepository, EmployeeDataRepository>();
            //container.RegisterType<IThingRepository, ThingRepository>();
            container.RegisterType<AdventureWorks2016Entities, AdventureWorks2016Entities>();

        }
    }
}