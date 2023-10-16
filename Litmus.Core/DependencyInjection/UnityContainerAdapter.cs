using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using Unity.Lifetime;

namespace Litmus.Core.DependencyInjection
{
    /// <summary>
    /// We use Unity as our DI Framework at Litmus.  Yes, it is a bit dated and the API isn't great.
    /// However, it has been reliable and we have hidden most of the sharp edges via the
    /// IDependencyInjectionContainer interface.  We may consider migrating off of it some day, but
    /// today we are too busy building features for our customers
    /// </summary>
    public class UnityContainerAdapter : IDependencyInjectionContainer
    {
        public IUnityContainer UnityContainer { get; }

        public UnityContainerAdapter()
        {
            this.UnityContainer = new UnityContainer();
            UnityContainer.RegisterInstance<IDependencyInjectionContainer>(this);
        }

        public IEnumerable<Type> RegisteredTypes => UnityContainer.Registrations.Select(r => r.RegisteredType);

        public IEnumerable<Type> MappedTypes => UnityContainer.Registrations.Select(r => r.MappedToType);
        
        public T Resolve<T>() => UnityContainer.Resolve<T>();
        public object Resolve(Type type) => UnityContainer.Resolve(type);

        public IEnumerable<object> ResolveAll(Type type) => UnityContainer.ResolveAll(type);
        public IDependencyInjectionContainer RegisterInstance<T>(T instance, bool overwriteRegistration = false)
        {
            UnityContainer.RegisterInstance(instance);
            return this;
        }

        public IDependencyInjectionContainer RegisterType<T>(bool overwriteRegistration = false)
        {
            UnityContainer.RegisterType<T>();
            return this;
        }

        public IDependencyInjectionContainer RegisterType<T>(Func<IDependencyInjectionContainer, T> factory, bool overwriteRegistration = false)
        {
            UnityContainer.RegisterFactory<T>(_ => factory(this));
            return this;
        }

        public IDependencyInjectionContainer RegisterType<TFrom, TTo>(bool overwriteRegistration = false) where TTo : TFrom
        {
            UnityContainer.RegisterType<TFrom, TTo>();
            return this;
        }

        public IDependencyInjectionContainer RegisterTypeSingleton<T>(bool overwriteRegistration = false)
        {
            UnityContainer.RegisterType<T>(new ContainerControlledLifetimeManager());
            return this;
        }

        public IDependencyInjectionContainer RegisterTypeSingleton<T>(Func<IDependencyInjectionContainer, T> factory, bool overwriteRegistration = false)
        {
            UnityContainer.RegisterFactory<T>(_ => factory(this), new ContainerControlledLifetimeManager());
            return this;
        }

        public IDependencyInjectionContainer RegisterTypeSingleton<TFrom, TTo>(bool overwriteRegistration = false) where TTo : TFrom
        {
            UnityContainer.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());
            return this;
        }

        public IDependencyInjectionContainer RegisterType(Type fromType, Type toType, bool overwriteRegistration = false)
        {
            UnityContainer.RegisterType(fromType, toType);
            return this;
        }

        public IDependencyInjectionContainer RegisterTypeSingleton(Type fromType, Type toType, bool overwriteRegistration = false)
        {
            UnityContainer.RegisterType(fromType, toType, new ContainerControlledLifetimeManager());
            return this;
        }

        public bool HasRegistrationFor<T>()
        {
            var requestedType = typeof(T);

            return HasRegistrationFor(requestedType);
        }

        public bool HasRegistrationFor(Type requestedType)
        {
            return RegisteredTypes.Any(t => t == requestedType);
        }

        public IDependencyInjectionContainer RegisterApplicationDependencies(
            IDependencyInjectionModule dependencyInjectionModule)
        {
            dependencyInjectionModule.RegisterDependencies(this);
            return this;
        }
    }
}
