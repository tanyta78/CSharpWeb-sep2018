﻿namespace SIS.MvcFramework.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ServiceCollection : IServiceCollection
    {
        private IDictionary<Type, Type> dependencyContainer;

        public ServiceCollection()
        {
            this.dependencyContainer = new Dictionary<Type, Type>();
        }

        public void AddService<TSource, TDestination>()
        {
            this.dependencyContainer[typeof(TSource)] = typeof(TDestination);
        }

        public T CreateInstance<T>()
        {
            return (T)CreateInstance(typeof(T));
        }

        public object CreateInstance(Type type)
        {
            //1.Get type - If  this.dependencyContainer[typeof(T)] => get type from dict, else get it from T
            if (this.dependencyContainer.ContainsKey(type))
            {
                type = this.dependencyContainer[type];
            }

            if (type.IsAbstract || type.IsInterface)
            {
                throw new Exception($"Type {type.FullName} can not be instantiated");
            }
            //2.Create instance of type

            //if empty-> use it
            var constructorInfo = type.GetConstructors().First();
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