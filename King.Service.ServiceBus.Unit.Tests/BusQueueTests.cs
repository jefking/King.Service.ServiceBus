namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
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

        [Test]
        public void HandleTransientError()
        {
            this.exception = null;
            var ex = new MessagingException("hahaha");

            var bq = new BusQueue(Guid.NewGuid().ToString(), connection);
            bq.TransientErrorOccured += this.Error;
            bq.HandleTransientError(ex);

            Assert.AreEqual(ex, this.exception);
        }

        [Test]
        public void HandleTransientErrorNull()
        {
            this.exception = null;
            var ex = new MessagingException("hahaha");

            var bq = new BusQueue(Guid.NewGuid().ToString(), connection);
            bq.TransientErrorOccured += this.Error;
            bq.HandleTransientError(null);

            Assert.IsNull(this.exception);
        }

        Exception exception = null;
        private void Error(object obj, MessagingException ex)
        {
            this.exception = ex;
        }
    }
}