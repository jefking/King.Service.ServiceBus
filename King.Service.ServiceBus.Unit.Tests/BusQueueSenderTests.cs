namespace King.Service.ServiceBus.Unit.Tests
{
    using System;
    using System.Threading.Tasks;
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Wrappers;
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
        public void IsBusMessageSender()
        {
            Assert.IsNotNull(new BusQueueSender(Guid.NewGuid().ToString(), connection) as BusMessageSender);
        }

        [Test]
        public void BufferedOffset()
        {
            Assert.AreEqual(-6, BusQueueSender.BufferedOffset);
        }

        [Test]
        public async Task SendBuffered()
        {
            var client = Substitute.For<IBusQueueClient>();
            client.Send(Arg.Any<BrokeredMessage>());

            var q = new BusQueueSender(Guid.NewGuid().ToString(), client);
            await q.SendBuffered(new object(), DateTime.UtcNow);

            client.Received().Send(Arg.Any<BrokeredMessage>());
        }

        [Test]
        public async Task SendBufferedDataNull()
        {
            var client = Substitute.For<IBusQueueClient>();

            var q = new BusQueueSender(Guid.NewGuid().ToString(), client);
            await q.SendBuffered(null, DateTime.UtcNow);
        }
    }
}