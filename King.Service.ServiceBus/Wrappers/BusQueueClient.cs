namespace King.Service.ServiceBus.Wrappers
{
    using Microsoft.Azure.ServiceBus;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Bus Queue Client Wrapper
    /// </summary>
    public class BusQueueClient : IBusQueueClient
    {
        #region Members
        /// <summary>
        /// Queue Client
        /// </summary>
        protected readonly IQueueClient client = null;

        /// <summary>
        /// Encoding Property Key
        /// </summary>
        public const string EncodingKey = "encoding";
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="connection">Connetion</param>
        public BusQueueClient(string name, string connection)
            : this(new QueueClient(connection, name))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Queue Client</param>
        public BusQueueClient(IQueueClient client)
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
        /// Queue Client
        /// </summary>
        public virtual IQueueClient Client
        {
            get
            {
                return this.client;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Send
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        public virtual async Task Send(Message message)
        {
            await this.client.SendAsync(message);
        }

        /// <summary>
        /// Send Batch
        /// </summary>
        /// <param name="message">Messages</param>
        /// <returns>Task</returns>
        public virtual async Task Send(IEnumerable<Message> messages)
        {
            //safety on messages
            await this.client.SendAsync(messages.ToList());
        }

        /// <summary>
        /// On Message
        /// </summary>
        /// <param name="callback">Call Back</param>
        /// <param name="options">Options</param>
        public virtual void OnMessage(Func<IMessageSession, Message, CancellationToken, Task> callback, SessionHandlerOptions options)
        {
            this.client.RegisterSessionHandler(callback, options);
        }
        #endregion
    }
}