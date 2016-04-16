namespace King.Service.ServiceBus.Unit.Tests
{
    using System;
    using Azure.Data;
    using NUnit.Framework;

    [TestFixture]
    public class BusTopicSubscriberTests
    {
        const string connection = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";

        [Test]
        public void Constructor()
        {
            new BusTopicSubscriber(Guid.NewGuid().ToString(), connection, Guid.NewGuid().ToString());
        }

        [Test]
        public void ConstructorNameNull()
        {
            Assert.That(() => new BusTopicSubscriber(null, connection, Guid.NewGuid().ToString()), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorConnectionNull()
        {
            Assert.That(() => new BusTopicSubscriber(Guid.NewGuid().ToString(), null, Guid.NewGuid().ToString()), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorSubscriptionNameNull()
        {
            Assert.That(() => new BusTopicSubscriber(Guid.NewGuid().ToString(), connection, null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsIAzureStorage()
        {
            Assert.IsNotNull(new BusTopicSubscriber(Guid.NewGuid().ToString(), connection, Guid.NewGuid().ToString()) as IAzureStorage);
        }

        [Test]
        public void Name()
        {
            var name = Guid.NewGuid().ToString();
            var s = new BusTopicSubscriber(Guid.NewGuid().ToString(), connection, name);

            Assert.AreEqual(name, s.Name);
        }

        [Test]
        public void TopicName()
        {
            var name = Guid.NewGuid().ToString();
            var s = new BusTopicSubscriber(name, connection, Guid.NewGuid().ToString());

            Assert.AreEqual(name, s.TopicName);
        }
    }
}