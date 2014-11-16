namespace King.Service.ServiceBus
{
    using King.Azure.Data;
    using System;

    #region IBusQueue
    /// <summary>
    /// 
    /// </summary>
    public interface IBusQueue
    {

    }
    #endregion

    #region IBusEventHandler
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
    #endregion
}