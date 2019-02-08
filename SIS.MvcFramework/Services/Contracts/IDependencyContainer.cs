namespace SIS.MvcFramework.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IDependencyContainer
    {
        void RegisterDependency<TSource, TDestination>();

        T CreateInstance<T>();

        object CreateInstance(Type type);
    }
}
