namespace King.Service.ServiceBus
{
    using System;
    using System.Threading.Tasks;
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Wrappers;
    using Newtonsoft.Json;

    /// <summary>
    /// Bus Queue Sender
    /// </summary>
    public class BusQueueSender : BusMessageSender, IBusQueueSender
    {
        #region Members
        /// <summary>
        /// Buffered Offset (Seconds)
        /// </summary>
        public const sbyte BufferedOffset = -6;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="connection">Connection String</param>
        public BusQueueSender(string name, string connection)
            : base(name, new BusQueueClient(name, connection))
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="name">Queue Name</param>
        /// <param name="manager">Manager</param>
        /// <param name="client"Client></param>
        public BusQueueSender(string name, IBusQueueClient client)
            : base(name, client)
        {
        }
        #endregion

        #region Methods
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