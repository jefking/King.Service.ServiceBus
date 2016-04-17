namespace King.Service.ServiceBus
{
    using System;
    using System.Threading.Tasks;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Bus Subscription Reciever
    /// </summary>
    public class BusSubscriptionReciever : IBusEventReciever
    {
        #region Members
        /// <summary>
        /// Service Bus Subscription Client
        /// </summary>
        protected readonly IBusReciever client;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BusSubscriptionReciever(string topicName, string connection, string subscriptionName, ReceiveMode mode = ReceiveMode.PeekLock)
            : this(new BusSubscriptionClient(SubscriptionClient.CreateFromConnectionString(connection, topicName, subscriptionName, mode)))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="client">Bus Subscription Client</param>
        public BusSubscriptionReciever(IBusReciever client)
        {
            if (null == client)
            {
                throw new ArgumentNullException("client");
            }

            this.client = client;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Register for Events
        /// </summary>
        /// <param name="callback">Call Back</param>
        /// <param name="options">Options</param>
        public void RegisterForEvents(Func<BrokeredMessage, Task> callback, OnMessageOptions options)
        {
            this.client.OnMessage(callback, options);
        }
        #endregion
    }
}