namespace King.Service.ServiceBus
{
    using King.Azure.Data;
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    #region IBusQueue
    /// <summary>
    /// Bus Queue Interface
    /// </summary>
    public interface IBusQueue
    {

        #region Methods
        /// <summary>
        /// Create If Not Exists
        /// </summary>
        /// <returns></returns>
        Task<bool> CreateIfNotExists();

        /// <summary>
        /// Delete Queue
        /// </summary>
        /// <returns></returns>
        Task Delete();

        /// <summary>
        /// Get Cloud Queue Message
        /// </summary>
        /// <returns>Message</returns>
        Task<BrokeredMessage> Get();

        /// <summary>
        /// Approixmate Message Count
        /// </summary>
        /// <returns>Message Count</returns>
        Task<long> ApproixmateMessageCount();

        /// <summary>
        /// Get Many Cloud Queue Message
        /// </summary>
        /// <returns>Messages</returns>
        Task<IEnumerable<BrokeredMessage>> GetMany(int messageCount = 5);

        /// <summary>
        /// Save Message to Queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        Task Save(BrokeredMessage message);

        /// <summary>
        /// Save Object to queue, as json
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>Task</returns>
        Task Save(object obj);
        #endregion
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