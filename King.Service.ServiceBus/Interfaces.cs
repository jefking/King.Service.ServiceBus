﻿namespace King.Service.ServiceBus
{
    using global::Azure.Data.Wrappers;
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
    public interface IBusQueue : IQueueCount, IAzureStorage
    {
        #region Properties
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
        /// Lock Duration
        /// </summary>
        /// <returns>Lock Duration</returns>
        Task<TimeSpan> LockDuration();

        /// <summary>
        /// Description
        /// </summary>
        /// <returns>Queue Description</returns>
        Task<QueueDescription> Description();
        #endregion
    }
    #endregion

    #region IBusQueueReciever
    /// <summary>
    /// Bus Queue Reciever Interface
    /// </summary>
    public interface IBusMessageReciever
    {
        #region Methods
        /// <summary>
        /// Get Cloud Queue Message
        /// </summary>
        /// <param name="waitTime">Wait Time</param>
        /// <returns>Message</returns>
        Task<BrokeredMessage> Get(TimeSpan waitTime);

        /// <summary>
        /// Get Many Cloud Queue Message
        /// </summary>
        /// <param name="waitTime">Wait Time</param>
        /// <param name="messageCount">Message Count</param>
        /// <returns>Messages</returns>
        Task<IEnumerable<BrokeredMessage>> GetMany(TimeSpan waitTime, int messageCount = 5);

        /// <summary>
        /// Register for Events
        /// </summary>
        /// <param name="callback">Callback</param>
        /// <param name="options">Options</param>
        void RegisterForEvents(Func<BrokeredMessage, Task> callback, OnMessageOptions options);
        #endregion

        #region Properties
        /// <summary>
        /// Server Wait Time
        /// </summary>
        TimeSpan ServerWaitTime
        {
            get;
        }
        #endregion
    }
    #endregion

    #region IBusMessageSender
    /// <summary>
    /// Service Bus Message Sender Interface
    /// </summary>
    public interface IBusMessageSender : IQueueObject
    {
        #region Methods
        /// <summary>
        /// Send Message to Queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        Task Send(BrokeredMessage message);

        /// <summary>
        /// Send Message to Queue
        /// </summary>
        /// <param name="messages">Messages</param>
        /// <returns>Task</returns>
        Task Send(IEnumerable<BrokeredMessage> messages);

        /// <summary>
        /// Send Object to queue, as json
        /// </summary>
        /// <param name="messages">Messages</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Task</returns>
        Task Send(IEnumerable<object> messages, Encoding encoding = Encoding.Json);

        /// <summary>
        /// Send Message with Retry
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="enqueueAt">Schedule for Enqueue</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Task</returns>
        Task Send(object message, DateTime enqueueAt, Encoding encoding = Encoding.Json);

        /// <summary>
        /// Send Message to Queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Task</returns>
        Task Send(object message, Encoding encoding = Encoding.Json);

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
    public interface IBusTopicSender : ITransientErrorHandler
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

    #region IBusQueueShardSender
    /// <summary>
    /// Bus Queue Shards Interface
    /// </summary>
    public interface IBusShardSender : IAzureStorage
    {
        #region Properties
        /// <summary>
        /// Shard Count
        /// </summary>
        byte ShardCount
        {
            get;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Queue Message to shard, 0 means at random
        /// </summary>
        /// <param name="obj">message</param>
        /// <param name="shardTarget">Shard Target</param>
        /// <returns>Task</returns>
        Task Save(object obj, byte shardTarget = 0);

        /// <summary>
        /// Determine index of queues to interact with
        /// </summary>
        /// <remarks>
        /// Specifically broken out for testing safety
        /// </remarks>
        /// <param name="shardTarget">Shard Target</param>
        /// <returns>Index</returns>
        byte Index(byte shardTarget);
        #endregion
    }
    #endregion

    #region IBusShard
    /// <summary>
    /// Service Bus Shard
    /// </summary>
    public interface IBusShard
    {
        #region Properties
        /// <summary>
        /// Resource
        /// </summary>
        IAzureStorage Resource
        {
            get;
        }

        /// <summary>
        /// Sender
        /// </summary>
        IQueueObject Sender
        {
            get;
        }
        #endregion
    }
    #endregion
}