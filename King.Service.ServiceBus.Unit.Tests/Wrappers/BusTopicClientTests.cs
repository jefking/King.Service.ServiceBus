namespace King.Service.ServiceBus.Unit.Tests.Wrappers
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorQueueClientNull()
        {
            new BusTopicClient(null);
        }

        [Test]
        public void ConstructorQueueClientNull()
        {
            var client = TopicClient.CreateFromConnectionString(connection, "test");
            var btc = new BusTopicClient(client);
            Assert.AreEqual(client, btc.Client);
        }
    }
}