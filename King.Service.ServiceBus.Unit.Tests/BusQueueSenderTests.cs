namespace King.Service.ServiceBus.Unit.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Models;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;

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
        public void SaveObjectNull()
        {
            var queue = new BusQueueSender(Guid.NewGuid().ToString(), connection);
            Assert.That(async () => await queue.Send((object)null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void SaveBrokeredMessageNull()
        {
            var queue = new BusQueueSender(Guid.NewGuid().ToString(), connection);
            Assert.That(async () => await queue.Send((BrokeredMessage)null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void SendAt()
        {
            var queue = new BusQueueSender(Guid.NewGuid().ToString(), connection);
            Assert.That(async () => await queue.Send(null, DateTime.UtcNow), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void SendForBufferAt()
        {
            var queue = new BusQueueSender(Guid.NewGuid().ToString(), connection);
            Assert.That(async () => await queue.Send((BufferedMessage)null), Throws.TypeOf<ArgumentNullException>());
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
        public void SendBatchNull()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            Assert.That(async () => await q.Send((IEnumerable<BrokeredMessage>)null), Throws.TypeOf<ArgumentNullException>());
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
        public void SendObjNullBatch()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            Assert.That(async () => await q.Send((IEnumerable<object>)null), Throws.TypeOf<ArgumentNullException>());
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
        public async Task SendThrows()
        {
            var msg = new BrokeredMessage();
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.Send(msg)).Do(x => { throw new Exception(); });

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            Assert.That(async () => await q.Send(msg), Throws.TypeOf<Exception>());
        }

        [Test]
        public void SendBatchThrows()
        {
            var msg = new List<BrokeredMessage>();
            msg.Add(new BrokeredMessage());
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.Send(msg)).Do(x => { throw new Exception(); });

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            Assert.That(async () => await q.Send(msg), Throws.TypeOf<Exception>());
        }

        [Test]
        public void SendObjBatchThrows()
        {
            var msg = new List<object>();
            msg.Add(new object());
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.Send(Arg.Any<IEnumerable<BrokeredMessage>>())).Do(x => { throw new Exception(); });

            var q = new BusQueueSender(Guid.NewGuid().ToString(), m, client);
            Assert.That(async () => await q.Send(msg), Throws.TypeOf<Exception>());
        }

        [Test]
        public void SendThrowsMessagingException()
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
            Assert.That(async () => await q.Send(msg), Throws.TypeOf<MessagingException>());
        }

        [Test]
        public void SendBatchThrowsMessagingException()
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
            Assert.That(async () => await q.Send(msgs), Throws.TypeOf<MessagingException>());
        }
    }
}