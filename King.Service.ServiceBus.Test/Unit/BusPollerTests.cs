namespace King.Service.ServiceBus.Test.Unit
{
    using King.Service.ServiceBus;
    using Microsoft.Azure.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusPollerTests
    {
        [Test]
        public void Constructor()
        {
            var queue = Substitute.For<IBusMessageReciever>();
            new BusPoller<object>(queue);
        }

        [Test]
        public void ConstructorQueueNull()
        {
            Assert.That(() => new BusPoller<object>(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task Poll()
        {
            var wait = TimeSpan.FromSeconds(10);
            //var msg = new Message("data");
            //var queue = Substitute.For<IBusMessageReciever>();
            //queue.ServerWaitTime.Returns(wait);
            //queue.Get(wait).Returns(Task.FromResult(msg));

            //var poller = new BusPoller<object>(queue);
            //var returned = await poller.Poll();

            //Assert.IsNotNull(returned);

            //queue.Received().Get(wait);

            Assert.Fail();
        }

        [Test]
        public async Task PollGetNull()
        {
            var wait = TimeSpan.FromSeconds(10);
            var queue = Substitute.For<IBusMessageReciever>();
            queue.ServerWaitTime.Returns(wait);
            //queue.Get(wait).Returns(Task.FromResult<Message>(null));

            //var poller = new BusPoller<object>(queue);
            //var returned = await poller.Poll();

            //Assert.IsNull(returned);

            //queue.Received().Get(wait);

            Assert.Fail();
        }

        [Test]
        public void PollGetThrows()
        {
            var wait = TimeSpan.FromSeconds(10);
            //var msg = new Message("data");
            //var queue = Substitute.For<IBusMessageReciever>();
            //queue.ServerWaitTime.Returns(wait);
            //queue.Get(wait).Returns<Message>(x => { throw new ApplicationException(); });

            //var poller = new BusPoller<object>(queue);
            //Assert.That(async () => await poller.Poll(), Throws.TypeOf<ApplicationException>());

            Assert.Fail();
        }

        [Test]
        public async Task PollMany()
        {
            var wait = TimeSpan.FromSeconds(10);
            //var msg = new Message("data");
            //var msgs = new List<Message>(3);
            //msgs.Add(msg);
            //msgs.Add(msg);
            //msgs.Add(msg);

            //var queue = Substitute.For<IBusMessageReciever>();
            //queue.ServerWaitTime.Returns(wait);
            //queue.GetMany(wait, 3).Returns(Task.FromResult<IEnumerable<Message>>(msgs));

            //var poller = new BusPoller<object>(queue);
            //var returned = await poller.PollMany(3);

            //Assert.IsNotNull(returned);
            //Assert.AreEqual(3, returned.Count());

            //queue.Received().GetMany(wait, 3);

            Assert.Fail();
        }

        [Test]
        public async Task PollGetManyNull()
        {
            var wait = TimeSpan.FromSeconds(10);
            var queue = Substitute.For<IBusMessageReciever>();
            queue.ServerWaitTime.Returns(wait);
            //queue.GetMany(wait, 3).Returns(Task.FromResult<IEnumerable<Message>>(null));

            //var poller = new BusPoller<object>(queue);
            //var returned = await poller.PollMany(3);

            //Assert.IsNull(returned);

            //queue.Received().GetMany(wait, 3);

            Assert.Fail();
        }

        [Test]
        public void PollGetManyThrows()
        {
            var wait = TimeSpan.FromSeconds(10);
            //var msg = new Message("data");
            //var queue = Substitute.For<IBusMessageReciever>();
            //queue.ServerWaitTime.Returns(wait);
            //queue.GetMany(wait).Returns<IEnumerable<Message>>(x => { throw new ApplicationException(); });

            //var poller = new BusPoller<object>(queue);
            //Assert.That(async () => await poller.PollMany(), Throws.TypeOf<ApplicationException>());

            Assert.Fail();
        }
    }
}