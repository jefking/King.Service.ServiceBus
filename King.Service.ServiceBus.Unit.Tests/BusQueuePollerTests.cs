namespace King.Service.ServiceBus.Unit.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using King.Service.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BusQueuePollerTests
    {
        [Test]
        public void Constructor()
        {
            var wait = TimeSpan.FromSeconds(10);
            var queue = Substitute.For<IBusQueueReciever>();
            new BusQueuePoller<object>(queue, wait);
        }

        [Test]
        public void ConstructorQueueNull()
        {
            var wait = TimeSpan.FromSeconds(10);
            Assert.That(() => new BusQueuePoller<object>(null, wait), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorWaitTimeZero()
        {
            var queue = Substitute.For<IBusQueueReciever>();
            var bqp = new BusQueuePoller<object>(queue, TimeSpan.Zero);
            Assert.AreEqual(BusQueuePoller<object>.DefaultWaitTime, bqp.WaitTime);
        }

        [Test]
        public void DefaultWaitTime()
        {
            Assert.AreEqual(TimeSpan.FromSeconds(15), BusQueuePoller<object>.DefaultWaitTime);
        }

        [Test]
        public async Task Poll()
        {
            var wait = TimeSpan.FromSeconds(10);
            var msg = new BrokeredMessage("data");
            var queue = Substitute.For<IBusQueueReciever>();
            queue.Get(wait).Returns(Task.FromResult(msg));

            var poller = new BusQueuePoller<object>(queue, wait);
            var returned = await poller.Poll();

            Assert.IsNotNull(returned);

            queue.Received().Get(wait);
        }

        [Test]
        public async Task PollGetNull()
        {
            var wait = TimeSpan.FromSeconds(10);
            var queue = Substitute.For<IBusQueueReciever>();
            queue.Get(wait).Returns(Task.FromResult<BrokeredMessage>(null));

            var poller = new BusQueuePoller<object>(queue, wait);
            var returned = await poller.Poll();

            Assert.IsNull(returned);

            queue.Received().Get(wait);
        }

        [Test]
        public void PollGetThrows()
        {
            var wait = TimeSpan.FromSeconds(10);
            var msg = new BrokeredMessage("data");
            var queue = Substitute.For<IBusQueueReciever>();
            queue.Get(wait).Returns<BrokeredMessage>(x => { throw new ApplicationException(); });

            var poller = new BusQueuePoller<object>(queue, wait);
            Assert.That(async () => await poller.Poll(), Throws.TypeOf<ApplicationException>());
        }

        [Test]
        public async Task PollMany()
        {
            var wait = TimeSpan.FromSeconds(10);
            var msg = new BrokeredMessage("data");
            var msgs = new List<BrokeredMessage>(3);
            msgs.Add(msg);
            msgs.Add(msg);
            msgs.Add(msg);

            var queue = Substitute.For<IBusQueueReciever>();
            queue.GetMany(wait, 3).Returns(Task.FromResult<IEnumerable<BrokeredMessage>>(msgs));

            var poller = new BusQueuePoller<object>(queue, wait);
            var returned = await poller.PollMany(3);

            Assert.IsNotNull(returned);
            Assert.AreEqual(3, returned.Count());

            queue.Received().GetMany(wait, 3);
        }

        [Test]
        public async Task PollGetManyNull()
        {
            var wait = TimeSpan.FromSeconds(10);
            var queue = Substitute.For<IBusQueueReciever>();
            queue.GetMany(wait, 3).Returns(Task.FromResult<IEnumerable<BrokeredMessage>>(null));

            var poller = new BusQueuePoller<object>(queue, wait);
            var returned = await poller.PollMany(3);

            Assert.IsNull(returned);

            queue.Received().GetMany(wait, 3);
        }

        [Test]
        public void PollGetManyThrows()
        {
            var wait = TimeSpan.FromSeconds(10);
            var msg = new BrokeredMessage("data");
            var queue = Substitute.For<IBusQueueReciever>();
            queue.GetMany(wait).Returns<IEnumerable<BrokeredMessage>>(x => { throw new ApplicationException(); });

            var poller = new BusQueuePoller<object>(queue, wait);
            Assert.That(async () => await poller.PollMany(), Throws.TypeOf<ApplicationException>());
        }
    }
}