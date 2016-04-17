namespace King.Service.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ServiceBus.Messaging;
    using Models;
    using Newtonsoft.Json;
    using Wrappers;

    /// <summary>
    /// Bus Message Sender
    /// </summary>
    public class BusMessageSender : TransientErrorHandler, IBusMessageSender
    {
        #region Members
        /// <summary>
        /// Buffered Offset (Seconds)
        /// </summary>
        public const sbyte BufferedOffset = -6;

        /// <summary>
        /// Service Bus Message Client
        /// </summary>
        protected readonly IBusSender client = null;

        /// <summary>
        /// Name
        /// </summary>
        protected readonly string name = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="name">Message Bus Name</param>
        /// <param name="client"Client>Service Bus Message Client</param>
        public BusMessageSender(string name, IBusSender client)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }
            if (null == client)
            {
                throw new ArgumentNullException("client");
            }

            this.name = name;
            this.client = client;
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
                    await this.client.Send(message);

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
        /// Send Message to Queue
        /// </summary>
        /// <param name="messages">Messages</param>
        /// <returns>Task</returns>
        public virtual async Task Send(IEnumerable<BrokeredMessage> messages)
        {
            if (null == messages)
            {
                throw new ArgumentNullException("message");
            }

            while (true)
            {
                try
                {
                    await this.client.Send(messages);

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
        /// Send Object to queue, as json
        /// </summary>
        /// <param name="messages">Messages</param>
        /// <returns>Task</returns>
        public virtual async Task Send(IEnumerable<object> messages)
        {
            if (null == messages)
            {
                throw new ArgumentNullException("obj");
            }

            if (messages is IEnumerable<BrokeredMessage>)
            {
                await this.Send(messages as IEnumerable<BrokeredMessage>);
            }
            else
            {
                await this.Send(messages.Select(obj => new BrokeredMessage(obj)
                {
                    ContentType = obj.GetType().ToString(),
                }));
            }
        }

        /// <summary>
        /// Send Message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="enqueueAt">Schedule for Enqueue</param>
        /// <returns>Task</returns>
        public virtual async Task Send(object message, DateTime enqueueAt)
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

        /// <summary>
        /// Send Message for Buffer
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="enqueueAt">Schedule for Enqueue</param>
        /// <param name="offset">Offset</param>
        /// <returns>Task</returns>
        public virtual async Task SendBuffered(object data, DateTime releaseAt, sbyte offset = BufferedOffset)
        {
            var message = new BufferedMessage
            {
                Data = null == data ? null : JsonConvert.SerializeObject(data),
                ReleaseAt = releaseAt,
            };

            await this.Send(message, releaseAt.AddSeconds(offset));
        }
        #endregion
    }
}