namespace King.Service.ServiceBus.Models
{
    using System;

    /// <summary>
    /// Buffered Message
    /// </summary>
    public class BufferedMessage : IBufferedMessage
    {
        #region Properties
        /// <summary>
        /// Data
        /// </summary>
        public virtual string Data
        {
            get;
            set;
        }

        /// <summary>
        /// Release At
        /// </summary>
        public virtual DateTime ReleaseAt
        {
            get;
            set;
        }
        #endregion
    }
}