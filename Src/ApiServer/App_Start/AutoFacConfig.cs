using Autofac;
using Autofac.Integration.WebApi;
using OpenFiddle.Repos;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace OpenFiddle
{
    public static class AutoFacConfig
    {
        public static void Register(HttpConfiguration config)
        {           
            var builder = new ContainerBuilder();
            builder.RegisterType<LogRepository>().AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterType<FiddleRepository>().AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;
        }
    }
}