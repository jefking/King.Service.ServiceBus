namespace King.Service.ServiceBus
{
    using Microsoft.ServiceBus.Messaging;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Bus Queue Sender
    /// </summary>
    public class BusQueueSender : BusQueue, IBusQueueSender
    {
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="connectionString">Connection String</param>
        public BusQueueSender(string name, string connectionString)
            :base(name, connectionString)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Save Message to Queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        public virtual async Task Send(BrokeredMessage message)
        {
            if (null == message)
            {
                throw new ArgumentNullException("message");
            }

            while (true)
            {
                try
                {
                    await this.client.SendAsync(message);

                    break;
                }
                catch (MessagingException ex)
                {
                    if (ex.IsTransient)
                    {
                        this.HandleTransientError(ex);
                    }
                    else
                    {
                        Trace.TraceError("Error: '{0}'", ex.ToString());

                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Save Object to queue, as json
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>Task</returns>
        public virtual async Task Send(object obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException("obj");
            }

            if (obj is BrokeredMessage)
            {
                await this.Send(obj as BrokeredMessage);
            }
            else
            {
                var msg = new BrokeredMessage(obj)
                {
                    ContentType = obj.GetType().ToString(),
                };

                await this.Send(msg);
            }
        }

        /// <summary>
        /// Send Message with Retry
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="enqueueAt">Schedule for Enqueue</param>
        /// <returns>Task</returns>
        public async Task Send(object message, DateTime enqueueAt)
        {
            if (null == message)
            {
                throw new ArgumentNullException("message");
            }

            var msg = new BrokeredMessage(message)
            {
                ScheduledEnqueueTimeUtc = enqueueAt,
                ContentType = message.GetType().ToString(),
            };

            await this.Send(msg);
        }
        #endregion
    }
}