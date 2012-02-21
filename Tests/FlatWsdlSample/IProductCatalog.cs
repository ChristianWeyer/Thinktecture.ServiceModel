/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.Collections.Generic;
using System.ServiceModel;

namespace Thinktecture.ServiceModel.Samples.FlatWsdlSample
{
    [ServiceContract(Namespace="http://www.thinktecture.com/samples/services/productcatalog",
        ConfigurationName="IProductCatalog")]
    public interface IProductCatalog
    {
        [OperationContract]
        IList<Product> GetCatalog();
    }
}