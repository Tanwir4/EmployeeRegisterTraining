using EmployeeTrainingRegistration.App_Start;
using EmployeeTrainingRegistrationServices.Interfaces;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EmployeeTrainingRegistration
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            HangfireAspNet.Use(GetHangfireServers);
      
            //BackgroundJob.Schedule<IAutomaticProcessingService>(x => x.StartAutomaticProcessing(), TimeSpan.FromMinutes(1));

        }
        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
        }

        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseActivator(new ContainerJobActivator(UnityConfig.Container))
                .UseSqlServerStorage("Server=localhost; Database=HangfireTest; Integrated Security=True;TrustServerCertificate=True;");

            yield return new BackgroundJobServer();
        }
    }
}
