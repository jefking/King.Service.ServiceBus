namespace King.Service.ServiceBus.Unit.Tests.Timing
{
    using System;
    using System.Threading.Tasks;
    using King.Service.ServiceBus.Timing;
    using King.Service.Timing;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class QueueTimingTrackerTests
    {
        [Test]
        public void Constructor()
        {
            var queue = Substitute.For<IBusQueue>();
            new QueueTimingTracker(queue);
        }

        [Test]
        public void ConstructorQueueNull()
        {
            Assert.That(() => new QueueTimingTracker(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void IsTimingTracker()
        {
            var queue = Substitute.For<IBusQueue>();
            Assert.IsNotNull(new QueueTimingTracker(queue) as TimingTracker);
        }

        [Test]
        public void Calculate()
        {
            var queue = Substitute.For<IBusQueue>();
            queue.LockDuration().Returns(Task.FromResult(TimeSpan.FromMinutes(2)));

            var tt = new QueueTimingTracker(queue);
            tt.Calculate(TimeSpan.FromMinutes(1), 22);

            queue.Received().LockDuration();
        }

        [Test]
        public void CalculateMultiple()
        {
            var queue = Substitute.For<IBusQueue>();
            queue.LockDuration().Returns(Task.FromResult(TimeSpan.FromMinutes(2)));

            var tt = new QueueTimingTracker(queue);
            tt.Calculate(TimeSpan.FromMinutes(1), 22);
            tt.Calculate(TimeSpan.FromMinutes(1), 22);
            tt.Calculate(TimeSpan.FromMinutes(1), 22);
            tt.Calculate(TimeSpan.FromMinutes(1), 22);

            queue.Received(1).LockDuration();
        }

        [Test]
        public void MaxBatchSize()
        {
            Assert.AreEqual(32, QueueTimingTracker.MaxBatchSize);
        }
    }
}