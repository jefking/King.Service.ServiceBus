namespace King.Service.ServiceBus.Wrappers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;

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
    public interface IBusTopicClient : IBusClient<TopicClient>, IBrokeredMessageSender
    {
    }
    #endregion

    #region IBusQueueClient
    /// <summary>
    /// Bus Queue Client Wrapper
    /// </summary>
    public interface IBusQueueClient : IBusClient<QueueClient>, IBrokeredMessageSender, IBusReciever
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
    public interface IBrokeredMessageSender : IBusSender<BrokeredMessage>
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
        /// Recieve
        /// </summary>
        /// <param name="serverWaitTime">Server Wait Time</param>
        /// <returns>Brokered Message</returns>
        Task<BrokeredMessage> Recieve(TimeSpan serverWaitTime);

        /// <summary>
        /// Recieve Batch
        /// </summary>
        /// <param name="messageCount">Message Count</param>
        /// <param name="serverWaitTime">Server Wait Time</param>
        /// <returns>Brokered Messages</returns>
        Task<IEnumerable<BrokeredMessage>> RecieveBatch(int messageCount, TimeSpan serverWaitTime);

        /// <summary>
        /// On Message
        /// </summary>
        /// <param name="callback">Call Back</param>
        /// <param name="options">Options</param>
        void OnMessage(Func<BrokeredMessage, Task> callback, OnMessageOptions options);
        #endregion
    }
    #endregion

    #region IHubClient
    /// <summary>
    /// Event Hub Client Interface
    /// </summary>
    public interface IHubClient : IBusClient<EventHubClient>, IBusSender<EventData>
    {
    }
    #endregion
}