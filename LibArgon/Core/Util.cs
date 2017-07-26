using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtificialNature
{
    class Singleton<T> where T : new()
    {
        private static T s_instance = default(T);

        protected Singleton() { }

        static T GetInstance()
        {
            if (s_instance == null)
            {
                s_instance = new T();
            }

            return s_instance;
        }
    }

    class Cache<T>
    {
        Dictionary<string, T> cache = new Dictionary<string, T>();

        public virtual T Get(string name)
        {
            if (cache.ContainsKey(name))
            {
                return cache[name];
            }
            else
            {
                return default(T);
            }
        }
        public virtual bool Set(string name, T t)
        {
            if (cache.ContainsKey(name))
            {
                return false;
            }
            else
            {
                cache[name] = t;
                return true;
            }
        }
    }
}
