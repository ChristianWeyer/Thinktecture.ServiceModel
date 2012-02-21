/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.Reflection;
using System.Resources;

namespace Thinktecture.ServiceModel
{
    /// <summary>
    /// 
    /// </summary>
    internal class ResourceHelper
    {
        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            ResourceManager resourceManager = new ResourceManager(
                "Thinktecture.ServiceModel.Properties.Resources",
                Assembly.GetExecutingAssembly());

            return resourceManager.GetString(key);
        }
    }
}
