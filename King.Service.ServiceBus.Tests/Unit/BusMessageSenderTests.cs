namespace King.Service.ServiceBus.Test.Unit
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using Models;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusMessageSenderTests
    {
        [Test]
        public void Constructor()
        {
            var c = Substitute.For<IMessageSender>();
            new BusMessageSender(Guid.NewGuid().ToString(), c);
        }

        [Test]
        public void ConstructorNameNull()
        {
            var c = Substitute.For<IMessageSender>();
            Assert.That(() => new BusMessageSender(null, c), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorClientNull()
        {
            Assert.That(() => new BusMessageSender(Guid.NewGuid().ToString(), null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void IsIBusMessageSender()
        {
            var c = Substitute.For<IMessageSender>();
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
        public void SaveMessageNull()
        {
            var c = Substitute.For<IBusTopicClient>();
            var queue = new BusMessageSender(Guid.NewGuid().ToString(), c);
            Assert.That(async () => await queue.Send((Message)null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task Send()
        {
            var msg = new Message();

            var client = Substitute.For<IBusTopicClient>();
            await client.Send(msg);

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send(msg);

            await client.Received().Send(msg);
        }

        [Test]
        public async Task SendBinary()
        {
            var msg = new Message();

            var client = Substitute.For<IBusTopicClient>();
            client.Send(msg);

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send(msg, Encoding.Binary);

            client.Received().Send(msg);
        }

        [Test]
        public async Task SendData()
        {
            var msg = new object();

            var client = Substitute.For<IBusTopicClient>();
            client.Send(Arg.Any<Message>());

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send(msg);

            client.Received().Send(Arg.Any<Message>());
        }

        [Test]
        public async Task SendDataBinary()
        {
            var msg = new object();

            var client = Substitute.For<IBusTopicClient>();
            client.Send(Arg.Any<Message>());

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send(msg, Encoding.Binary);

            client.Received().Send(Arg.Any<Message>());
        }

        [Test]
        public async Task SendMessageAsObject()
        {
            var msg = new Message();

            var client = Substitute.For<IBusTopicClient>();
            client.Send(msg);

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send((object)msg);

            client.Received().Send(msg);
        }

        [Test]
        public async Task SendMessageAsObjectAsBinary()
        {
            var msg = new Message();

            var client = Substitute.For<IBusTopicClient>();
            client.Send(msg);

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send((object)msg, Encoding.Binary);

            client.Received().Send(msg);
        }
        
        [Test]
        public void SendThrows()
        {
            var msg = new Message();

            var client = Substitute.For<IBusTopicClient>();
            client.When(c => c.Send(msg)).Do(x => { throw new Exception(); });

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            Assert.That(async () => await q.Send(msg), Throws.TypeOf<Exception>());
        }

        [Test]
        public void SendThrowsMessagingException()
        {
            var msg = new Message();

            var first = true;
            var client = Substitute.For<IBusTopicClient>();
            client.When(c => c.Send(msg)).Do(x =>
            {
                var tmp = first;
                first = false;
                throw new Exception();
            });

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            Assert.That(async () => await q.Send(msg), Throws.TypeOf<Exception>());
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
            Assert.That(async () => await q.Send((IEnumerable<Message>)null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task SendBatchObjAsMessage()
        {
            var msg = new List<object>();
            msg.Add(new object());

            var client = Substitute.For<IBusQueueClient>();
            client.Send(Arg.Any<IEnumerable<Message>>());

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send(msg);

            client.Received().Send(Arg.Any<IEnumerable<Message>>());
        }

        [Test]
        public async Task SendBatchObjAsMessageBinary()
        {
            var msg = new List<object>();
            msg.Add(new object());

            var client = Substitute.For<IBusQueueClient>();
            client.Send(Arg.Any<IEnumerable<Message>>());

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send(msg, Encoding.Binary);

            client.Received().Send(Arg.Any<IEnumerable<Message>>());
        }

        [Test]
        public async Task SendObjBatch()
        {
            var msg = new List<Message>();
            msg.Add(new Message());

            var client = Substitute.For<IBusQueueClient>();
            client.Send(Arg.Any<IEnumerable<Message>>());

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send((IEnumerable<object>)msg);

            client.Received().Send(Arg.Any<IEnumerable<Message>>());
        }

        [Test]
        public async Task SendObjBatchBinary()
        {
            var msg = new List<Message>();
            msg.Add(new Message());

            var client = Substitute.For<IBusQueueClient>();
            client.Send(Arg.Any<IEnumerable<Message>>());

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send((IEnumerable<object>)msg, Encoding.Binary);

            client.Received().Send(Arg.Any<IEnumerable<Message>>());
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
            client.Send(Arg.Any<Message>());

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send(new BufferedMessage() { ReleaseAt = DateTime.UtcNow });

            client.Received().Send(Arg.Any<Message>());
        }

        [Test]
        public async Task SendForBufferBinary()
        {
            var client = Substitute.For<IBusQueueClient>();
            client.Send(Arg.Any<Message>());

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.Send(new BufferedMessage() { ReleaseAt = DateTime.UtcNow }, Encoding.Binary);

            client.Received().Send(Arg.Any<Message>());
        }

        [Test]
        public void SendBatchThrows()
        {
            var msg = new List<Message>();
            msg.Add(new Message());

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
            client.When(c => c.Send(Arg.Any<IEnumerable<Message>>())).Do(x => { throw new Exception(); });

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            Assert.That(async () => await q.Send(msg), Throws.TypeOf<Exception>());
        }

        [Test]
        public void SendBatchThrowsMessagingException()
        {
            var msgs = new List<Message>();
            msgs.Add(new Message());

            var first = true;
            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.Send(msgs)).Do(x =>
            {
                var tmp = first;
                first = false;
                throw new Exception();
            });

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            Assert.That(async () => await q.Send(msgs), Throws.TypeOf<Exception>());

            Assert.Fail();
        }

        [Test]
        public void SendAtNullMessage()
        {
            var client = Substitute.For<IBusTopicClient>();

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            Assert.That(async () => await q.Send(null, DateTime.UtcNow), Throws.TypeOf<ArgumentNullException>());

        }

        [Test]
        public void BufferedOffset()
        {
            Assert.AreEqual(-6, BusMessageSender.BufferedOffset);
        }

        [Test]
        public async Task SendBuffered()
        {
            var client = Substitute.For<IBusQueueClient>();
            client.Send(Arg.Any<Message>());

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.SendBuffered(new object(), DateTime.UtcNow);

            client.Received().Send(Arg.Any<Message>());
        }

        [Test]
        public async Task SendBufferedDataNull()
        {
            var client = Substitute.For<IBusQueueClient>();

            var q = new BusMessageSender(Guid.NewGuid().ToString(), client);
            await q.SendBuffered(null, DateTime.UtcNow);
        }
    }
}