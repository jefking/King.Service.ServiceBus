namespace King.Service.ServiceBus.Unit.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class InitializeTopicTests
    {
        [Test]
        public void Constructor()
        {
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            new InitializeTopic(Guid.NewGuid().ToString(), fake);
        }

        [Test]
        public void ConstructorConnectionStringNull()
        {
            Assert.That(() => new InitializeTopic(Guid.NewGuid().ToString(), null), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorNameNull()
        {
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            Assert.That(() => new InitializeTopic(null, fake), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void IsInitializeTask()
        {
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            Assert.IsNotNull(new InitializeTopic(Guid.NewGuid().ToString(), fake) as InitializeTask);
        }
    }
}