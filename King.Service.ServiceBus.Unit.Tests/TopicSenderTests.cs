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
    public class TopicSenderTests
    {
        const string connection = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";

        [Test]
        public void Constructor()
        {
            new TopicSender(Guid.NewGuid().ToString(), connection);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNameNull()
        {
            new TopicSender(null, connection);
        }

        [Test]
        public void IsITopicSender()
        {
            Assert.IsNotNull(new TopicSender(Guid.NewGuid().ToString(), connection) as ITopicSender);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorMockableNameNull()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusTopicClient>();
            new TopicSender(null, m, client);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorManagerNull()
        {
            var client = Substitute.For<IBusTopicClient>();
            new TopicSender(Guid.NewGuid().ToString(), null, client);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorClientNull()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            new TopicSender(Guid.NewGuid().ToString(), m, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveObjectNull()
        {
            var queue = new TopicSender(Guid.NewGuid().ToString(), connection);
            await queue.Send((object)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveBrokeredMessageNull()
        {
            var queue = new TopicSender(Guid.NewGuid().ToString(), connection);
            await queue.Send((BrokeredMessage)null);
        }

        [Test]
        public async Task Send()
        {
            var msg = new BrokeredMessage();
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusTopicClient>();
            client.Send(msg);

            var q = new TopicSender(Guid.NewGuid().ToString(), m, client);
            await q.Send(msg);

            client.Received().Send(msg);
        }

        [Test]
        public void HandleTransientError()
        {
            this.exception = null;
            var ex = new MessagingException("hahaha");

            var bq = new TopicSender(Guid.NewGuid().ToString(), connection);
            bq.TransientErrorOccured += this.Error;
            bq.HandleTransientError(ex);

            Assert.AreEqual(ex, this.exception);
        }

        [Test]
        public void HandleTransientErrorNull()
        {
            this.exception = null;
            var ex = new MessagingException("hahaha");

            var bq = new TopicSender(Guid.NewGuid().ToString(), connection);
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