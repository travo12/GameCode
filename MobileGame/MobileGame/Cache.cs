using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using System.Reactive.Linq;

namespace MobileGame
{
    class Cache
    {
        public Cache()
        {
			
            //BlobCache.ApplicationName = "LeeGame";
        }
        public async Task RemoveObject(string key)
        {
            await BlobCache.LocalMachine.Invalidate(key);
        }
        public async Task<T> GetObject<T>(string key)
        {
            try
            {
                return await BlobCache.LocalMachine.GetObject<T>(key);
            }
            catch (KeyNotFoundException)
            {
                return default(T);
            }
        }
        public async Task InsertObject<T>(string key, T value)
        {
            await BlobCache.LocalMachine.InsertObject(key, value);
        }
        
        public async Task<T> GetOrFetch<T>(string key, Func<Task<T>> funct)
        {
            return await BlobCache.LocalMachine.GetOrFetchObject<T>(key, funct);

        }
    }
}
