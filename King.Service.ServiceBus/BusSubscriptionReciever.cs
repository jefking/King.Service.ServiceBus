namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;

    /// <summary>
    /// Bus Subscription Reciever
    /// </summary>
    public class BusSubscriptionReciever : BusMessageReciever
    {
        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BusSubscriptionReciever(string topicName, string connection, string subscriptionName, ReceiveMode mode = ReceiveMode.PeekLock)
            : base(new BusSubscriptionClient(new SubscriptionClient(connection, topicName, subscriptionName, mode)))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="client">Bus Subscription Client</param>
        public BusSubscriptionReciever(IBusReciever client)
            : base(client)
        {
        }
        #endregion
    }
}