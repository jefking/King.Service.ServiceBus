namespace King.Service.ServiceBus.Unit.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class InitializeHubTests
    {
        [Test]
        public void Constructor()
        {
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            new InitializeHub(Guid.NewGuid().ToString(), fake);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorConnectionStringNull()
        {
            new InitializeHub(Guid.NewGuid().ToString(), null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNameNull()
        {
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            new InitializeHub(null, fake);
        }

        [Test]
        public void IsInitializeTask()
        {
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            Assert.IsNotNull(new InitializeHub(Guid.NewGuid().ToString(), fake) as InitializeTask);
        }

        [Test]
        public void PartitionCountTooSmall()
        {
            var rand = new Random();
            var count = rand.Next(int.MinValue, 8);
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            var t = new InitializeHub(Guid.NewGuid().ToString(), fake, count);
            Assert.AreEqual(8, t.PartitionCount);
        }

        [Test]
        public void PartitionCountTooBig()
        {
            var rand = new Random();
            var count = rand.Next(32, int.MaxValue);
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            var t = new InitializeHub(Guid.NewGuid().ToString(), fake, count);
            Assert.AreEqual(32, t.PartitionCount);
        }

        [Test]
        public void PartitionCount()
        {
            var rand = new Random();
            var count = rand.Next(8, 32);
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            var t = new InitializeHub(Guid.NewGuid().ToString(), fake, count);
            Assert.AreEqual(count, t.PartitionCount);
        }
    }
}