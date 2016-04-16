namespace King.Service.ServiceBus.Wrappers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Bus Subscription Client
    /// </summary>
    public class BusSubscriptionClient : IBusSubscriptionClient
    {
        #region Members
        /// <summary>
        /// Subscription Client
        /// </summary>
        protected readonly SubscriptionClient client = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Queue Client</param>
        public BusSubscriptionClient(SubscriptionClient client)
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
        /// On Message Async
        /// </summary>
        /// <param name="callback">Call Back</param>
        /// <param name="options">Options</param>
        public void OnMessageAsync(Func<BrokeredMessage, Task> callback, OnMessageOptions options)
        {
            this.client.OnMessageAsync(callback, options);
        }
        #endregion
    }
}