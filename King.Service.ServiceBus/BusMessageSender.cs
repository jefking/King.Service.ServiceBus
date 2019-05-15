namespace King.Service.ServiceBus
{
    using Microsoft.Azure.ServiceBus;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;
    using Wrappers;

    /// <summary>
    /// Bus Message Sender
    /// </summary>
    public class BusMessageSender : IBusMessageSender
    {
        #region Members
        /// <summary>
        /// Buffered Offset (Seconds)
        /// </summary>
        public const sbyte BufferedOffset = -6;

        /// <summary>
        /// Service Bus Message Client
        /// </summary>
        protected readonly IMessageSender client = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="client"Client>Service Bus Message Client</param>
        public BusMessageSender(IMessageSender client)
        {
            if (null == client)
            {
                throw new ArgumentNullException("client");
            }

            this.client = client;
        }

        public Task<T> GetAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetManyAsync<T>(int count = 5)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> PeekAsync<T>(int count = 1)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Save Message to Queue
        /// </summary>
        /// <param name="message">Message</param>
        /// <returns>Task</returns>
        public virtual async Task Send(Message message)
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
                catch (Exception ex)
                {
                    Trace.TraceError("Error: '{0}'", ex.ToString());

                    throw;
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
            await Send(obj, Encoding.Json);
        }

        /// <summary>
        /// Save Object to queue, as json
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="encoding">Encoding (Default Json)</param>
        /// <returns>Task</returns>
        public virtual async Task Send(object obj, Encoding encoding = Encoding.Json)
        {
            if (null == obj)
            {
                throw new ArgumentNullException("obj");
            }

            if (obj is Message)
            {
                await this.Send(obj as Message);
            }
            else
            {
                byte[] data;
                switch (encoding)
                {
                    case Encoding.Json:
                        var j = JsonConvert.SerializeObject(obj);
                        data = System.Text.Encoding.Default.GetBytes(j);
                        break;
                    default:
                        var bf = new BinaryFormatter();
                        using (var ms = new MemoryStream())
                        {
                            bf.Serialize(ms, obj);
                            data = ms.ToArray();
                        }
                        break;
                }
                
                var msg = new Message(data)
                {
                    ContentType = obj.GetType().ToString(),
                };

                msg.UserProperties.Add("encoding", (byte)encoding);

                await this.Send(msg);
            }
        }

        /// <summary>
        /// Send Message to Queue
        /// </summary>
        /// <param name="messages">Messages</param>
        /// <returns>Task</returns>
        public virtual async Task Send(IEnumerable<Message> messages)
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
                catch (Exception ex)
                {
                    Trace.TraceError("Error: '{0}'", ex.ToString());

                    throw;
                }
            }
        }

        /// <summary>
        /// Send Object to queue, as json
        /// </summary>
        /// <param name="messages">Messages</param>
        /// <param name="encoding">Encoding (Default Json)</param>
        /// <returns>Task</returns>
        public virtual async Task Send(IEnumerable<object> messages, Encoding encoding = Encoding.Json)
        {
            if (null == messages)
            {
                throw new ArgumentNullException("obj");
            }

            if (messages is IEnumerable<Message>)
            {
                await this.Send(messages as IEnumerable<Message>);
            }
            else
            {
                var Messages = new List<Message>(messages.Count());
                foreach (var m in messages)
                {
                    byte[] data;
                    switch (encoding)
                    {
                        case Encoding.Json:
                            var j = JsonConvert.SerializeObject(m);
                            data = System.Text.Encoding.Default.GetBytes(j);
                            break;
                        default:
                            var bf = new BinaryFormatter();
                            using (var ms = new MemoryStream())
                            {
                                bf.Serialize(ms, m);
                                data = ms.ToArray();
                            }
                            break;
                    }

                    var msg = new Message(data)
                    {
                        ContentType = m.GetType().ToString(),
                    };

                    msg.UserProperties.Add("encoding", (byte)encoding);
                }

                await this.Send(Messages);
            }
        }

        /// <summary>
        /// Send Message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="enqueueAt">Schedule for Enqueue</param>
        /// <returns>Task</returns>
        public virtual async Task Send(object message, DateTime enqueueAt, Encoding encoding = Encoding.Json)
        {
            if (null == message)
            {
                throw new ArgumentNullException("message");
            }

            byte[] data;
            switch (encoding)
            {
                case Encoding.Json:
                    var j = JsonConvert.SerializeObject(message);
                    data = System.Text.Encoding.Default.GetBytes(j);
                    break;
                default:
                    var bf = new BinaryFormatter();
                    using (var ms = new MemoryStream())
                    {
                        bf.Serialize(ms, message);
                        data = ms.ToArray();
                    }
                    break;
            }

            var msg = new Message(data)
            {
                ScheduledEnqueueTimeUtc = enqueueAt,
                ContentType = message.GetType().ToString(),
            };
            msg.UserProperties.Add("encoding", (byte)encoding);

            await this.Send(msg);
        }

        public Task SendAsync<T>(T message)
        {
            throw new NotImplementedException();
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
                UserProperties =
                {
                    //MAP OBJECT DATA TO USER PROPERTIES, For filtering!!
                }
            };

            await this.Send(message, releaseAt.AddSeconds(offset));
        }
        #endregion
    }
}