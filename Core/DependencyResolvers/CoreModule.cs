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
using Autofac.Core;
using System.Diagnostics;

namespace Core.DependencyResolvers
{
    public class CoreModule : ICoreModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddScoped<IHttpContextAccessor,HttpContextAccessor>();

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var config = sp.GetService<IConfiguration>();
                var redisConfiguration = config.GetSection("Redis:Configuration").Value;
                return ConnectionMultiplexer.Connect(redisConfiguration);
            });
            services.AddSingleton<ICacheManager, RedisCacheManager>();

            services.AddSingleton<Stopwatch>();
        }
    }
}
