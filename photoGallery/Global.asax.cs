using photoGallery.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace photoGallery
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();            
            //System.Data.Entity.Database.SetInitializer<GalleryContext>(new photoGallery.Models.SampleData());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<GalleryContext>());
           // Database.SetInitializer<GalleryContext>(null);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
