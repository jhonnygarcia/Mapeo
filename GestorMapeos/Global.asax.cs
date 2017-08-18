using System;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using GestorMapeos.App_Start;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;

namespace GestorMapeos
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            AutofacConfig.Configure();
            var container = AutofacConfig.Container;

            GlobalConfiguration.Configure(config => AutofacConfig.WebApiConfig(config, container));
            AutofacConfig.MvcConfig(container);
            
            // MVC
            RouteTable.Routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            Database.SetInitializer<GestorContext>(null);
            Database.SetInitializer<ErpContext>(null);

            var urlRecursosVista = ConfigurationManager.AppSettings["RecursosVista"];
            if (!string.IsNullOrEmpty(urlRecursosVista) && urlRecursosVista.StartsWith("https"))
            {
                GlobalFilters.Filters.Add(new RequreSecureConnectionFilter());
            }
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
