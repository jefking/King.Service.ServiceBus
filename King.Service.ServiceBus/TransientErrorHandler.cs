namespace King.Service.ServiceBus
{
    using System.Diagnostics;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Transient Error Handler
    /// </summary>
    public class TransientErrorHandler : ITransientErrorHandler
    {
        #region Events
        /// <summary>
        /// Transient Error Event
        /// </summary>
        public event TransientErrorEventHandler TransientErrorOccured;
        #endregion

        #region Methods
        /// <summary>
        /// Handle Transient Error
        /// </summary>
        /// <param name="ex">Messaging Exception</param>
        public virtual void HandleTransientError(MessagingException ex)
        {
            if (null != ex)
            {
                var handle = this.TransientErrorOccured;
                if (null != handle)
                {
                    var arg = new TransientErrorArgs
                    {
                        Exception = ex,
                    };
                    handle(this, arg);
                }

                Trace.TraceWarning("Transient Error: '{0}'", ex.ToString());
            }
        }
        #endregion
    }
}