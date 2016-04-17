namespace King.Service.ServiceBus
{
    using System;
    using System.Collections.Generic;
    using King.Azure.Data;
    using King.Service.Data;
    using King.Service.Scalability;
    using King.Service.Timing;

    /// <summary>
    /// Storage Queue AutoScaler
    /// </summary>
    /// <typeparam name="T">Processor Type</typeparam>
    public class BusQueueAutoScaler<T> : QueueAutoScaler<IQueueConnection<T>>
    {
        #region Members
        /// <summary>
        /// Queue Throughput
        /// </summary>
        protected readonly IQueueThroughput throughput = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="count">Count</param>
        /// <param name="messagesPerScaleUnit">Messages Per-Scale Unit</param>
        /// <param name="connection">Setup</param>
        /// <param name="minimum">Minimum Scale</param>
        /// <param name="maximum">Maximmum Scale</param>
        /// <param name="checkScaleInMinutes">Check Scale Every</param>
        public BusQueueAutoScaler(IQueueCount count, IQueueConnection<T> connection, ushort messagesPerScaleUnit = QueueScaler<T>.MessagesPerScaleUnitDefault, byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = BaseTimes.ScaleCheck)
            : this(count, connection, new QueueThroughput(), messagesPerScaleUnit, minimum, maximum, checkScaleInMinutes)
        {
        }

        /// <summary>
        /// Mockable Constructor
        /// </summary>
        /// <param name="count">Count</param>
        /// <param name="connection">Setup</param>
        /// <param name="throughput">Throughput</param>
        /// <param name="messagesPerScaleUnit">Messages Per-Scale Unit</param>
        /// <param name="connection">Setup</param>
        /// <param name="minimum">Minimum Scale</param>
        /// <param name="maximum">Maximmum Scale</param>
        /// <param name="checkScaleInMinutes">Check Scale Every</param>
        public BusQueueAutoScaler(IQueueCount count, IQueueConnection<T> connection, IQueueThroughput throughput, ushort messagesPerScaleUnit = QueueScaler<T>.MessagesPerScaleUnitDefault, byte minimum = 1, byte maximum = 2, byte checkScaleInMinutes = BaseTimes.ScaleCheck)
            : base(count, messagesPerScaleUnit, connection, minimum, maximum, checkScaleInMinutes)
        {
            if (null == throughput)
            {
                throw new ArgumentNullException("throughput");
            }

            this.throughput = throughput;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Scale Unit
        /// </summary>
        /// <param name="connection">Queue Connection</param>
        /// <returns>Scalable Task</returns>
        public override IEnumerable<IScalable> ScaleUnit(IQueueConnection<T> connection)
        {
            if (null == connection)
            {
                throw new ArgumentNullException("setup");
            }

            yield return this.throughput.Runner(this.Runs(connection), connection.Setup.Priority);
        }

        /// <summary>
        /// Runs
        /// </summary>
        /// <param name="connection">Queue Connection</param>
        /// <returns>Dynamic Runs</returns>
        public virtual IDynamicRuns Runs(IQueueConnection<T> connection)
        {
            if (null == connection)
            {
                throw new ArgumentNullException("setup");
            }

            var frequency = this.throughput.Frequency(connection.Setup.Priority);
            return new QueueBatchDynamic<T>(connection.Setup.Name, connection.ConnectionString, connection.Setup.Processor(), frequency.Minimum, frequency.Maximum);
        }
        #endregion
    }
}