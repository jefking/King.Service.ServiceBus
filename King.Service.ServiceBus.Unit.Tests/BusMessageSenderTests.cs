namespace King.Service.ServiceBus.Unit.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using Models;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BusMessageSenderTests
    {
        [Test]
        public void Constructor()
        {
            var c = Substitute.For<IBusSender>();
            new BusMessageSender(Guid.NewGuid().ToString(), c);
        }

        [Test]
        public void ConstructorNameNull()
        {
            var c = Substitute.For<IBusSender>();
            Assert.That(() => new BusMessageSender(null, c), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorClientNull()
        {
            Assert.That(() => new BusMessageSender(Guid.NewGuid().ToString(), null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsTransientErrorHandler()
        {
            var c = Substitute.For<IBusSender>();
            Assert.IsNotNull(new BusMessageSender(Guid.NewGuid().ToString(), c) as TransientErrorHandler);
        }

        [Test]
        public void IsIBusMessageSender()
        {
            var c = Substitute.For<IBusSender>();
            Assert.IsNotNull(new BusMessageSender(Guid.NewGuid().ToString(), c) as IBusMessageSender);
        }

        [Test]
        public void ConstructorMockableNameNull()
        {
            var client = Substitute.For<IBusTopicClient>();
            Assert.That(() => new BusMessageSender(null, client), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void SaveObjectNull()
        {
            var c = Substitute.For<IBusTopicClient>();
            var queue = new BusMessageSender(Guid.NewGuid().ToString(), c);
            Assert.That(async () => await queue.Send((object)null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void SaveBrokeredMessageNull()
        {
            var c = Substitute.For<IBusTopicClient>();
            var queue = new BusMessageSender(Guid.NewGuid().ToString(), c);
            Assert.That(async () => await queue.Send((BrokeredMessage)null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task Send()
        {
            var msg = new BrokeredMessage();

            var client = Substitute.For<IBusTopicClient>();
            client.Send(msg);

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send(msg);

            client.Received().Send(msg);
        }

        [Test]
        public async Task SendData()
        {
            var msg = new object();

            var client = Substitute.For<IBusTopicClient>();
            client.Send(Arg.Any<BrokeredMessage>());

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send(msg);

            client.Received().Send(Arg.Any<BrokeredMessage>());
        }

        [Test]
        public async Task SendBrokeredMessageAsObject()
        {
            var msg = new BrokeredMessage();

            var client = Substitute.For<IBusTopicClient>();
            client.Send(msg);

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send((object)msg);

            client.Received().Send(msg);
        }

        [Test]
        public void HandleTransientError()
        {
            this.exception = null;
            var ex = new MessagingException("hahaha");

            var c = Substitute.For<IBusTopicClient>();

            var bq = new BusMessageSender(Guid.NewGuid().ToString(), c);
            bq.TransientErrorOccured += this.Error;
            bq.HandleTransientError(ex);

            Assert.AreEqual(ex, this.exception);
        }

        [Test]
        public void HandleTransientErrorNull()
        {
            this.exception = null;
            var ex = new MessagingException("hahaha");

            var c = Substitute.For<IBusTopicClient>();

            var bq = new BusMessageSender(Guid.NewGuid().ToString(), c);
            bq.TransientErrorOccured += this.Error;
            bq.HandleTransientError(null);

            Assert.IsNull(this.exception);
        }

        Exception exception = null;

        private void Error(object obj, TransientErrorArgs args)
        {
            this.exception = args.Exception;
        }

        [Test]
        public void SendThrows()
        {
            var msg = new BrokeredMessage();

            var client = Substitute.For<IBusTopicClient>();
            client.When(c => c.Send(msg)).Do(x => { throw new Exception(); });

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            Assert.That(async () => await q.Send(msg), Throws.TypeOf<Exception>());
        }

        [Test]
        public async Task SendThrowsMessagingException()
        {
            var msg = new BrokeredMessage();

            var first = true;
            var client = Substitute.For<IBusTopicClient>();
            client.When(c => c.Send(msg)).Do(x =>
            {
                var tmp = first;
                first = false;
                throw new MessagingException(Guid.NewGuid().ToString(), tmp, new Exception());
            });

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            Assert.That(async () => await q.Send(msg), Throws.TypeOf<MessagingException>());
        }

        [Test]
        public void SendForBufferAt()
        {
            var client = Substitute.For<IBusTopicClient>();
            var queue = new BusMessageSender(Guid.NewGuid().ToString(), client);
            Assert.That(async () => await queue.Send((BufferedMessage)null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void SendBatchNull()
        {
            var client = Substitute.For<IBusQueueClient>();

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            Assert.That(async () => await q.Send((IEnumerable<BrokeredMessage>)null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task SendBatchObjAsBrokeredMessage()
        {
            var msg = new List<object>();
            msg.Add(new object());

            var client = Substitute.For<IBusQueueClient>();
            client.Send(Arg.Any<IEnumerable<BrokeredMessage>>());

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send(msg);

            client.Received().Send(Arg.Any<IEnumerable<BrokeredMessage>>());
        }

        [Test]
        public async Task SendObjBatch()
        {
            var msg = new List<BrokeredMessage>();
            msg.Add(new BrokeredMessage());

            var client = Substitute.For<IBusQueueClient>();
            client.Send(Arg.Any<IEnumerable<BrokeredMessage>>());

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send((IEnumerable<object>)msg);

            client.Received().Send(Arg.Any<IEnumerable<BrokeredMessage>>());
        }

        [Test]
        public void SendObjNullBatch()
        {
            var client = Substitute.For<IBusQueueClient>();

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            Assert.That(async () => await q.Send((IEnumerable<object>)null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task SendForBuffer()
        {
            var client = Substitute.For<IBusQueueClient>();
            client.Send(Arg.Any<BrokeredMessage>());

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send(new BufferedMessage() { ReleaseAt = DateTime.UtcNow });

            client.Received().Send(Arg.Any<BrokeredMessage>());
        }

        [Test]
        public void SendBatchThrows()
        {
            var msg = new List<BrokeredMessage>();
            msg.Add(new BrokeredMessage());

            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.Send(msg)).Do(x => { throw new Exception(); });

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            Assert.That(async () => await q.Send(msg), Throws.TypeOf<Exception>());
        }

        [Test]
        public void SendObjBatchThrows()
        {
            var msg = new List<object>();
            msg.Add(new object());

            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.Send(Arg.Any<IEnumerable<BrokeredMessage>>())).Do(x => { throw new Exception(); });

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            Assert.That(async () => await q.Send(msg), Throws.TypeOf<Exception>());
        }

        [Test]
        public void SendBatchThrowsMessagingException()
        {
            var msgs = new List<BrokeredMessage>();
            msgs.Add(new BrokeredMessage());

            var first = true;
            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.Send(msgs)).Do(x =>
            {
                var tmp = first;
                first = false;
                throw new MessagingException(Guid.NewGuid().ToString(), tmp, new Exception());
            });

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            Assert.That(async () => await q.Send(msgs), Throws.TypeOf<MessagingException>());
        }

        [Test]
        public void SendAtNullMessage()
        {
            var client = Substitute.For<IBusTopicClient>();

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            Assert.That(async () => await q.Send(null, DateTime.UtcNow), Throws.TypeOf<ArgumentNullException>());

        }
    }
}