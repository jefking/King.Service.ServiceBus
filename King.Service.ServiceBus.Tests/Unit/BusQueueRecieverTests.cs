namespace King.Service.ServiceBus.Test.Unit
{
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

    [TestFixture]
    public class BusQueueRecieverTests
    {
        const string connection = Configuration.ConnectionString;

        [Test]
        public void Constructor()
        {
            new BusQueueReciever(Guid.NewGuid().ToString(), connection);
        }

        [Test]
        public void IsIBusQueueReciever()
        {
            Assert.IsNotNull(new BusQueueReciever(Guid.NewGuid().ToString(), connection) as IBusMessageReciever);
        }

        [Test]
        public void IsBusMessageReciever()
        {
            Assert.IsNotNull(new BusQueueReciever(Guid.NewGuid().ToString(), connection) as BusMessageReciever);
        }

        [Test]
        public void RegisterForEventsCallbackNull()
        {
            var queue = new BusQueueReciever(Guid.NewGuid().ToString(), connection);
            //Assert.That(async () => queue.RegisterForEvents(null, new OnMessageOptions()), Throws.TypeOf<ArgumentNullException>());

            Assert.Fail();
        }

        [Test]
        public void RegisterForEventsOptionsNull()
        {
            var queue = new BusQueueReciever(Guid.NewGuid().ToString(), connection);
            //Assert.That(() => queue.RegisterForEvents(this.OnMessageArrived, null), Throws.TypeOf<ArgumentNullException>());

            Assert.Fail();
        }

        private async Task OnMessageArrived(Message message)
        {
            await new TaskFactory().StartNew(() => { });
        }

        [Test]
        public void GetThrows()
        {
            var wait = TimeSpan.FromSeconds(10);

            var client = Substitute.For<IBusQueueClient>();
            //client.When(c => c.Recieve(Arg.Any<TimeSpan>())).Do(x => { throw new Exception(); });

            //var q = new BusQueueReciever(client);
            //Assert.That(async () => await q.Get(wait), Throws.TypeOf<Exception>());

            Assert.Fail();
        }

        [Test]
        public void GetThrowsMessagingException()
        {
            var wait = TimeSpan.FromSeconds(10);

            var first = true;
            var client = Substitute.For<IBusQueueClient>();
            //client.When(c => c.Recieve(Arg.Any<TimeSpan>())).Do(x =>
            //{
            //    var tmp = first;
            //    first = false;
            //    throw new MessagingException(Guid.NewGuid().ToString(), tmp, new Exception());
            //});

            //var q = new BusQueueReciever(client);
            //Assert.That(async () => await q.Get(wait), Throws.TypeOf<MessagingException>());

            Assert.Fail();
        }

        [Test]
        public void GetManyThrows()
        {
            var wait = TimeSpan.FromSeconds(10);

            var client = Substitute.For<IBusQueueClient>();
            //client.When(c => c.RecieveBatch(5, Arg.Any<TimeSpan>())).Do(x => { throw new Exception(); });

            //var q = new BusQueueReciever(client);
            //Assert.That(async () => await q.GetMany(wait), Throws.TypeOf<Exception>());

            Assert.Fail();
        }

        [Test]
        public void GetManyThrowsMessagingException()
        {
            var wait = TimeSpan.FromSeconds(10);
            //var m = NamespaceManager.CreateFromConnectionString(connection);
            //var first = true;
            //var client = Substitute.For<IBusQueueClient>();
            //client.When(c => c.RecieveBatch(5, Arg.Any<TimeSpan>())).Do(x =>
            //{
            //    var tmp = first;
            //    first = false;
            //    throw new MessagingException(Guid.NewGuid().ToString(), tmp, new Exception());
            //});

            //var q = new BusQueueReciever(client);
            //Assert.That(async () => await q.GetMany(wait), Throws.TypeOf<MessagingException>());

            Assert.Fail();
        }
    }
}