namespace King.Service.ServiceBus.Unit.Tests.Wrappers
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BusTopicClientTests
    {
        const string connection = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";

        [Test]
        public void Constructor()
        {
            var name = Guid.NewGuid().ToString();
            new BusTopicClient(TopicClient.CreateFromConnectionString(connection, name));
        }

        [Test]
        public void ConstructorQueueClientNull()
        {
            Assert.That(() => new BusTopicClient(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Client()
        {
            var client = TopicClient.CreateFromConnectionString(connection, "test");
            var btc = new BusTopicClient(client);
            Assert.AreEqual(client, btc.Client);
        }
    }
}