namespace King.Service.ServiceBus.Models
{
    using global::Azure.Data.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;
    using Wrappers;

    /// <summary>
    /// Generic Wrapper for Brokered Messages
    /// </summary>
    /// <typeparam name="T">Type, Serialized in Message Body</typeparam>
    public class Queued<T>
    {
        #region Members
        /// <summary>
        /// Brokered Message
        /// </summary>
        protected readonly Message message = null;

        /// <summary>
        /// Cached Copy of Message Data
        /// </summary>
        protected T cache = default(T);

        /// <summary>
        /// Cached
        /// </summary>
        protected bool cached = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="msg">Message</param>
        public Queued(Message msg)
        {
            if (null == msg)
            {
                throw new ArgumentNullException("msg");
            }

            this.message = msg;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Data
        /// </summary>
        /// <returns>Data</returns>
        public virtual Task<T> Data()
        {
            if (!cached)
            {
                var p = this.message.UserProperties;

                if (p.ContainsKey(BusQueueClient.EncodingKey)
                    && (Encoding)p[BusQueueClient.EncodingKey] == Encoding.Json)
                {
                    var d = message.Body;
                    var raw = System.Text.Encoding.Default.GetString(d);
                    cache = JsonConvert.DeserializeObject<T>(raw);
                }
                else
                {
                    using (var memStream = new MemoryStream())
                    {
                        var binForm = new BinaryFormatter();
                        memStream.Write(this.message.Body, 0, this.message.Body.Length);
                        memStream.Seek(0, SeekOrigin.Begin);
                        cache = (T)binForm.Deserialize(memStream);
                    }
                }

                cached = true;
            }

            return Task.FromResult(cache);
        }
        #endregion
    }
}