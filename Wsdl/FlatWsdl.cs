/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Xml.Schema;
using Thinktecture.ServiceModel.Properties;
using ServiceDescription = System.Web.Services.Description.ServiceDescription;

namespace Thinktecture.ServiceModel.Description
{
    /// <summary>
    /// Custom endpoint behavior to enable flattened WSDL emmision. Use <see cref="ServiceHost"/>
    /// to automatically apply this behavior to all endpoints in the host.
    /// </summary>
    public class FlatWsdl : IWsdlExportExtension, IEndpointBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlatWsdl"/> class.
        /// </summary>
        public FlatWsdl()
        {
        }

        /// <summary>
        /// Passes data at runtime to bindings to support custom behavior.
        /// </summary>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            // no-op
        }

        /// <summary>
        /// Implements a modification or extension of the client across an endpoint.
        /// </summary>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            // no-op
        }

        /// <summary>
        /// Implements a modification or extension of the service across an endpoint.
        /// </summary>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // no-op
        }

        /// <summary>
        /// Confirms that the endpoint meets some intended criteria.
        /// </summary>        
        public void Validate(ServiceEndpoint endpoint)
        {
            // no-op
        }

        /// <summary>
        /// Writes custom Web Services Description Language (WSDL) elements into the generated WSDL for a contract.
        /// </summary>
        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
            // no -op
        }

        /// <summary>
        /// Writes custom Web Services Description Language (WSDL) elements into 
        /// the generated WSDL for an endpoint.
        /// </summary>
        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            // We don't support more than one WSDL.
            if (exporter.GeneratedWsdlDocuments.Count > 1)
            {
                Trace.TraceError(Resources.ExInconsistantXmlNamespaces);
                throw new InvalidOperationException(Resources.ExInconsistantXmlNamespaces);
            }

            ServiceDescription wsdl =
                exporter.GeneratedWsdlDocuments[0];
            XmlSchemaSet schemaSet =
                exporter.GeneratedXmlSchemas;
            Collection<XmlSchema> importsList = new Collection<XmlSchema>();

            for (int i = 0; i < wsdl.Types.Schemas.Count; i++)
            {
                XmlSchema schema = wsdl.Types.Schemas[i];
                ResolveImportedSchemas(schema, schemaSet, importsList);

                // If we don't have anything else (e.g. inlined types) 
                // in this schema, we can remove it.
                if (schema.Includes.Count == 0 && schema.Items.Count == 0)
                {
                    wsdl.Types.Schemas.RemoveAt(i--);
                }
            }

            // Finally, add each of the real schemas we extracted in the above step.            
            while(importsList.Count != 0)
            {
                int l = importsList.Count - 1;
                wsdl.Types.Schemas.Add(importsList[l]);
                importsList.RemoveAt(l);
            }
        }

        /// <summary>
        /// This method enumarates the schema imports in schema and adds the actual
        /// schemas resolved from schemaSet to importsList.
        /// </summary>
        private void ResolveImportedSchemas(XmlSchema schema, 
            XmlSchemaSet schemaSet, Collection<XmlSchema> importsList)
        {
            for (int i = 0; i < schema.Includes.Count; i++)
            {
                // Do we have a reference to an XmlSchemaImport?
                XmlSchemaImport import = schema.Includes[i] as XmlSchemaImport;

                if (import != null)
                {
                    // Get the real schemas belonging to the imported schema namespace.
                    ICollection realSchemas = schemaSet.Schemas(import.Namespace);

                    // Add the real schemas to the list.
                    foreach (XmlSchema ixsd in realSchemas)
                    {                       
                        // Check whether we already got it in the list...
                        if (!importsList.Contains(ixsd))
                        {
                            importsList.Add(ixsd);

                            // Process the real schemas recursively and remove the imports in them. 
                            // I really don't think that there would be a schema with 2000+ nested schemas...
                            // If this is the case, we will need to change this code to use a queue.                           
                            ResolveImportedSchemas(ixsd, schemaSet, importsList);
                        }
                    }

                    // Finally, remove the import and reset the counter.
                    schema.Includes.RemoveAt(i--);
                }
            }
        }
    }
}