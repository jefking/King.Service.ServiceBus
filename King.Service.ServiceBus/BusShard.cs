namespace King.Service.ServiceBus
{
    using King.Azure.Data;
    using System;

    /// <summary>
    /// Bus Shard
    /// </summary>
    public class BusShard : IBusShard
    {
        #region Members
        /// <summary>
        /// Resource
        /// </summary>
        protected readonly IAzureStorage resource;

        /// <summary>
        /// Sender
        /// </summary>
        protected readonly IQueueObject sender;
        #endregion

        #region Constructors
        public BusShard(IAzureStorage resource, IQueueObject sender)
        {
            if (null == resource)
            {
                throw new ArgumentNullException("resource");
            }
            if (null == sender)
            {
                throw new ArgumentNullException("sender");
            }

            this.resource = resource;
            this.sender = sender;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Resource
        /// </summary>
        public virtual IAzureStorage Resource
        {
            get
            {
                return this.resource;
            }
        }

        /// <summary>
        /// Sender
        /// </summary>
        public virtual IQueueObject Sender
        {
            get
            {
                return this.sender;
            }
        }
        #endregion
    }
}