namespace King.Service.ServiceBus.Test.Unit
{
    using King.Service.ServiceBus;
    using King.Service.ServiceBus.Wrappers;
    using Microsoft.Azure.ServiceBus;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading;
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
            var m = Substitute.For<SessionHandlerOptions>();
            var queue = new BusQueueReciever(Guid.NewGuid().ToString(), connection);
            Assert.That(async () => queue.RegisterForEvents(null, m), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void RegisterForEventsOptionsNull()
        {
            var queue = new BusQueueReciever(Guid.NewGuid().ToString(), connection);
            Assert.That(() => queue.RegisterForEvents(this.OnMessageArrived, null), Throws.TypeOf<ArgumentNullException>());
        }

        private async Task OnMessageArrived(IMessageSession ms, Message m, CancellationToken ct)
        {
            await new TaskFactory().StartNew(() => { });
        }
    }
}