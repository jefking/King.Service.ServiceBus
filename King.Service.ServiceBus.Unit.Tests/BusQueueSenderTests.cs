namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
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
    }
}