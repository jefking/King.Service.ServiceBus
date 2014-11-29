namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using System;

    /// <summary>
    /// Topic Sender
    /// </summary>
    public class TopicSender : ITopicSender
    {
        #region Members
        /// <summary>
        /// Queue Client
        /// </summary>
        protected readonly IBusTopicClient client = null;

        /// <summary>
        /// Name
        /// </summary>
        protected readonly string name = null;

        /// <summary>
        /// Namespace Manager
        /// </summary>
        protected readonly NamespaceManager manager = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="connectionString">Connection String</param>
        public TopicSender(string name, string connectionString)
            : this(name, NamespaceManager.CreateFromConnectionString(connectionString), new BusTopicClient(TopicClient.CreateFromConnectionString(connectionString, name)))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="manager">Manager</param>
        /// <param name="client"Client></param>
        public TopicSender(string name, NamespaceManager manager, IBusTopicClient client)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }
            if (null == client)
            {
                throw new ArgumentNullException("client");
            }
            if (null == manager)
            {
                throw new ArgumentNullException("manager");
            }

            this.name = name;
            this.manager = manager;
            this.client = client;
        }
        #endregion
    }
}