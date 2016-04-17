namespace King.Service.ServiceBus.Timing
{
    using System;
    using King.Service.Timing;

    /// <summary>
    /// Bus Topic Timing Tracker
    /// </summary>
    public class TopicTimingTracker : TimingTracker
    {
        #region Members
        /// <summary>
        /// Maximum batchsize = 32
        /// </summary>
        public const byte MaxBatchSize = 32;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public TopicTimingTracker()
            : base(TimeSpan.FromMinutes(1), MaxBatchSize)
        {
        }
        #endregion
    }
}