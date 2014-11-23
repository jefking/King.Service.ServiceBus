namespace King.Service.ServiceBus.Wrappers
{
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Bus Queue Client Wrapper
    /// </summary>
    public interface IBusQueueClient
    {
        #region Methods
        /// <summary>
        /// Send
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        Task Send(BrokeredMessage message);

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
}