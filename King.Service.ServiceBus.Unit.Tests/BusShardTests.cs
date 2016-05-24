namespace King.Service.ServiceBus.Unit.Tests
{
    using System;
    using King.Azure.Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BusShardTests
    {
        [Test]
        public void Constructor()
        {
            var r = Substitute.For<IAzureStorage>();
            var s = Substitute.For<IBusMessageSender>();
            new BusShard(r, s);
        }

        [Test]
        public void ConstructorResourcesNull()
        {
            var s = Substitute.For<IBusMessageSender>();

            Assert.That(() => new BusShard(null, s), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorSenderNull()
        {
            var r = Substitute.For<IAzureStorage>();

            Assert.That(() => new BusShard(r, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Resource()
        {
            var r = Substitute.For<IAzureStorage>();
            var s = Substitute.For<IBusMessageSender>();
            var bs = new BusShard(r, s);

            Assert.AreEqual(r, bs.Resource);
        }

        [Test]
        public void Sender()
        {
            var r = Substitute.For<IAzureStorage>();
            var s = Substitute.For<IBusMessageSender>();
            var bs = new BusShard(r, s);

            Assert.AreEqual(s, bs.Sender);
        }
    }
}