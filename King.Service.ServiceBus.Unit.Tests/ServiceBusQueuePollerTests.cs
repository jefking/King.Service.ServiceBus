namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class ServiceBusQueuePollerTests
    {
        [Test]
        public void Constructor()
        {
            var queue = Substitute.For<IBusQueue>();
            new ServiceBusQueuePoller<object>(queue);

        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorQueueNull()
        {
            new ServiceBusQueuePoller<object>(null);
        }


        [Test]
        public async Task Poll()
        {
            var msg = new BrokeredMessage("data");
            var queue = Substitute.For<IBusQueue>();
            queue.Get().Returns(Task.FromResult(msg));

            var poller = new ServiceBusQueuePoller<object>(queue);
            var returned = await poller.Poll();

            Assert.IsNotNull(returned);

            queue.Received().Get();
        }

        [Test]
        public async Task PollGetNull()
        {
            var queue = Substitute.For<IBusQueue>();
            queue.Get().Returns(Task.FromResult<BrokeredMessage>(null));

            var poller = new ServiceBusQueuePoller<object>(queue);
            var returned = await poller.Poll();

            Assert.IsNull(returned);

            queue.Received().Get();
        }

        [Test]
        [ExpectedException(typeof(ApplicationException))]
        public async Task PollGetThrows()
        {
            var msg = new BrokeredMessage("data");
            var queue = Substitute.For<IBusQueue>();
            queue.Get().Returns(x => { throw new ApplicationException(); });

            var poller = new ServiceBusQueuePoller<object>(queue);
            await poller.Poll();
        }

        [Test]
        public async Task PollMany()
        {
            var msg = new BrokeredMessage("data");
            var msgs = new List<BrokeredMessage>(3);
            msgs.Add(msg);
            msgs.Add(msg);
            msgs.Add(msg);

            var queue = Substitute.For<IBusQueue>();
            queue.GetMany(3).Returns(Task.FromResult<IEnumerable<BrokeredMessage>>(msgs));

            var poller = new ServiceBusQueuePoller<object>(queue);
            var returned = await poller.PollMany(3);

            Assert.IsNotNull(returned);
            Assert.AreEqual(3, returned.Count());

            queue.Received().GetMany(3);
        }

        [Test]
        public async Task PollGetManyNull()
        {
            var queue = Substitute.For<IBusQueue>();
            queue.GetMany(3).Returns(Task.FromResult<IEnumerable<BrokeredMessage>>(null));

            var poller = new ServiceBusQueuePoller<object>(queue);
            var returned = await poller.PollMany(3);

            Assert.IsNull(returned);

            queue.Received().GetMany(3);
        }

        [Test]
        [ExpectedException(typeof(ApplicationException))]
        public async Task PollGetManyThrows()
        {
            var msg = new BrokeredMessage("data");
            var queue = Substitute.For<IBusQueue>();
            queue.GetMany().Returns(x => { throw new ApplicationException(); });

            var poller = new ServiceBusQueuePoller<object>(queue);
            await poller.PollMany();
        }
    }
}