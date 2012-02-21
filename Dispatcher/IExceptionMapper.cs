using System;

namespace Thinktecture.ServiceModel.Dispatcher
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExceptionMapper
    {
        /// <summary>
        /// Maps to fault.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        object MapToFault(Exception exception);
    }
}
