namespace King.Service.ServiceBus.Test.Unit.Timing
{
    using System;
    using System.Threading.Tasks;
    using King.Service.ServiceBus.Timing;
    using King.Service.Timing;
    using Microsoft.Azure.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class QueueTimingTrackerTests
    {
        [Test]
        public void Constructor()
        {
            var queue = Substitute.For<IQueueClient>();
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
            var queue = Substitute.For<IQueueClient>();
            Assert.IsNotNull(new QueueTimingTracker(queue) as TimingTracker);
        }

        [Test]
        public void Calculate()
        {
            var queue = Substitute.For<IQueueClient>();
            //queue.LockDuration().Returns(Task.FromResult(TimeSpan.FromMinutes(2)));

            //var tt = new QueueTimingTracker(queue);
            //tt.Calculate(TimeSpan.FromMinutes(1), 22);

            //queue.Received().LockDuration();

            Assert.Fail();
        }

        [Test]
        public void CalculateMultiple()
        {
            var queue = Substitute.For<IQueueClient>();
            //queue.LockDuration().Returns(Task.FromResult(TimeSpan.FromMinutes(2)));

            //var tt = new QueueTimingTracker(queue);
            //tt.Calculate(TimeSpan.FromMinutes(1), 22);
            //tt.Calculate(TimeSpan.FromMinutes(1), 22);
            //tt.Calculate(TimeSpan.FromMinutes(1), 22);
            //tt.Calculate(TimeSpan.FromMinutes(1), 22);

            //queue.Received(1).LockDuration();

            Assert.Fail();
        }

        [Test]
        public void MaxBatchSize()
        {
            Assert.AreEqual(32, QueueTimingTracker.MaxBatchSize);
        }
    }
}