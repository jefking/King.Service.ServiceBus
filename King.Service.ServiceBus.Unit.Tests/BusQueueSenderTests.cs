﻿namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;
    using System;
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
    }
}