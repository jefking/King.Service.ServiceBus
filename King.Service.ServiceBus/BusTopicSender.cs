namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;

    /// <summary>
    /// Bus Topic Sender
    /// </summary>
    public class BusTopicSender : BusMessageSender
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Topic Name</param>
        /// <param name="connectionString">Connection String</param>
        public BusTopicSender(string name, string connectionString)
            : this(name, new BusTopicClient(new TopicClient(connectionString, name)))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="name">Topic Name</param>
        /// <param name="client"Client>Service Bus Message Client</param>
        public BusTopicSender(string name, IMessageSender client)
            : base(name, client)
        {
        }
        #endregion
    }
}