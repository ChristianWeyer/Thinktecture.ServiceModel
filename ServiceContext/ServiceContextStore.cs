/*
   Copyright (c) 2002-2010, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.ServiceModel;
using System.Web;
using System.Web.Caching;

namespace Thinktecture.ServiceModel
{
    /// <summary>
    /// Custom memory-based store for storing data as key-value pairs within the 
    /// current execution context.
    /// </summary>
    public class ServiceContextStore
    {
        private Cache dataStore;
        private static object lockObject = new object();
        private static ServiceContextStore sc = new ServiceContextStore();

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceContextStore"/> class.
        /// </summary>
        private ServiceContextStore()
        {
            if (dataStore == null)
                dataStore = HttpRuntime.Cache;
        }

        /// <summary>
        /// Gets the number of items in the store.
        /// </summary>
        public int Count
        {
            get { return dataStore.Count; }
        }

        /// <summary>
        /// Gets the <see cref="ServiceContextStore"/> for the current context.
        /// </summary>
        public static ServiceContextStore Current
        {         
            get
            {
                return sc;            
            }
        }

        /// <summary>
        /// Gets the value at the specified key.
        /// </summary>
        public T Get<T>(string key)
        {
            return (T)dataStore[key];
        }

        /// <summary>
        /// Sets the value at the specified key.
        /// </summary>
        public void Set<T>(string key, T value)
        {
            dataStore[key] = value;
        }

        /// <summary>
        /// Adds the specified key-value pair to the store.
        /// </summary>       
        public object Add<T>(string key, T value)
        {
            return dataStore.Add(
                key, value,
                null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration,
                CacheItemPriority.Default, null);
        }

        /// <summary>
        /// Removes the item specified by the key from the store.
        /// </summary>
        public object Remove(string key)
        {
            return dataStore.Remove(key);
        }
    }
}