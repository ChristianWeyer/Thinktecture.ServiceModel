/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Thinktecture.ServiceModel.Samples.FlatWsdlSample
{
    [ServiceBehavior(Namespace="http://www.thinktecture.com/samples/services/productcatalog",
        ConfigurationName="ProductCatalogService")]
    public class ProductCatalog : IProductCatalog
    {
        public IList<Product> GetCatalog()
        {
            throw new Exception("This method is not implmeneted.");
        }
    }
}