using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Syntax;
using Ninject.Web.Mvc;
using PhotoWall.Models;
using SignalR;

namespace VideoChatWebApplication
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            // Configure Ninject
            StandardKernel kernel = new StandardKernel();
            kernel.Bind<IUserManager>().To<UserManager>().InSingletonScope();

            // Activate Ninject in SignalR
            GlobalHost.DependencyResolver = new SignalR.Ninject.NinjectDependencyResolver(kernel);
            RouteTable.Routes.MapHubs();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            // Activate Ninject in ASP.NET MVC
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}