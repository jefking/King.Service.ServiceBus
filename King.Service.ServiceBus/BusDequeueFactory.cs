namespace King.Service.ServiceBus
{
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Scalability;
    using System;
    using System.Collections.Generic;
    using King.Service.Data.Model;


    /// <summary>
    /// Service Bus Dequeue Factory
    /// </summary>
    /// <typeparam name="T">Model Type</typeparam>
    public class BusDequeueFactory : StorageDequeueFactory
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
        /// Creates the Queue, and Loads Dynamic Dequeuer
        /// </summary>
        /// <typeparam name="T">Passthrough</typeparam>
        /// <param name="setup">Setup</param>
        /// <param name="processor">Processor</param>
        /// <returns>Tasks</returns>
        public override IEnumerable<IRunnable> Tasks<T>(IQueueSetup<T> setup)
        {
            if (null == setup)
            {
                throw new ArgumentNullException("setup");
            }

            var queue = new BusQueueReciever(setup.Name, base.connectionString);
            yield return new InitializeBusQueue(setup.Name, base.connectionString);
            yield return this.Dequeue<T>(setup);
        }

        /// <summary>
        /// Dequeue Task (Storage Queue Auto Scaler)
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="setup">Setup</param>
        /// <returns>Storage Queue Auto Scaler</returns>
        public override IRunnable Dequeue<T>(IQueueSetup<T> setup)
        {
            if (null == setup)
            {
                throw new ArgumentNullException("setup");
            }

            var queue = new BusQueue(setup.Name, base.connectionString);
            var messagesPerScaleUnit = this.throughput.MessagesPerScaleUnit(setup.Priority);
            var minimum = this.throughput.MinimumScale(setup.Priority);
            var maximum = this.throughput.MaximumScale(setup.Priority);
            var checkScaleInMinutes = this.throughput.CheckScaleEvery(setup.Priority);
            var connection = new QueueConnection<T>()
            {
                ConnectionString = this.connectionString,
                Setup = setup,
            };

            return new BusQueueAutoScaler<T>(queue, connection, messagesPerScaleUnit, minimum, maximum, checkScaleInMinutes);
        }
        #endregion
    }
}