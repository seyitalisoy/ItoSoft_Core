using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CrossCuttingConcerns.Caching.Redis;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IHttpContextAccessor,HttpContextAccessor>();

            serviceCollection.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var config = sp.GetService<IConfiguration>();
                var redisConfiguration = config.GetSection("Redis:Configuration").Value;
                return ConnectionMultiplexer.Connect(redisConfiguration);
            });
            serviceCollection.AddSingleton<ICacheManager, RedisCacheManager>();
        }
    }
}
