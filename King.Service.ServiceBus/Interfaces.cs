namespace King.Service.ServiceBus
{
    using King.Azure.Data;
    using System;
    
    public interface IBusDequeueBatch<T>
    {

    }

    public interface IBusDequeue<T>
    {

    }

    public interface IBusEventHandler<T> : IProcessor<T>
    {
        #region Methods
        /// <summary>
        /// On Error
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="ex">Exception</param>
        void OnError(string action, Exception ex);
        #endregion
    }
}