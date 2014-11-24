namespace King.Service.ServiceBus.Models
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Buffered Message
    /// </summary>
    public class BufferedMessage<T> : IBufferedMessage<T>
    {
        #region Properties
        /// <summary>
        /// Data
        /// </summary>
        public virtual T Data
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