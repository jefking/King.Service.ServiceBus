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
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
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
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            Assert.That(() => new BusTopic(null, fake), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsIAzureStorage()
        {
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            Assert.IsNotNull(new BusTopic(Guid.NewGuid().ToString(), fake) as IAzureStorage);
        }

        [Test]
        public void Name()
        {
            var name = Guid.NewGuid().ToString();
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            var bt = new BusTopic(name, fake);

            Assert.AreEqual(name, bt.Name);
        }
    }
}