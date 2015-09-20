namespace King.Service.ServiceBus
{
    using System;
    using Microsoft.ServiceBus.Messaging;

    #region Tansient Error
    /// <summary>
    /// Transient Error Args
    /// </summary>
    public class TransientErrorArgs : EventArgs
    {
        #region Propertiees
        /// <summary>
        /// Exception
        /// </summary>
        public MessagingException Exception
        {
            get;
            set;
        }
        #endregion
    }

    /// <summary>
    /// Transient Error Event
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Tansient Error Args</param>
    public delegate void TransientErrorEventHandler(object sender, TransientErrorArgs e);
    #endregion
}