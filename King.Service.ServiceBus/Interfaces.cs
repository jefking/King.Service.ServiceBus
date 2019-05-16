namespace King.Service.ServiceBus
{
    using global::Azure.Data.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    #region IBusManagementClient
    /// <summary>
    /// Service Bus Management Client Interface
    /// </summary>
    public interface IBusManagementClient
    {
        #region Methods
        Task QueueCreate(string queue);
        Task<bool> QueueExists(string queue);
        Task TopicCreate(string topicPath);
        Task<bool> TopicExists(string topicPath);
        Task SubscriptionCreate(string topicPath, string subscriptionName);
        Task<bool> SubscriptionExists(string topicPath, string subscriptionName);
        Task CreateRuleAsync(string topicPath, string subscriptionName, string name, Filter filter);
        Task DeleteRuleAsync(string topicPath, string subscriptionName, string name);
        Task<RuleDescription> GetRuleAsync(string topicPath, string subscriptionName, string name);
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
        Task SendBuffered(object data, DateTime releaseAt, sbyte offset = BusMessageSender.BufferedOffset);
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


    #region IBusClient
    /// <summary>
    /// Service Bus Client
    /// </summary>
    public interface IBusClient<T>
    {
        #region Properties
        /// <summary>
        /// Queue Client
        /// </summary>
        T Client
        {
            get;
        }
        #endregion
    }
    #endregion

    #region IBusTopicClient
    /// <summary>
    /// Bus Topic Client Interface
    /// </summary>
    public interface IBusTopicClient : IBusClient<ITopicClient>, IMessageSender
    {
    }
    #endregion

    #region IBusQueueClient
    /// <summary>
    /// Bus Queue Client Wrapper
    /// </summary>
    public interface IBusQueueClient : IBusClient<IQueueClient>, IMessageSender, IBusReciever
    {
    }
    #endregion

    #region IBusSender
    /// <summary>
    /// Service Bus Sender
    /// </summary>
    public interface IBusSender<T>
    {
        #region Methods
        /// <summary>
        /// Send
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        Task Send(T message);

        /// <summary>
        /// Send Batch
        /// </summary>
        /// <param name="message">Messages</param>
        /// <returns>Task</returns>
        Task Send(IEnumerable<T> messages);
        #endregion
    }
    #endregion

    #region IBusSender
    /// <summary>
    /// Service Bus Sender
    /// </summary>
    public interface IMessageSender : IBusSender<Message>
    {
    }
    #endregion

    #region IBusReciever
    /// <summary>
    /// Service Bus Message Reciever
    /// </summary>
    public interface IBusReciever
    {
        #region Methods
        /// <summary>
        /// On Message
        /// </summary>
        /// <param name="callback">Call Back</param>
        /// <param name="options">Options</param>
        void OnMessage(Func<Message, CancellationToken, Task> callback, MessageHandlerOptions options);
        #endregion
    }
    #endregion

    #region ISubscription
    /// <summary>
    /// Service Bus Topic Subscription
    /// </summary>
    public interface ISubscription : IBusReciever
    {
        #region Methods
        /// <summary>
        /// Add Rule
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="filter">Filter</param>
        Task AddRule(string name, Filter filter);
        #endregion
    }
    #endregion

}