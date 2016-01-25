namespace King.Service.ServiceBus.Unit.Tests.Timing
{
    using King.Service.ServiceBus.Timing;
    using King.Service.Timing;
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
        public void ConstructorQueueNull()
        {
            Assert.That(() => new BusQueueTimingTracker(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void IsTimingTracker()
        {
            var queue = Substitute.For<IBusQueue>();
            Assert.IsNotNull(new BusQueueTimingTracker(queue) as TimingTracker);
        }

        [Test]
        public void Calculate()
        {
            var queue = Substitute.For<IBusQueue>();
            queue.LockDuration().Returns(Task.FromResult(TimeSpan.FromMinutes(2)));

            var tt = new BusQueueTimingTracker(queue);
            tt.Calculate(TimeSpan.FromMinutes(1), 22);

            queue.Received().LockDuration();
        }

        [Test]
        public void CalculateMultiple()
        {
            var queue = Substitute.For<IBusQueue>();
            queue.LockDuration().Returns(Task.FromResult(TimeSpan.FromMinutes(2)));

            var tt = new BusQueueTimingTracker(queue);
            tt.Calculate(TimeSpan.FromMinutes(1), 22);
            tt.Calculate(TimeSpan.FromMinutes(1), 22);
            tt.Calculate(TimeSpan.FromMinutes(1), 22);
            tt.Calculate(TimeSpan.FromMinutes(1), 22);

            queue.Received(1).LockDuration();
        }

        [Test]
        public void MaxBatchSize()
        {
            Assert.AreEqual(32, BusQueueTimingTracker.MaxBatchSize);
        }
    }
}