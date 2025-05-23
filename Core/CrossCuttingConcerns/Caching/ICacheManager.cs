﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching
{
    public interface ICacheManager
    {
        void Set(string key, object value, int duration);
        T Get<T>(string key);
        bool IsAdd(string key);
        void Remove(string key);
        void RemoveByPattern(string key);

    }
}
