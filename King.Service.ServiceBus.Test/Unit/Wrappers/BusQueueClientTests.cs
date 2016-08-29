namespace King.Service.ServiceBus.Test.Unit.Wrappers
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BusQueueClientTests
    {
        const string connection = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            var name = Guid.NewGuid().ToString();
            new BusQueueClient(QueueClient.CreateFromConnectionString(connection, name));
        }

        [Test]
        public void ConstructorQueueClientNull()
        {
            Assert.That(() => new BusQueueClient(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Client()
        {
            var client = QueueClient.CreateFromConnectionString(connection, "test");
            var btc = new BusQueueClient(client);
            Assert.AreEqual(client, btc.Client);
        }

        [Test]
        public void EncodingKey()
        {
            Assert.AreEqual("encoding", BusQueueClient.EncodingKey);
        }
    }
}