namespace King.Service.ServiceBus.Test.Unit.Wrappers
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BusTopicClientTests
    {
        const string connection = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            var name = Guid.NewGuid().ToString();
            new BusTopicClient(new TopicClient(connection, name));
        }

        [Test]
        public void ConstructorTopicClientNull()
        {
            Assert.That(() => new BusTopicClient(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Client()
        {
            var client = new TopicClient(connection, "test");
            var btc = new BusTopicClient(client);
            Assert.AreEqual(client, btc.Client);
        }
    }
}