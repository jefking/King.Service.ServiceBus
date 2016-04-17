namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Wrappers;

    /// <summary>
    /// Bus Queue Sender
    /// </summary>
    public class BusQueueSender : BusMessageSender
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="connection">Connection String</param>
        public BusQueueSender(string name, string connection)
            : base(name, new BusQueueClient(name, connection))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="manager">Manager</param>
        /// <param name="client"Client></param>
        public BusQueueSender(string name, IBusQueueClient client)
            : base(name, client)
        {
        }
        #endregion
    }
}