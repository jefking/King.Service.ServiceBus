namespace King.Service.ServiceBus
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Bus Message Reciever
    /// </summary>
    public class BusMessageReciever : IBusMessageReciever
    {
        #region Members
        /// <summary>
        /// Service Bus Message Client
        /// </summary>
        protected readonly IBusReciever reciever = null;

        /// <summary>
        /// Server Wait Time in Seconds
        /// </summary>
        public const int DefaultWaitTime = 15;

        /// <summary>
        /// Server Wait Time
        /// </summary>
        protected readonly TimeSpan serverWaitTime = TimeSpan.FromSeconds(DefaultWaitTime);
        #endregion

        #region Constructors
        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="reciever">Service Bus Message Client</param>
        /// <param name="waitTime">Server Wait Time</param>
        public BusMessageReciever(IBusReciever reciever, int waitTime = DefaultWaitTime)
        {
            if (null == reciever)
            {
                throw new ArgumentNullException("client");
            }

            this.reciever = reciever;
            var wt = waitTime <= 0 ? DefaultWaitTime : waitTime;
            this.serverWaitTime = TimeSpan.FromSeconds(wt);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Server Wait Time
        /// </summary>
        public TimeSpan ServerWaitTime
        {
            get
            {
                return this.serverWaitTime;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// On Error
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="ex">Exception</param>
        public virtual void RegisterForEvents(Func<Message, Task> callback, MessageHandlerOptions options)
        {
            if (null == callback)
            {
                throw new ArgumentNullException("callback");
            }
            if (null == options)
            {
                throw new ArgumentNullException("options");
            }

            this.reciever.OnMessage(callback, options);
        }
        #endregion
    }
}