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
    public class ServiceBusQueuePollerTests
    {
        [Test]
        public void Constructor()
        {
            var wait = TimeSpan.FromSeconds(10);
            var queue = Substitute.For<IBusQueueReciever>();
            new ServiceBusQueuePoller<object>(queue, wait);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorQueueNull()
        {
            var wait = TimeSpan.FromSeconds(10);
            new ServiceBusQueuePoller<object>(null, wait);
        }

        [Test]
        public async Task Poll()
        {
            var wait = TimeSpan.FromSeconds(10);
            var msg = new BrokeredMessage("data");
            var queue = Substitute.For<IBusQueueReciever>();
            queue.Get(wait).Returns(Task.FromResult(msg));

            var poller = new ServiceBusQueuePoller<object>(queue, wait);
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

            var poller = new ServiceBusQueuePoller<object>(queue, wait);
            var returned = await poller.Poll();

            Assert.IsNull(returned);

            queue.Received().Get(wait);
        }

        [Test]
        [ExpectedException(typeof(ApplicationException))]
        public async Task PollGetThrows()
        {
            var wait = TimeSpan.FromSeconds(10);
            var msg = new BrokeredMessage("data");
            var queue = Substitute.For<IBusQueueReciever>();
            queue.Get(wait).Returns<BrokeredMessage>(x => { throw new ApplicationException(); });

            var poller = new ServiceBusQueuePoller<object>(queue, wait);
            await poller.Poll();
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

            var poller = new ServiceBusQueuePoller<object>(queue, wait);
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

            var poller = new ServiceBusQueuePoller<object>(queue, wait);
            var returned = await poller.PollMany(3);

            Assert.IsNull(returned);

            queue.Received().GetMany(wait, 3);
        }

        [Test]
        [ExpectedException(typeof(ApplicationException))]
        public async Task PollGetManyThrows()
        {
            var wait = TimeSpan.FromSeconds(10);
            var msg = new BrokeredMessage("data");
            var queue = Substitute.For<IBusQueueReciever>();
            queue.GetMany(wait).Returns<IEnumerable<BrokeredMessage>>(x => { throw new ApplicationException(); });

            var poller = new ServiceBusQueuePoller<object>(queue, wait);
            await poller.PollMany();
        }
    }
}