using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Helpers;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Core.Utilities.Results;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheAspect : MethodInterception
    {
        private int _duration;
        private ICacheManager _cacheManager;

        public CacheAspect(int duration = 60)
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        public override void Intercept(IInvocation invocation)
        {
            var methodName = $"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}";
            var arguments = invocation.Arguments.ToList();
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";

            var returnType = invocation.Method.ReturnType;
            var genericArg = returnType.GetGenericArguments().First();

            if (_cacheManager.IsAdd(key))
            {
                var serializableType = typeof(SerializableDataResult<>).MakeGenericType(genericArg);

                var cachedData = typeof(ICacheManager)
                    .GetMethod("Get")
                    .MakeGenericMethod(serializableType)
                    .Invoke(_cacheManager, new object[] { key });

                var dataProp = serializableType.GetProperty("Data")?.GetValue(cachedData);
                var messageProp = serializableType.GetProperty("Message")?.GetValue(cachedData);
                var successProp = serializableType.GetProperty("Success")?.GetValue(cachedData);

                var dataResultType = typeof(DataResult<>).MakeGenericType(genericArg);
                var returnValue = Activator.CreateInstance(dataResultType, dataProp, (bool)successProp, (string)messageProp);

                invocation.ReturnValue = returnValue;
                return;
            }

            invocation.Proceed();

            var serializable = CacheHelper.MapToSerializable(invocation.ReturnValue);
            _cacheManager.Set(key, serializable, _duration);
        }
    }
}
