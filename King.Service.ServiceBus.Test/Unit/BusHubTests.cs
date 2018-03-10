namespace King.Service.ServiceBus.Test.Unit
{
    using System;
    using global::Azure.Data.Wrappers;
    using NUnit.Framework;

    [TestFixture]
    public class BusHubTests
    {
        [Test]
        public void Constructor()
        {
            var fake = Configuration.ConnectionString;
            new BusHub(Guid.NewGuid().ToString(), fake);
        }

        [Test]
        public void ConstructorConnectionStringNull()
        {
            Assert.That(() => new BusHub(Guid.NewGuid().ToString(), null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorNameNull()
        {
            var fake = Configuration.ConnectionString;
            Assert.That(() => new BusHub(null, fake), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsInitializeTask()
        {
            var fake = Configuration.ConnectionString;
            Assert.IsNotNull(new BusHub(Guid.NewGuid().ToString(), fake) as IAzureStorage);
        }

        [Test]
        public void Name()
        {
            var name = Guid.NewGuid().ToString();
            var fake = Configuration.ConnectionString;
            var bt = new BusHub(name, fake);

            Assert.AreEqual(name, bt.Name);
        }

        [Test]
        public void PartitionCount()
        {
            var random = new Random();
            var pc = random.Next(8, 32);
            var fake = Configuration.ConnectionString;
            var bt = new BusHub(Guid.NewGuid().ToString(), fake, pc);

            Assert.AreEqual(pc, bt.PartitionCount);
        }

        [Test]
        public void PartitionCountLow()
        {
            var random = new Random();
            var pc = random.Next(0, 8);
            var fake = Configuration.ConnectionString;
            var bt = new BusHub(Guid.NewGuid().ToString(), fake, pc);

            Assert.AreEqual(8, bt.PartitionCount);
        }

        [Test]
        public void PartitionCountHigh()
        {
            var random = new Random();
            var pc = random.Next(32, 200);
            var fake = Configuration.ConnectionString;
            var bt = new BusHub(Guid.NewGuid().ToString(), fake, pc);

            Assert.AreEqual(32, bt.PartitionCount);
        }
    }
}