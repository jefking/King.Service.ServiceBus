namespace King.Service.ServiceBus.Unit.Tests
{
    using King.Service.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using System.Threading.Tasks;

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
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveObjectNull()
        {
            var queue = new BusQueue(Guid.NewGuid().ToString(), connection);
            await queue.Send((object)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SaveBrokeredMessageNull()
        {
            var queue = new BusQueue(Guid.NewGuid().ToString(), connection);
            await queue.Send((BrokeredMessage)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterForEventsCallbackNull()
        {
            var queue = new BusQueue(Guid.NewGuid().ToString(), connection);
            queue.RegisterForEvents(null, new OnMessageOptions());
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RegisterForEventsOptionsNull()
        {
            var queue = new BusQueue(Guid.NewGuid().ToString(), connection);
            queue.RegisterForEvents(this.OnMessageArrived, null);
        }

        private async Task OnMessageArrived(BrokeredMessage message)
        {
            await new TaskFactory().StartNew(() => { });
        }
    }
}