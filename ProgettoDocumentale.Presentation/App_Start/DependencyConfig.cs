using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using MediatR;
using Microsoft.Owin.Security;
using ProgettoDocumentale.Application.Abstractions;
using ProgettoDocumentale.Application.Common.Interfaces;
using ProgettoDocumentale.Application.Requests.UserManagment.Queries.GetUserBy;
using ProgettoDocumentale.Application.Services;
using ProgettoDocumentale.Infrastructure.Persistence;

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
                .InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterAssemblyTypes(appAssembly)
               .AsClosedTypesOf(typeof(IRequestHandler<,>))
               .InstancePerLifetimeScope();

            builder.RegisterType<PasswordEncryptionService>()
               .As<IPasswordEncryptionService>()
               .SingleInstance();

            builder.RegisterType<ProgettoDocContext>()
               .As<IProgettoDocContext>()
               .InstancePerLifetimeScope();

            //builder.Register(c => HttpContext.Current.GetOwinContext().Authentication)
            // .As<IAuthenticationManager>()
            //.InstancePerRequest();

            builder.RegisterAssemblyTypes(webAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(FluentValidation.IValidator<>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}