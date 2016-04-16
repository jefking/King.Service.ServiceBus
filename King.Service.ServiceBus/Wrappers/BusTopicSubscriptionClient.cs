namespace King.Service.ServiceBus.Wrappers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Bus Topic Subscription Client
    /// </summary>
    public class BusTopicSubscriptionClient : IBusEventReciever
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
        public BusTopicSubscriptionClient(SubscriptionClient client)
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
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="options"></param>
        public void RegisterForEvents(Func<BrokeredMessage, Task> callback, OnMessageOptions options)
        {
            this.client.OnMessageAsync(callback, options);
        }
        #endregion
    }
}