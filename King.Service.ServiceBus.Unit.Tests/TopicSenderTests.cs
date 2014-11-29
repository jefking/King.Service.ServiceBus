namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class TopicSenderTests
    {
        const string connection = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";

        [Test]
        public void Constructor()
        {
            new TopicSender(Guid.NewGuid().ToString(), connection);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNameNull()
        {
            new TopicSender(null, connection);
        }

        [Test]
        public void IsITopicSender()
        {
            Assert.IsNotNull(new TopicSender(Guid.NewGuid().ToString(), connection) as ITopicSender);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMockableNameNull()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusTopicClient>();
            new TopicSender(null, m, client);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorManagerNull()
        {
            var client = Substitute.For<IBusTopicClient>();
            new TopicSender(Guid.NewGuid().ToString(), null, client);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorClientNull()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            new TopicSender(Guid.NewGuid().ToString(), m, null);
        }
    }
}