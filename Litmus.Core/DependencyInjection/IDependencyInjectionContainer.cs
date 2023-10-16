using System;
using System.Collections.Generic;

namespace Litmus.Core.DependencyInjection
{
    public interface IDependencyInjectionContainer
    {
        T Resolve<T>();
        object Resolve(Type type);
        IEnumerable<object> ResolveAll(Type type);

        IDependencyInjectionContainer RegisterInstance<T>(T instance, bool overwriteRegistration = false);
        IDependencyInjectionContainer RegisterType<T>(bool overwriteRegistration = false);
        IDependencyInjectionContainer RegisterType<T>(Func<IDependencyInjectionContainer, T> factory, bool overwriteRegistration = false);
        IDependencyInjectionContainer RegisterTypeSingleton<T>(bool overwriteRegistration = false);
        IDependencyInjectionContainer RegisterTypeSingleton<T>(Func<IDependencyInjectionContainer, T> factory, bool overwriteRegistration = false);
        IDependencyInjectionContainer RegisterType<TFrom, TTo>(bool overwriteRegistration = false) where TTo : TFrom;
        IDependencyInjectionContainer RegisterTypeSingleton<TFrom, TTo>(bool overwriteRegistration = false) where TTo : TFrom;
        IDependencyInjectionContainer RegisterType(Type fromType, Type toType, bool overwriteRegistration = false);
        IDependencyInjectionContainer RegisterTypeSingleton(Type fromType, Type toType, bool overwriteRegistration = false);

        IEnumerable<Type> RegisteredTypes { get; }
        IEnumerable<Type> MappedTypes { get; }
        bool HasRegistrationFor<T>();
        bool HasRegistrationFor(Type requestedType);
    }
}
