namespace King.Service.ServiceBus
{
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Bus Queue Reciever
    /// </summary>
    public class BusQueueReciever : BusQueue, IBusQueueReciever
    {
        #region Members
        /// <summary>
        /// Server Wait Time
        /// </summary>
        protected static readonly TimeSpan serverWaitTime = TimeSpan.FromSeconds(15);
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="connectionString">Connection String</param>
        public BusQueueReciever(string name, string connectionString)
            :base(name, connectionString)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get Cloud Queue Message
        /// </summary>
        /// <returns>Message</returns>
        public virtual async Task<BrokeredMessage> Get()
        {
            while (true)
            {
                try
                {
                    return await base.client.Receive(serverWaitTime);
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
        /// Get Many Cloud Queue Message
        /// </summary>
        /// <returns>Messages</returns>
        public virtual async Task<IEnumerable<BrokeredMessage>> GetMany(int messageCount = 5)
        {
            messageCount = 1 > messageCount ? 5 : messageCount;

            while (true)
            {
                try
                {
                    return await this.client.ReceiveBatch(messageCount, serverWaitTime);
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

            this.client.OnMessage(callback, options);
        }
        #endregion
    }
}