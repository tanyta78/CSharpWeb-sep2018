namespace SIS.MvcFramework.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ServiceCollection : IServiceCollection
    {
        private readonly IDictionary<Type, Type> dependencyContainer;
        private readonly IDictionary<Type, Func<object>> dependencyContainerWithFunc;

        public ServiceCollection()
        {
            this.dependencyContainerWithFunc = new Dictionary<Type, Func<object>>();
            this.dependencyContainer = new Dictionary<Type, Type>();
        }

        public void AddService<TSource, TDestination>()
        {
            this.dependencyContainer[typeof(TSource)] = typeof(TDestination);
        }

        public void AddService<T>(Func<T> p)
        {
            this.dependencyContainerWithFunc[typeof(T)] = () => p();
        }

        public T CreateInstance<T>()
        {
            return (T)this.CreateInstance(typeof(T));
        }

        public object CreateInstance(Type type)
        {
            //1.Get type - If  this.dependencyContainer[typeof(T)] => get type from dict, else get it from T
            if (this.dependencyContainerWithFunc.ContainsKey(type))
            {
                return this.dependencyContainerWithFunc[type]();
            }
            if (this.dependencyContainer.ContainsKey(type))
            {
                type = this.dependencyContainer[type];
            }

            if (type.IsAbstract || type.IsInterface)
            {
                throw new Exception($"Type {type.FullName} can not be instantiated");
            }
            //2.Create instance of type

            // if empty-> use it
            var constructorInfo = type.GetConstructors().OrderBy(x => x.GetParameters().Length).First();
            var constructorParameters = constructorInfo.GetParameters();
            var constructorParameterObjects = new List<object>();

            foreach (var constructorParameter in constructorParameters)
            {
                var parameterObject = this.CreateInstance(constructorParameter.ParameterType);
                constructorParameterObjects.Add(parameterObject);

            }

            var obj = constructorInfo.Invoke(constructorParameterObjects.ToArray());
            return obj;
        }
    }
}