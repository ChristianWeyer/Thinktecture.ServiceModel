/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System.Runtime.Serialization;

namespace Thinktecture.ServiceModel.Samples.FlatWsdlSample
{
    [DataContract(Namespace="http://www.thinktecture.com/samples/services/productcatalog/data")]
    public class Product
    {
        [DataMember] 
        private string description;
        [DataMember]
        private string name;
        [DataMember] 
        private string sku;
        [DataMember] 
        private double unitPrice;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Sku
        {
            get { return sku; }
            set { sku = value; }
        }

        public double UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }
    }
}