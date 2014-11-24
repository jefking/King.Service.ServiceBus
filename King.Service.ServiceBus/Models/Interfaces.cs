namespace King.Service.ServiceBus.Models
{
    using System;

    #region IBufferedMessage
    /// <summary>
    /// Buffered Message Interface
    /// </summary>
    public interface IBufferedMessage<T>
    {
        #region Properties
        /// <summary>
        /// Data
        /// </summary>
        T Data
        {
            get;
        }

        /// <summary>
        /// Release At
        /// </summary>
        DateTime ReleaseAt
        {
            get;
        }
        #endregion
    }
    #endregion
}