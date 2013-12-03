using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LeetRaids
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("Elmah/{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Elmah/{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Elmah/exceptions.axd");
            routes.IgnoreRoute("exceptions.axd");
            routes.IgnoreRoute("{resource}.axd");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );

            routes.MapRoute(
                "GetMemberInfo",                                              // Route name
                "{controller}/{action}",                           // URL with parameters
                new { controller = "Account", action = "GetMembersGames" }  // Parameter defaults
            );

            routes.MapRoute(
                "InsertNewCharacterAJAX",                                              // Route name
                "{controller}/{action}",                           // URL with parameters
                new { controller = "Account", action = "InsertNewCharacterAJAX" }  // Parameter defaults
            );

            routes.MapRoute(
                "GetMembersEventsByDayAJAX",                       // Route name
                "{controller}/{action}",                           // URL with parameters
                new { controller = "Event", action = "GetMembersEventsByDayAJAX" }  // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory());
        }
    }
}