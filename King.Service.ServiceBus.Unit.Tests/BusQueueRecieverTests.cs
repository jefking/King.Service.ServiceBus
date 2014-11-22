namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
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
    }
}