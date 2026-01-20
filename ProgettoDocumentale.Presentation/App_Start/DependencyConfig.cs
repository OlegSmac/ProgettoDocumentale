using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using FluentValidation;
using MediatR;
using Microsoft.Owin.Security;
using ProgettoDocumentale.Application.Abstractions;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Services;
using ProgettoDocumentale.Infrastructure.Persistence;
using ProgettoDocumentale.Infrastructure.Services;

namespace ProgettoDocumentale.Presentation.App_Start
{
    public static class DependencyConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            var webAssembly = Assembly.GetExecutingAssembly();
            var appAssembly = typeof(IProgettoDocContext).Assembly;

            builder.RegisterControllers(webAssembly);

            builder.RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerRequest();

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterAssemblyTypes(appAssembly)
               .AsClosedTypesOf(typeof(IRequestHandler<,>))
               .InstancePerLifetimeScope();

            builder.RegisterType<DateTimeService>()
                .As<IDateTime>()
                .SingleInstance();

            builder.RegisterType<CurrentUserService>()
                .As<ICurrentUserService>()
                .InstancePerRequest();

            builder.Register(c => new ProgettoDocContext(
                c.Resolve<IDateTime>(),
                c.Resolve<ICurrentUserService>()
            ))
            .As<IProgettoDocContext>()
            .As<System.Data.Entity.DbContext>()
            .InstancePerRequest();

            builder.RegisterAssemblyTypes(appAssembly)
                .Where(t => t.GetInterfaces().Any(i => i.IsClosedTypeOf(typeof(IValidator<>))))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}