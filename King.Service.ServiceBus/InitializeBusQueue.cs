namespace King.Service.ServiceBus
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Initialize Bus Queue
    /// </summary>
    public class InitializeBusQueue : InitializeTask
    {
        #region Members
        /// <summary>
        /// Queue
        /// </summary>
        protected readonly IBusQueue queue = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connectionString">Connection String</param>
        public InitializeBusQueue(string name, string connectionString)
            : this(new BusQueue(name, connectionString))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="queue">Service Bus Queue</param>
        public InitializeBusQueue(IBusQueue queue)
        {
            if (null == queue)
            {
                throw new ArgumentNullException("queue");
            }

            this.queue = queue;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run Async
        /// </summary>
        /// <returns></returns>
        public override async Task RunAsync()
        {
            await this.queue.CreateIfNotExists();
        }
        #endregion
    }
}