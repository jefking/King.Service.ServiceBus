namespace King.Service.ServiceBus.Unit.Tests.Timing
{
    using King.Service.ServiceBus.Timing;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusQueueTimingTrackerTests
    {
        [Test]
        public void Constructor()
        {
            var queue = Substitute.For<IBusQueue>();
            new BusQueueTimingTracker(queue);
        }

        [Test]
        public void MaxBatchSize()
        {
            Assert.AreEqual(32, BusQueueTimingTracker.MaxBatchSize);
        }
    }
}