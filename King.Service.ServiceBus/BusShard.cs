namespace King.Service.ServiceBus
{
    using System;
    using King.Azure.Data;

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
        protected readonly IBusMessageSender sender;
        #endregion

        #region Constructors
        public BusShard(IAzureStorage resource, IBusMessageSender sender)
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
        public virtual IBusMessageSender Sender
        {
            get
            {
                return this.sender;
            }
        }
        #endregion
    }
}