namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusQueueSenderTests
    {
        const string connection = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";

        [Test]
        public void Constructor()
        {
            new BusQueueSender(Guid.NewGuid().ToString(), connection);
        }

        [Test]
        public void IsIBusQueueSender()
        {
            Assert.IsNotNull(new BusQueueSender(Guid.NewGuid().ToString(), connection) as IBusQueueSender);
        }

        [Test]
        public void IsBusQueue()
        {
            Assert.IsNotNull(new BusQueueSender(Guid.NewGuid().ToString(), connection) as BusQueue);
        }

        [Test]
        public void BufferedOffset()
        {
            Assert.AreEqual(-6, BusQueueSender.BufferedOffset);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveObjectNull()
        {
            var queue = new BusQueueSender(Guid.NewGuid().ToString(), connection);
            await queue.Send((object)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveBrokeredMessageNull()
        {
            var queue = new BusQueueSender(Guid.NewGuid().ToString(), connection);
            await queue.Send((BrokeredMessage)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SendAt()
        {
            var queue = new BusQueueSender(Guid.NewGuid().ToString(), connection);
            await queue.Send(null, DateTime.UtcNow);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SendForBufferAt()
        {
            var queue = new BusQueueSender(Guid.NewGuid().ToString(), connection);
            await queue.Send((BufferedMessage)null);
        }

        [Test]
        public async Task Send()
        {
            var msg = new BrokeredMessage();
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();
            client.Send(msg);

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            await q.Send(msg);

            client.Received().Send(msg);
        }

        [Test]
        public async Task SendBatch()
        {
            var msg = new List<BrokeredMessage>();
            msg.Add(new BrokeredMessage());
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();
            client.Send(msg);

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            await q.Send(msg);

            client.Received().Send(msg);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SendBatchNull()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            await q.Send((IEnumerable<BrokeredMessage>)null);
        }

        [Test]
        public async Task SendBatchObjAsBrokeredMessage()
        {
            var msg = new List<object>();
            msg.Add(new object());
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();
            client.Send(Arg.Any<IEnumerable<BrokeredMessage>>());

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            await q.Send(msg);

            client.Received().Send(Arg.Any<IEnumerable<BrokeredMessage>>());
        }

        [Test]
        public async Task SendObjBatch()
        {
            var msg = new List<BrokeredMessage>();
            msg.Add(new BrokeredMessage());
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();
            client.Send(Arg.Any<IEnumerable<BrokeredMessage>>());

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            await q.Send((IEnumerable<object>)msg);

            client.Received().Send(Arg.Any<IEnumerable<BrokeredMessage>>());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SendObjNullBatch()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            await q.Send((IEnumerable<object>)null);
        }

        [Test]
        public async Task SendForBuffer()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();
            client.Send(Arg.Any<BrokeredMessage>());

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            await q.Send(new BufferedMessage() { ReleaseAt = DateTime.UtcNow });

            client.Received().Send(Arg.Any<BrokeredMessage>());
        }

        [Test]
        public async Task SendBuffered()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();
            client.Send(Arg.Any<BrokeredMessage>());

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            await q.SendBuffered(new object(), DateTime.UtcNow);

            client.Received().Send(Arg.Any<BrokeredMessage>());
        }

        [Test]
        public async Task SendBufferedDataNull()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            await q.SendBuffered(null, DateTime.UtcNow);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public async Task SendThrows()
        {
            var msg = new BrokeredMessage();
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.Send(msg)).Do(x => { throw new Exception(); });

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            await q.Send(msg);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public async Task SendBatchThrows()
        {
            var msg = new List<BrokeredMessage>();
            msg.Add(new BrokeredMessage());
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.Send(msg)).Do(x => { throw new Exception(); });

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            await q.Send(msg);
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public async Task SendObjBatchThrows()
        {
            var msg = new List<object>();
            msg.Add(new object());
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.Send(Arg.Any<IEnumerable<BrokeredMessage>>())).Do(x => { throw new Exception(); });

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            await q.Send(msg);
        }

        [Test]
        [ExpectedException(typeof(MessagingException))]
        public async Task SendThrowsMessagingException()
        {
            var msg = new BrokeredMessage();
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var first = true;
            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.Send(msg)).Do(x =>
            {
                var tmp = first;
                first = false;
                throw new MessagingException(Guid.NewGuid().ToString(), tmp, new Exception());
            });

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            await q.Send(msg);
        }

        [Test]
        [ExpectedException(typeof(MessagingException))]
        public async Task SendBatchThrowsMessagingException()
        {
            var msgs = new List<BrokeredMessage>();
            msgs.Add(new BrokeredMessage());
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var first = true;
            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.Send(msgs)).Do(x =>
            {
                var tmp = first;
                first = false;
                throw new MessagingException(Guid.NewGuid().ToString(), tmp, new Exception());
            });

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            await q.Send(msgs);
        }
    }
}