namespace King.Service.ServiceBus.Test.Unit.Wrappers
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class HubClientTests
    {
        const string connection = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            var name = Guid.NewGuid().ToString();
            new HubClient(EventHubClient.CreateFromConnectionString(connection, name));
        }

        [Test]
        public void ConstructorQueueClientNull()
        {
            Assert.That(() => new HubClient(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Client()
        {
            var client = EventHubClient.CreateFromConnectionString(connection, "test");
            var btc = new HubClient(client);
            Assert.AreEqual(client, btc.Client);
        }
    }
}