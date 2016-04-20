namespace King.Service.ServiceBus
{
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Transient Error Handler
    /// </summary>
    public class TransientErrorHandler : ITransientErrorHandler, IDisposable
    {
        #region Events
        /// <summary>
        /// Transient Error Event
        /// </summary>
        public event TransientErrorEventHandler TransientErrorOccured;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public TransientErrorHandler() { }


        /// <summary>
        /// Finalizer
        /// </summary>
        ~TransientErrorHandler()
        {
            this.Dispose(false);
        }
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

        /// <summary>
        /// Dispose
        /// </summary>
        public virtual void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">Disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.TransientErrorOccured = null;
            }
        }
        #endregion
    }
}