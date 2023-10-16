namespace Litmus.Core.DependencyInjection
{
    public interface IDependencyInjectionModule
    {
        void RegisterDependencies(IDependencyInjectionContainer container);
    }
}
