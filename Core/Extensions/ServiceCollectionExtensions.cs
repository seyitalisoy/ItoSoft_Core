﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencyResolvers
            (this IServiceCollection serviceCollection, params ICoreModule[] modules)
        {
            foreach (var module in modules)
            {
                module.Load(serviceCollection);
            }
            return ServiceTool.Create(serviceCollection);
        }
    }
    //public static class ServiceCollectionExtensions
    //{
    //    public static IServiceCollection AddDependencyResolvers(this IServiceCollection services,
    //        ICoreModule[] modules)
    //    {
    //        foreach (var module in modules)
    //        {
    //            module.Load(services);
    //        }

    //        return ServiceTool.Create(services);
    //    }
    //}
}
