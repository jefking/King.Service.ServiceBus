namespace King.Service.ServiceBus.Test.Unit
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class BusTopicSenderTests
    {
        const string connection = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            new BusTopicSender(Guid.NewGuid().ToString(), connection);
        }

        [Test]
        public void ConstructorNameNull()
        {
            Assert.That(() => new BusTopicSender(null, connection), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsBusMessageSender()
        {
            Assert.IsNotNull(new BusTopicSender(Guid.NewGuid().ToString(), connection) as BusMessageSender);
        }
    }
}