﻿namespace King.Service.ServiceBus
{
    using King.Azure.Data;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus;
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
        #region Properties
        /// <summary>
        /// Queue Name
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Queue Client
        /// </summary>
        IBusQueueClient Client
        {
            get;
        }

        /// <summary>
        /// Namespace Manager
        /// </summary>
        NamespaceManager Manager
        {
            get;
        }
        #endregion

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
        /// Approixmate Message Count
        /// </summary>
        /// <returns>Message Count</returns>
        Task<long> ApproixmateMessageCount();
        #endregion
    }
    #endregion

    #region IBusQueueReciever
    /// <summary>
    /// Bus Queue Reciever Interface
    /// </summary>
    public interface IBusQueueReciever : IBusQueue
    {
        #region Methods
        /// <summary>
        /// Get Cloud Queue Message
        /// </summary>
        /// <returns>Message</returns>
        Task<BrokeredMessage> Get();

        /// <summary>
        /// Get Many Cloud Queue Message
        /// </summary>
        /// <returns>Messages</returns>
        Task<IEnumerable<BrokeredMessage>> GetMany(int messageCount = 5);

        /// <summary>
        /// Register for Events
        /// </summary>
        /// <param name="callback">Callback</param>
        /// <param name="options">Options</param>
        void RegisterForEvents(Func<BrokeredMessage, Task> callback, OnMessageOptions options);
        #endregion
    }
    #endregion

    #region IBusQueueSender
    /// <summary>
    /// Bus Queue Sender Interface
    /// </summary>
    public interface IBusQueueSender : IBusQueue
    {
        #region Methods
        /// <summary>
        /// Save Message to Queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        Task Send(BrokeredMessage message);

        /// <summary>
        /// Save Object to queue, as json
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>Task</returns>
        Task Send(object obj);

        /// <summary>
        /// Send Message with Retry
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="enqueueAt">Schedule for Enqueue</param>
        /// <returns>Task</returns>
        Task Send(object message, DateTime enqueueAt);

        /// <summary>
        /// Send Message for Buffer
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="enqueueAt">Schedule for Enqueue</param>
        /// <param name="offset">Offset</param>
        /// <returns>Task</returns>
        Task SendBuffered(object data, DateTime releaseAt, sbyte offset = BusQueueSender.BufferedOffset);
        #endregion
    }
    #endregion

    #region IBusEventHandler
    /// <summary>
    /// Service Bus Queue Event Handler Interface
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

    #region ITopicSender
    /// <summary>
    /// Topic Sender
    /// </summary>
    public interface ITopicSender
    {
        #region Methods
        /// <summary>
        /// Save Message to Queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        Task Send(BrokeredMessage message);

        /// <summary>
        /// Save Object to queue, as json
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>Task</returns>
        Task Send(object obj);
        #endregion
    }
    #endregion

    #region ITransientErrorHandler
    /// <summary>
    /// Transient Error Handler Interface
    /// </summary>
    public interface ITransientErrorHandler
    {
        #region Events
        /// <summary>
        /// Transient Error Event
        /// </summary>
        event TransientErrorEventHandler TransientErrorOccured;
        #endregion
    }
    #endregion
}