namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class TopicSubscriberTests
    {
        const string connection = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";

        [Test]
        public void Constructor()
        {
            new TopicSubscriber(Guid.NewGuid().ToString(), connection, Guid.NewGuid().ToString());
        }

        [Test]
        public void ConstructorNameNull()
        {
            Assert.That(() => new TopicSubscriber(null, connection, Guid.NewGuid().ToString()), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorConnectionNull()
        {
            Assert.That(() => new TopicSubscriber(Guid.NewGuid().ToString(), null, Guid.NewGuid().ToString()), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorSubscriptionNameNull()
        {
            Assert.That(() => new TopicSubscriber(Guid.NewGuid().ToString(), connection, null), Throws.TypeOf<ArgumentException>());
        }
    }
}