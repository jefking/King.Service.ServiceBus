namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class BusQueueTests
    {
        const string connection = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";

        [Test]
        public void Constructor()
        {
            new BusQueue(Guid.NewGuid().ToString(), connection);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNameNull()
        {
            new BusQueue(null, connection);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorConnectionStringNull()
        {
            new BusQueue(Guid.NewGuid().ToString(), null);
        }
    }
}