namespace King.Service.ServiceBus.Test.Unit
{
    using System;
    using King.Service.ServiceBus;
    using NUnit.Framework;

    [TestFixture]
    public class BusQueueSenderTests
    {
        const string connection = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            new BusQueueSender(Guid.NewGuid().ToString(), connection);
        }

        [Test]
        public void IsBusMessageSender()
        {
            Assert.IsNotNull(new BusQueueSender(Guid.NewGuid().ToString(), connection) as BusMessageSender);
        }
    }
}