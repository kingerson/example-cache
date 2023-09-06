using Autofac;
using Autofac.Core;
using Autofac.Features.Variance;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using MS.IConstruye.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MS.IConstruye
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterSource(new HandlerControvariantRegistrationSource(
               typeof(IRequestHandler<,>),
               typeof(INotificationHandler<>),
               typeof(IValidator<>)
           ));

            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly).AsImplementedInterfaces();


            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>),
                typeof(IValidator<>),
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                builder
                    .RegisterAssemblyTypes(typeof(BaseCommand).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();
            }

            builder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            base.Load(builder);
        }

    }

    public class HandlerControvariantRegistrationSource : IRegistrationSource
    {
        private readonly IRegistrationSource _source = new ContravariantRegistrationSource();
        private readonly List<Type> _types = new List<Type>();

        public HandlerControvariantRegistrationSource(params Type[] types)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));
            if (!types.All(x => x.IsGenericTypeDefinition))
                throw new ArgumentException("types must be generic");
            _types.AddRange(types);
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Autofac.Core.Service service, Func<Autofac.Core.Service, IEnumerable<ServiceRegistration>> registrationAccessor)
        {
            var component = _source.RegistrationsFor(service, registrationAccessor);
            foreach (var c in component)
            {
                var defs = c.Target.Services
                   .OfType<TypedService>()
                   .Select(x => x.ServiceType.GetGenericTypeDefinition());

                if (defs.Any(_types.Contains))
                    yield return c;
            }
        }

        public bool IsAdapterForIndividualComponents => _source.IsAdapterForIndividualComponents;
    }
}
