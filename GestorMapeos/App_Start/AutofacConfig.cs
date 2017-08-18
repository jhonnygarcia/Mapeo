using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using GestorMapeos.ApiControllers.Base;
using GestorMapeos.App_Start;
using GestorMapeos.Models.Model.Erp20;
using GestorMapeos.Models.Model.Gestor;
using GestorMapeos.Models.Services.Gestor;
using Newtonsoft.Json;

namespace GestorMapeos
{
    public static class AutofacConfig
    {
        public static IContainer Container { get; set; }
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ErpContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<GestorContext>().AsSelf().InstancePerLifetimeScope();

            //-> Servicios 
            builder.RegisterAssemblyTypes(typeof(IPlantillaAsignaturaIntegracionServices).Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .Where(t => t.Name.EndsWith("Services"))
                .PropertiesAutowired();

            builder.RegisterAssemblyTypes(typeof(IPlantillaAsignaturaIntegracionServices).Assembly)
                .InstancePerLifetimeScope()
                .Where(t => t.Name.EndsWith("Services"))
                .PropertiesAutowired();

            Container = builder.Build();
        }
        public static void MvcConfig(IContainer container)
        {
            var builder = new ContainerBuilder();

            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterFilterProvider();

            builder.RegisterControllers(AppDomain.CurrentDomain.GetAssemblies())
                .AssignableTo<Controller>()
                .PropertiesAutowired();

            // Filters Globales
            builder.Update(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            AreaRegistration.RegisterAllAreas();
        }
        public  static void WebApiConfig(HttpConfiguration config, IContainer container)
        {
            // Configuración de WebAPI
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(AppDomain.CurrentDomain.GetAssemblies())
                .AssignableTo<ApiControllerBase>()
                .PropertiesAutowired();
            builder.RegisterWebApiFilterProvider(config);

            builder.Update(container);

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            config.MapHttpAttributeRoutes(new CustomDirectRouteProvider());
            config.Filters.Add(new ExceptionHandlingAttribute());
        }
        public class CustomDirectRouteProvider : DefaultDirectRouteProvider
        {
            protected override IReadOnlyList<IDirectRouteFactory>
            GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
            {
                // inherit route attributes decorated on base class controller's actions
                return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(true);
            }
        }
    }
}