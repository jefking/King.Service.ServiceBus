namespace King.Service.ServiceBus.Unit.Tests.Wrappers
{
    using System;
    using System.Threading.Tasks;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BusTopicSubscriptionClientTests
    {
        const string connection = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";

        [Test]
        public void Constructor()
        {
            var name = Guid.NewGuid().ToString();
            var topicPath = Guid.NewGuid().ToString();
            new BusTopicSubscriptionClient(SubscriptionClient.CreateFromConnectionString(connection, topicPath, name));
        }

        [Test]
        public void ConstructorQueueClientNull()
        {
            Assert.That(() => new BusTopicSubscriptionClient(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void RegisterForEvents()
        {
            var name = Guid.NewGuid().ToString();
            var topicPath = Guid.NewGuid().ToString();
            var c = new BusTopicSubscriptionClient(SubscriptionClient.CreateFromConnectionString(connection, topicPath, name));
            c.RegisterForEvents(Arg.Any<Func<BrokeredMessage, Task>>(), Arg.Any<OnMessageOptions>());
        }
    }
}