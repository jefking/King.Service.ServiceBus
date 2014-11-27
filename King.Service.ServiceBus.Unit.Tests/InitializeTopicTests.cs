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
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorConnectionStringNull()
        {
            new InitializeTopic(Guid.NewGuid().ToString(), null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNameNull()
        {
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            new InitializeTopic(null, fake);
        }

        [Test]
        public void IsInitializeTask()
        {
            var fake = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";
            Assert.IsNotNull(new InitializeTopic(Guid.NewGuid().ToString(), fake) as InitializeTask);
        }
    }
}