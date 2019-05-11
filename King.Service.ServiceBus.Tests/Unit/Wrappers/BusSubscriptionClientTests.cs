namespace King.Service.ServiceBus.Test.Unit.Wrappers
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BusSubscriptionClientTests
    {
        const string connection = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            var topic = Guid.NewGuid().ToString();
            var subscription = Guid.NewGuid().ToString();
            new BusSubscriptionClient(new SubscriptionClient(connection, topic, subscription));
        }

        [Test]
        public void ConstructorSubscriptionClientNull()
        {
            Assert.That(() => new BusSubscriptionClient(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Client()
        {
            var client = new SubscriptionClient(connection, "t", "s");
            var btc = new BusSubscriptionClient(client);
            Assert.AreEqual(client, btc.Client);
        }
    }
}