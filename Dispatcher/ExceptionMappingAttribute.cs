/*
   Copyright (c) 2011, thinktecture (http://www.thinktecture.com).
   All rights reserved, comes as-is and without any warranty. Use of this
   source file is governed by the license which is contained in LICENSE.TXT 
   in the distribution.
*/

using System;
using System.Reflection;

namespace Thinktecture.ServiceModel.Dispatcher
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class ExceptionMappingAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the type of the exception.
        /// </summary>
        /// <value>The type of the exception.</value>
        internal Type ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets the type of the fault.
        /// </summary>
        /// <value>The type of the fault.</value>
        internal Type FaultType { get; set; }

        private ConstructorInfo exceptionConstructor;
        private ConstructorInfo parameterlessConstructor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMappingAttribute"/> class.
        /// </summary>
        /// <param name="exceptionType">Type of the exception.</param>
        /// <param name="faultDetailType">Type of the fault detail.</param>
        public ExceptionMappingAttribute(Type exceptionType,
            Type faultDetailType)
        {
            ExceptionType = exceptionType;
            FaultType = faultDetailType;

            exceptionConstructor = FaultType.GetConstructor(new Type[] { exceptionType });

            if (exceptionConstructor == null)
                parameterlessConstructor = FaultType.GetConstructor(Type.EmptyTypes);
        }

        /// <summary>
        /// Gets the fault detail for exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        internal object GetFaultDetailForException(Exception exception)
        {
            if (exceptionConstructor != null)
                return exceptionConstructor.Invoke(new object[] { exception });
            if (parameterlessConstructor != null)
                return parameterlessConstructor.Invoke(new object[] { });

            return null;
        }
    }
}
