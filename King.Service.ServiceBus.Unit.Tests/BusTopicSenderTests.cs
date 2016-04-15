namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusTopicSenderTests
    {
        const string connection = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";

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
        public void IsITopicSender()
        {
            Assert.IsNotNull(new BusTopicSender(Guid.NewGuid().ToString(), connection) as ITopicSender);
        }

        [Test]
        public void ConstructorMockableNameNull()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusTopicClient>();
            Assert.That(() => new BusTopicSender(null, m, client), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ConstructorManagerNull()
        {
            var client = Substitute.For<IBusTopicClient>();
            Assert.That(() => new BusTopicSender(Guid.NewGuid().ToString(), null, client), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ConstructorClientNull()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            Assert.That(() => new BusTopicSender(Guid.NewGuid().ToString(), m, null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void SaveObjectNull()
        {
            var queue = new BusTopicSender(Guid.NewGuid().ToString(), connection);
            Assert.That(async () => await queue.Send((object)null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void SaveBrokeredMessageNull()
        {
            var queue = new BusTopicSender(Guid.NewGuid().ToString(), connection);
            Assert.That(async () => await queue.Send((BrokeredMessage)null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task Send()
        {
            var msg = new BrokeredMessage();
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusTopicClient>();
            client.Send(msg);

            var q = new BusTopicSender(Guid.NewGuid().ToString(), m, client);
            await q.Send(msg);

            client.Received().Send(msg);
        }

        [Test]
        public async Task SendData()
        {
            var msg = new object();
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusTopicClient>();
            client.Send(Arg.Any<BrokeredMessage>());

            var q = new BusTopicSender(Guid.NewGuid().ToString(), m, client);
            await q.Send(msg);

            client.Received().Send(Arg.Any<BrokeredMessage>());
        }

        [Test]
        public async Task SendBrokeredMessageAsObject()
        {
            var msg = new BrokeredMessage();
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusTopicClient>();
            client.Send(msg);

            var q = new BusTopicSender(Guid.NewGuid().ToString(), m, client);
            await q.Send((object)msg);

            client.Received().Send(msg);
        }

        [Test]
        public void HandleTransientError()
        {
            this.exception = null;
            var ex = new MessagingException("hahaha");

            var bq = new BusTopicSender(Guid.NewGuid().ToString(), connection);
            bq.TransientErrorOccured += this.Error;
            bq.HandleTransientError(ex);

            Assert.AreEqual(ex, this.exception);
        }

        [Test]
        public void HandleTransientErrorNull()
        {
            this.exception = null;
            var ex = new MessagingException("hahaha");

            var bq = new BusTopicSender(Guid.NewGuid().ToString(), connection);
            bq.TransientErrorOccured += this.Error;
            bq.HandleTransientError(null);

            Assert.IsNull(this.exception);
        }

        Exception exception = null;

        private void Error(object obj, TransientErrorArgs args)
        {
            this.exception = args.Exception;
        }

        [Test]
        public void SendThrows()
        {
            var msg = new BrokeredMessage();
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusTopicClient>();
            client.When(c => c.Send(msg)).Do(x => { throw new Exception(); });

            var q = new BusTopicSender(Guid.NewGuid().ToString(), m, client);
            Assert.That(async () => await q.Send(msg), Throws.TypeOf<Exception>());
        }

        [Test]
        public async Task SendThrowsMessagingException()
        {
            var msg = new BrokeredMessage();
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var first = true;
            var client = Substitute.For<IBusTopicClient>();
            client.When(c => c.Send(msg)).Do(x =>
            {
                var tmp = first;
                first = false;
                throw new MessagingException(Guid.NewGuid().ToString(), tmp, new Exception());
            });

            var q = new BusTopicSender(Guid.NewGuid().ToString(), m, client);
            Assert.That(async () => await q.Send(msg), Throws.TypeOf<MessagingException>());
        }
    }
}