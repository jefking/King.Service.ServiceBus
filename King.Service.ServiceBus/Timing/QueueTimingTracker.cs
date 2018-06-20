namespace King.Service.ServiceBus.Timing
{
    using King.Service.Timing;
    using Microsoft.Azure.ServiceBus;
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Bus Queue Timing Tracker
    /// </summary>
    public class QueueTimingTracker : TimingTracker
    {
        #region Members
        /// <summary>
        /// Maximum batchsize = 32
        /// </summary>
        public const byte MaxBatchSize = 32;

        /// <summary>
        /// First Run
        /// </summary>
        protected bool firstRun = true;

        /// <summary>
        /// Queue
        /// </summary>
        protected readonly IQueueClient queue = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="queue">Queue</param>
        public QueueTimingTracker(IQueueClient queue)
            : base(TimeSpan.FromMinutes(1), MaxBatchSize)
        {
            if (null == queue)
            {
                throw new ArgumentNullException("queue");
            }

            this.queue = queue;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Calculate
        /// </summary>
        /// <param name="duration">Duration</param>
        /// <param name="currentSize">Current Size</param>
        /// <returns>Size</returns>
        public override byte Calculate(TimeSpan duration, byte currentSize)
        {
            if (this.firstRun)
            {
                //BUG
                base.maxTime = TimeSpan.FromMinutes(1);//this.queue.LockDuration().Result;

                Trace.TraceInformation("Lock duration for {0} is {1}.", this.queue.QueueName, base.maxTime);

                this.firstRun = false;
            }

            return base.Calculate(duration, currentSize);
        }
        #endregion
    }
}