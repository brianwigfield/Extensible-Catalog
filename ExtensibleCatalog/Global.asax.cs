using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ExtensibleCatalog.Common;
using MongoDB.Bson.DefaultSerializer;

namespace ExtensibleCatalog
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
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            SetupParts();
            System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new CustomPathProvider());

        }

        protected void SetupParts()
        {
            var itemDirectoryCatalog = new DirectoryCatalog("CatalogItems");
            var assemblyCatalog = new AssemblyCatalog(typeof (MefDependencyResolver).Assembly);
            DependencyResolver.SetResolver(new MefDependencyResolver(itemDirectoryCatalog, assemblyCatalog));
            itemDirectoryCatalog.Changed += CatalogChanged;

            BsonClassMap.RegisterClassMap<Models.CatalogItem>();
            RegisterItemMaps();
        }

        void CatalogChanged(object sender, ComposablePartCatalogChangeEventArgs e)
        {
            RegisterItemMaps();
        }

        void RegisterItemMaps()
        {
            foreach (var item in DependencyResolver.Current.GetServices<Models.CatalogItem>())
            {
                BsonClassMap.LookupClassMap(item.GetType());
            }            
        }


    }
}