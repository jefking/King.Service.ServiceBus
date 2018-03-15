namespace King.Service.ServiceBus.Wrappers
{
    using Microsoft.Azure.ServiceBus;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Bus Subscription Client
    /// </summary>
    public class BusSubscriptionClient : IBusReciever
    {
        #region Members
        /// <summary>
        /// Subscription Client
        /// </summary>
        protected readonly ISubscriptionClient client = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="topicPath">Topic Name</param>
        /// <param name="connection">Connection String</param>
        /// <param name="name">Subscription Name</param>
        public BusSubscriptionClient(string topicPath, string connection, string name)
            : this(new SubscriptionClient(connection, topicPath, name))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Subscription Client</param>
        public BusSubscriptionClient(ISubscriptionClient client)
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
        public void OnMessage(Func<Message, Task> callback, MessageHandlerOptions options)
        {
            //REGISTER FOR EVENT
            //this.client.OnMessageAsync(callback, options);
        }
        #endregion
    }
}