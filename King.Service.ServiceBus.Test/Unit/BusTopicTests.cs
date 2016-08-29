namespace King.Service.ServiceBus.Test.Unit
{
    using System;
    using King.Azure.Data;
    using NUnit.Framework;

    [TestFixture]
    public class BusTopicTests
    {
        [Test]
        public void Constructor()
        {
            var fake = Configuration.ConnectionString;
            new BusTopic(Guid.NewGuid().ToString(), fake);
        }

        [Test]
        public void ConstructorConnectionStringNull()
        {
            Assert.That(() => new BusTopic(Guid.NewGuid().ToString(), null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorNameNull()
        {
            var fake = Configuration.ConnectionString;
            Assert.That(() => new BusTopic(null, fake), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsIAzureStorage()
        {
            var fake = Configuration.ConnectionString;
            Assert.IsNotNull(new BusTopic(Guid.NewGuid().ToString(), fake) as IAzureStorage);
        }

        [Test]
        public void Name()
        {
            var name = Guid.NewGuid().ToString();
            var fake = Configuration.ConnectionString;
            var bt = new BusTopic(name, fake);

            Assert.AreEqual(name, bt.Name);
        }
    }
}