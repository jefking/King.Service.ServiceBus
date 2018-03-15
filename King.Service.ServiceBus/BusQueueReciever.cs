namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Wrappers;

    /// <summary>
    /// Bus Queue Reciever
    /// </summary>
    public class BusQueueReciever : BusMessageReciever
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="connectionString">Connection String</param>
        public BusQueueReciever(string name, string connectionString)
            : base(new BusQueueClient(name, connectionString))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="client">Client</param>
        public BusQueueReciever(IBusQueueClient client)
            : base(client)
        {
        }
        #endregion
    }
}