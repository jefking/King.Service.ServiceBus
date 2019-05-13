namespace King.Service.ServiceBus.Wrappers
{
    using Microsoft.Azure.ServiceBus;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Bus Subscription Client Wrapper
    /// </summary>
    public class BusSubscriptionClient : ISubscription
    {
        #region Members
        /// <summary>
        /// Subscription Client
        /// </summary>
        protected readonly ISubscriptionClient client = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="connection">Connection</param>
        /// <param name="topic">Topic</param>
        /// <param name="subscription">Subscription</param>
        public BusSubscriptionClient(string connection, string topic, string subscription)
            : this(new SubscriptionClient(connection, topic, subscription))
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

        #region Properties
        /// <summary>
        /// Subscription Client
        /// </summary>
        public virtual ISubscriptionClient Client
        {
            get
            {
                return this.client;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// On Message
        /// </summary>
        /// <param name="callback">Call Back</param>
        /// <param name="options">Options</param>
        public virtual void OnMessage(Func<Message, CancellationToken, Task> callback, MessageHandlerOptions options)
        {
            this.client.RegisterMessageHandler(callback, options);
        }

        /// <summary>
        /// Add Rule
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="filter">Filter</param>
        public async Task AddRule(string name, Filter filter)
        {
            await this.client.AddRuleAsync(name, filter);
        }
        #endregion
    }
}