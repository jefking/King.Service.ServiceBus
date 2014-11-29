namespace King.Service.ServiceBus.Unit.Tests.Wrappers
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus.Messaging;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusQueueClientTests
    {
        const string connection = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";

        [Test]
        public void Constructor()
        {
            var name = Guid.NewGuid().ToString();
            new BusQueueClient(QueueClient.CreateFromConnectionString(connection, name));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorQueueClientNull()
        {
            new BusQueueClient(null);
        }
    }
}