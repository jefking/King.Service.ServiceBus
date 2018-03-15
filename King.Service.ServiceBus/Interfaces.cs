namespace King.Service.ServiceBus
{
    using global::Azure.Data.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    #region IBusQueueReciever
    /// <summary>
    /// Bus Queue Reciever Interface
    /// </summary>
    public interface IBusMessageReciever
    {
        #region Methods
        /// <summary>
        /// Register for Events
        /// </summary>
        /// <param name="callback">Callback</param>
        /// <param name="options">Options</param>
        void RegisterForEvents(Func<Message, Task> callback, MessageHandlerOptions options);
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
        Task Send(Message message);

        /// <summary>
        /// Send Message to Queue
        /// </summary>
        /// <param name="messages">Messages</param>
        /// <returns>Task</returns>
        Task Send(IEnumerable<Message> messages);

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
        /// <returns>Task</returns>
        Task OnError(string action, Exception ex);
        #endregion
    }
    #endregion
    
    #region IBusQueueShardSender
    /// <summary>
    /// Bus Queue Shards Interface
    /// </summary>
    public interface IBusShardSender
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
}