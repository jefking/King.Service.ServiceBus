namespace King.Service.ServiceBus
{
    using Microsoft.ServiceBus;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Initialize Bus Queue
    /// </summary>
    public class InitializeBusQueue : InitializeTask
    {
        #region Members
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
        /// <param name="name"></param>
        /// <param name="manager"></param>
        public InitializeBusQueue(string name, NamespaceManager manager)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }
            if (null == manager)
            {
                throw new ArgumentNullException("manager");
            }

            this.name = name;
            this.manager = manager;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Run
        /// </summary>
        public override void Run()
        {
            if (manager.QueueExists(name))
            {
                Trace.TraceInformation("Queue Already Exists", name);
            }
            else
            {
                var desc = manager.CreateQueueAsync(name);
            }
        }
        #endregion
    }
}