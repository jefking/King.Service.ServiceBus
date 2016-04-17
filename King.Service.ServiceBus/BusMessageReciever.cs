namespace King.Service.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Bus Message Reciever
    /// </summary>
    public class BusMessageReciever : TransientErrorHandler, IBusMessageReciever
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
        /// Get Cloud Message
        /// </summary>
        /// <param name="waitTime">Wait Time</param>
        /// <returns>Message</returns>
        public virtual async Task<BrokeredMessage> Get(TimeSpan waitTime)
        {
            waitTime = waitTime <= TimeSpan.Zero ? serverWaitTime : waitTime;

            while (true)
            {
                try
                {
                    return await this.reciever.Recieve(waitTime);
                }
                catch (MessagingException ex)
                {
                    if (ex.IsTransient)
                    {
                        base.HandleTransientError(ex);
                    }
                    else
                    {
                        Trace.TraceError(ex.ToString());

                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Get Many Cloud Message
        /// </summary>
        /// <param name="waitTime">Wait Time</param>
        /// <param name="messageCount">Message Count</param>
        /// <returns>Messages</returns>
        public virtual async Task<IEnumerable<BrokeredMessage>> GetMany(TimeSpan waitTime, int messageCount = 5)
        {
            messageCount = 1 > messageCount ? 5 : messageCount;
            waitTime = waitTime <= TimeSpan.Zero ? serverWaitTime : waitTime;

            while (true)
            {
                try
                {
                    return await this.reciever.RecieveBatch(messageCount, waitTime);
                }
                catch (MessagingException ex)
                {
                    if (ex.IsTransient)
                    {
                        base.HandleTransientError(ex);
                    }
                    else
                    {
                        Trace.TraceError(ex.ToString());

                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// On Error
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="ex">Exception</param>
        public virtual void RegisterForEvents(Func<BrokeredMessage, Task> callback, OnMessageOptions options)
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