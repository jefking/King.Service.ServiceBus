namespace King.Service.ServiceBus.Wrappers
{
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBusQueueClient
    {
        #region Methods
        Task Send(BrokeredMessage message);
        Task<BrokeredMessage> Receive(TimeSpan serverWaitTime);
        Task<IEnumerable<BrokeredMessage>> ReceiveBatch(int messageCount, TimeSpan serverWaitTime);
        void OnMessage(Func<BrokeredMessage, Task> callback, OnMessageOptions options);
        #endregion
    }
}