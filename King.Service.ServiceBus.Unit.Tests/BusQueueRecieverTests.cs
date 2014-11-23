namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusQueueRecieverTests
    {
        const string connection = "Endpoint=sb://test.servicebus.windows.net;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=[your secret]";

        [Test]
        public void Constructor()
        {
            new BusQueueReciever(Guid.NewGuid().ToString(), connection);
        }

        [Test]
        public void IsIBusQueueReciever()
        {
            Assert.IsNotNull(new BusQueueReciever(Guid.NewGuid().ToString(), connection) as IBusQueueReciever);
        }

        [Test]
        public void IsBusQueue()
        {
            Assert.IsNotNull(new BusQueueReciever(Guid.NewGuid().ToString(), connection) as BusQueue);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterForEventsCallbackNull()
        {
            var queue = new BusQueueReciever(Guid.NewGuid().ToString(), connection);
            queue.RegisterForEvents(null, new OnMessageOptions());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterForEventsOptionsNull()
        {
            var queue = new BusQueueReciever(Guid.NewGuid().ToString(), connection);
            queue.RegisterForEvents(this.OnMessageArrived, null);
        }

        private async Task OnMessageArrived(BrokeredMessage message)
        {
            await new TaskFactory().StartNew(() => { });
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public async Task GetThrows()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.Recieve(Arg.Any<TimeSpan>())).Do(x => { throw new Exception(); });

            var q = new BusQueueReciever(Guid.NewGuid().ToString(), m, client);
            await q.Get();
        }

        [Test]
        [ExpectedException(typeof(MessagingException))]
        public async Task GetThrowsMessagingException()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var first = true;
            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.Recieve(Arg.Any<TimeSpan>())).Do(x =>
            {
                var tmp = first;
                first = false;
                throw new MessagingException(Guid.NewGuid().ToString(), tmp, new Exception());
            });

            var q = new BusQueueReciever(Guid.NewGuid().ToString(), m, client);
            await q.Get();
        }

        [Test]
        [ExpectedException(typeof(Exception))]
        public async Task GetManyThrows()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.RecieveBatch(5, Arg.Any<TimeSpan>())).Do(x => { throw new Exception(); });

            var q = new BusQueueReciever(Guid.NewGuid().ToString(), m, client);
            await q.GetMany();
        }

        [Test]
        [ExpectedException(typeof(MessagingException))]
        public async Task GetManyThrowsMessagingException()
        {
            var m = NamespaceManager.CreateFromConnectionString(connection);
            var first = true;
            var client = Substitute.For<IBusQueueClient>();
            client.When(c => c.RecieveBatch(5, Arg.Any<TimeSpan>())).Do(x =>
            {
                var tmp = first;
                first = false;
                throw new MessagingException(Guid.NewGuid().ToString(), tmp, new Exception());
            });

            var q = new BusQueueReciever(Guid.NewGuid().ToString(), m, client);
            await q.GetMany();
        }
    }
}