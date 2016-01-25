namespace King.Service.ServiceBus.Unit.Tests
{
    using System;
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;

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
        public void ConstructorNameNull()
        {
            Assert.That(() => new BusQueue(null, connection), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorMockableNameNull()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();
            Assert.That(() => new BusQueue(null, m, client), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorManagerNull()
        {
            var client = Substitute.For<IBusQueueClient>();
            Assert.That(() => new BusQueue(Guid.NewGuid().ToString(), null, client), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorClientNull()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            Assert.That(() => new BusQueue(Guid.NewGuid().ToString(), m, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Name()
        {
            var expected = Guid.NewGuid().ToString();
            var bq = new BusQueue(expected, connection);
            Assert.AreEqual(expected, bq.Name);
        }

        [Test]
        public void Client()
        {
            var expected = Substitute.For<IBusQueueClient>();
            var bq = new BusQueue(Guid.NewGuid().ToString(), NamespaceManager.CreateFromConnectionString(connection), expected);
            Assert.AreEqual(expected, bq.Client);
        }

        [Test]
        public void Manager()
        {
            var expected = NamespaceManager.CreateFromConnectionString(connection);
            var bq = new BusQueue(Guid.NewGuid().ToString(), expected, Substitute.For<IBusQueueClient>());
            Assert.AreEqual(expected, bq.Manager);
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

        private void Error(object obj, TransientErrorArgs args)
        {
            this.exception = args.Exception;
        }
    }
}