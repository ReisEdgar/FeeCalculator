using Logic.Mappers;
using Logic;
using System;
using FileRepository;
using System.Collections.Generic;
using System.Reflection;
using Presentation.Mappers;
using Presentation;

namespace Infrastructure
{
    public static class DependencyInjectionService
    {
        public static void RegisterDependencies()
        {
            Register<ITransactionRepository, TransactionRepository>();
            Register<ITransactionFeeService, TransactionFeeService>();
            Register<ITransactionDiscountService, TransactionDiscountService>();
            Register<ITransactionMapper, TransactionMapper>();
            Register<ITransactionFeeMapper, TransactionFeeMapper>();
            Register<ITransactionFeeDisplay, TransactionFeeDisplay>();

        }
        static readonly IDictionary<Type, Type> types = new Dictionary<Type, Type>();

        public static void Register<TContract, TImplementation>()
        {
            types[typeof(TContract)] = typeof(TImplementation);
        }

        public static T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public static object Resolve(Type contract)
        {
            Type implementation = types[contract];
            ConstructorInfo constructor = implementation.GetConstructors()[0];
            ParameterInfo[] constructorParameters = constructor.GetParameters();
            if (constructorParameters.Length == 0)
            {
                return Activator.CreateInstance(implementation);
            }

            List<object> parameters = new List<object>(constructorParameters.Length);
            foreach (ParameterInfo parameterInfo in constructorParameters)
            {
                parameters.Add(Resolve(parameterInfo.ParameterType));
            }

            return constructor.Invoke(parameters.ToArray());
        }
    }

}

