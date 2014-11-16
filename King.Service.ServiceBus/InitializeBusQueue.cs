namespace King.Service.ServiceBus
{
    using Microsoft.ServiceBus;
    using System;

    /// <summary>
    /// 
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
        /// 
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
        /// 
        /// </summary>
        public override void Run()
        {
            if (manager.QueueExists(name))
            {
            }
            else
            {
                var desc = manager.CreateQueueAsync(name);
            }
        }
        #endregion
    }
}