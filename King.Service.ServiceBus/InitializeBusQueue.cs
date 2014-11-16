namespace King.Service.ServiceBus
{
    using Microsoft.ServiceBus;
    using System;
    using System.Diagnostics;
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
        /// <param name="manager"></param>
        public InitializeBusQueue(string name, NamespaceManager manager)
            :this(new BusQueue(name, manager))
        {
        }

        public InitializeBusQueue(IBusQueue queue)
        {
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