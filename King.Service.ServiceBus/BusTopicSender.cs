namespace King.Service.ServiceBus
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Topic Sender
    /// </summary>
    public class BusTopicSender : TransientErrorHandler, ITopicSender
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
        public BusTopicSender(string name, string connectionString)
            : this(name, NamespaceManager.CreateFromConnectionString(connectionString), new BusTopicClient(TopicClient.CreateFromConnectionString(connectionString, name)))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="manager">Manager</param>
        /// <param name="client"Client>Bus Topic Client</param>
        public BusTopicSender(string name, NamespaceManager manager, IBusTopicClient client)
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

        #region Methods
        /// <summary>
        /// Save Message to Queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        public async Task Send(BrokeredMessage message)
        {
            if (null == message)
            {
                throw new ArgumentNullException("message");
            }

            while (true)
            {
                try
                {
                    await this.client.Send(message);

                    break;
                }
                catch (MessagingException ex)
                {
                    if (ex.IsTransient)
                    {
                        this.HandleTransientError(ex);
                    }
                    else
                    {
                        Trace.TraceError("Error: '{0}'", ex.ToString());

                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Save Object to queue, as json
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>Task</returns>
        public async Task Send(object obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException("obj");
            }

            if (obj is BrokeredMessage)
            {
                await this.Send(obj as BrokeredMessage);
            }
            else
            {
                var msg = new BrokeredMessage(obj)
                {
                    ContentType = obj.GetType().ToString(),
                };

                await this.Send(msg);
            }
        }
        #endregion
    }
}