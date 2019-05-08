namespace King.Service.ServiceBus.Wrappers
{
    using Microsoft.Azure.ServiceBus;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

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
        void OnMessage(Func<IMessageSession, Message, CancellationToken, Task> callback, SessionHandlerOptions options);
        #endregion
    }
    #endregion
}