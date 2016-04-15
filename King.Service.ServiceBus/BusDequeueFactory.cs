namespace King.Service.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using King.Service.Data;
    using King.Service.Data.Model;
    using King.Service.Scalability;

    /// <summary>
    /// Service Bus Dequeue Factory
    /// </summary>
    public class BusDequeueFactory : DequeueFactory
    {
        #region Constructors
        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        /// <param name="throughput">Queue Throughput</param>
        public BusDequeueFactory(string connectionString, IQueueThroughput throughput = null)
            : base(connectionString, throughput)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create Queue
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Initialization Tasks</returns>
        public override IRunnable Initialize(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }

            return new InitializeStorageTask(new BusQueue(name, base.connectionString));
        }

        /// <summary>
        /// Creates the Queue, and Loads Dynamic Dequeuer
        /// </summary>
        /// <typeparam name="T">Passthrough</typeparam>
        /// <param name="setup">Setup</param>
        /// <returns>Tasks</returns>
        public override IEnumerable<IRunnable> Tasks<T>(IQueueSetup<T> setup)
        {
            if (null == setup)
            {
                throw new ArgumentNullException("setup");
            }

            var queue = new BusQueueReciever(setup.Name, base.connectionString);
            yield return this.Initialize(setup.Name);
            yield return this.Dequeue<T>(setup);
        }

        /// <summary>
        /// Dequeue Task (Service Bus Queue Auto Scaler)
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="setup">Setup</param>
        /// <returns>Service Bus Queue Auto Scaler</returns>
        public override IRunnable Dequeue<T>(IQueueSetup<T> setup)
        {
            if (null == setup)
            {
                throw new ArgumentNullException("setup");
            }

            var messagesPerScaleUnit = this.throughput.MessagesPerScaleUnit(setup.Priority);
            var scale = this.throughput.Scale(setup.Priority);
            var checkScaleInMinutes = this.throughput.CheckScaleEvery(setup.Priority);
            var connection = new QueueConnection<T>()
            {
                ConnectionString = this.connectionString,
                Setup = setup,
            };

            return new BusQueueAutoScaler<T>(new BusQueue(setup.Name, base.connectionString)
                , connection
                , messagesPerScaleUnit
                , scale.Minimum
                , scale.Maximum
                , checkScaleInMinutes);
        }
        #endregion
    }
}